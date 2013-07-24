using aspdev.repaem.ViewModel;
using Dapper.Data;
using DapperExtensions;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace aspdev.repaem.Models.Data
{
    //TODO: create database structure from files
    public class Database : DbContext, IDatabase
    {
        const string connection = "localhost";
        //Використовуэться в двух місцях, отже перенесемо сюди
        const string sqlGetBases = @"
SELECT {0} rp.Id as Id, 
	rp.Name as Name, 
	CAST(rp.Description as nvarchar(256)) + '...'  as Description,
	(SELECT CONVERT(nvarchar(50), AVG(cm.Rating))
			FROM Comments cm 
			WHERE cm.RepBaseId = rp.Id 
			GROUP BY cm.RepBaseId) as Rating,
	(SELECT COUNT(cm.RepBaseId) FROM Comments cm 
			WHERE cm.RepBaseId = rp.Id 
			GROUP BY cm.RepBaseId) as RatingCount,
	(SELECT ph.ThumbnailSrc FROM PhotoToRepBase phrb 
	INNER JOIN Photos ph ON ph.Id = phrb.PhotoId
	WHERE phrb.RepBaseId = rp.Id AND ph.IsLogo = 1) as ImageSrc,
	rp.Address as Address
FROM RepBases rp 
ORDER BY rp.CreationDate DESC";
        

        public Database()
            : base(connection)
        {
            
        }

        public Database(IDbConnectionFactory factory) : base(factory)
        {

        }

        /// <summary>
        /// Получить две последние добавленные базы с рейтингом и количеством голосов
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RepBaseListItem> GetNewBases()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = String.Format(sqlGetBases, "TOP 2");
                var rp = cn.Query<RepBaseListItem>(sql);
                return rp;
            }
        }

        /// <summary>
        /// Получить список значений для словаря
        /// </summary>
        /// <param name="tableName">Название словаря</param>
        /// <param name="fKey">Внешний ключ</param>
        /// <returns></returns>
        public List<SelectListItem> GetDictionary(string tableName, int fKey = 0)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = String.Format("SELECT Id as Value, Name as Text FROM {0}", tableName);

                //Обработка конкретных словарей
                switch (tableName)
                {
                    case "Rooms": sql += " WHERE RepBaseId = " + fKey.ToString();
                        break;
                    case "Distincts": sql += " WHERE CityId = " + fKey.ToString();
                        break;
                }
                var result = cn.Query<SelectListItem>(sql).ToList();
                return result;
            }
        }

        public List<RepbaseInfo> GetAllBasesCoordinates()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"SELECT Id, Name, LEFT(Description, 256) Description, Lat, Long FROM RepBases";
                var result = cn.Query<RepbaseInfo>(sql).ToList();
                return result;
            }
        }

        /// <summary>
        /// Просто назва бази
        /// </summary>
        /// <param name="repId">Ід бази</param>
        /// <returns></returns>
        public string GetBaseName(int repId)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                var sql = string.Format("SELECT TOP 1 Name FROM RepBases WHERE Id = {0}", repId);
                return cn.Query<string>(sql).First();
            }
        }

        /// <summary>
        /// Витаскуємо всі бази
        /// </summary>
        /// <returns></returns>
        public List<RepBaseListItem> GetAllBases()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = String.Format(sqlGetBases, "");
                var rp = cn.Query<RepBaseListItem>(sql).ToList();
                return rp;
            }
        }

        public List<RepBaseListItem> GetBasesByFilter(RepBaseFilter f)
        {
            if (f == null)
                return null;

            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                var rp = cn.Query<RepBaseListItem>("spGetRepBases", new { 
                    Name = f.Name, 
                    CityId = f.City.Value, 
                    DistinctId = f.Distinct.Value,
                    Date = f.Date,
                    TimeStart = f.Time.Begin,
                    TimeEnd = f.Time.End,
                    PriceStart = f.Price.Begin,
                    PriceEnd = f.Price.End
                }, CommandType.StoredProcedure).ToList();
                return rp;
            }
        }

        //З цією функцією вийшло трошки тупо. По идее, треба вибирати координати баз разом у GetBasesByFilter... Але поки що буде так
        public List<RepbaseInfo> GetBasesCoordinatesByList(List<RepBaseListItem> RepBases)
        {
            if(RepBases == null||RepBases.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (var rb in RepBases)
                sb.Append(rb.Id + ", ");
            sb.Remove(sb.Length - 2, 1);

            string sql = string.Format("SELECT Id, Lat, Long, Name as Title, Description FROM RepBases WHERE Id IN ({0})", sb.ToString());

            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                var coords = cn.Query<RepbaseInfo>(sql).ToList();
                return coords;
            }
        }

        /// <summary>
        /// Створює в базі тестові данні повязанні одні з одним
        /// </summary>
        public void CreateDemoData()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                const string testMail = "tsdaemon@gmail.com";
                const string testPhone = "+380956956757";
                const string testAddress = "Красноткацкая, 14, кв.22";

                MD5 md5 = MD5.Create();
                Guid testPaswordhash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes("123")));

                const string testBandName = "Кровавые ошметки";
                const string testDescription = @"
В раёне Севастопольской площади.
База на втором этаже капитального гаража. Комната 30м кв.
репетиционка ещё в стадии доработки но играть уже можно)
АПАРАТ:
-Гитарная голова KRANK Rev 1+ 120w
-гитарный кабинет Ibanez 300w
-басовый комбик ashdown five fifteen 100w
-Ударная установка SONOR Smart Force Studio Set (SW) (без железа)
-Линия 400w 2 шт

на базе есть туалет, кондиционер, вентилятор, чай, кофе, пиченьки)))
цена 20грн/час
сольная или вдвоём 15грн/час
093-822-47-73 Виталий
http://vk.com/id40535556
";
                const string testPhoto = "/Images/testBase.jpg";
                const string testTphoto = "/Images/testBase.small.png";

                Random r = new Random();

                #region Other stuff
                City c1 = new City() { Name = "Киев" };
                City c2 = new City() { Name = "Кременчуг" };

                cn.Insert<City>(c1);
                cn.Insert<City>(c2);

                Distinct d1 = new Distinct() { CityId = c1.Id, Name = "Дарницкий" };
                Distinct d2 = new Distinct() { CityId = c1.Id, Name = "Соломенский" };
                Distinct d3 = new Distinct() { CityId = c2.Id, Name = "Автозаводской" };
                Distinct d4 = new Distinct() { CityId = c2.Id, Name = "Крюковский" };

                cn.Insert<Distinct>(d1);
                cn.Insert<Distinct>(d2);
                cn.Insert<Distinct>(d3);
                cn.Insert<Distinct>(d4);

                User m1 = new User() { CityId = c1.Id, Email = testMail, Name = "Вася", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Manager" };
                User m2 = new User() { CityId = c1.Id, Email = testMail, Name = "Коля", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Manager" };
                User m3 = new User() { CityId = c2.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Manager" };
                User m4 = new User() { CityId = c2.Id, Email = testMail, Name = "Слава", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Manager" };

                cn.Insert<User>(m1);
                cn.Insert<User>(m2);
                cn.Insert<User>(m3);
                cn.Insert<User>(m4);

                User mm1 = new User() { BandName = testBandName, CityId = c1.Id, Email = testMail, Name = "Вася", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Musician" };
                User mm2 = new User() { BandName = testBandName, CityId = c1.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Musician" };
                User mm3 = new User() { BandName = testBandName, CityId = c2.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Musician" };
                User mm4 = new User() { BandName = testBandName, CityId = c2.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone, Role = "Musician" };

                cn.Insert<User>(mm1);
                cn.Insert<User>(mm2);
                cn.Insert<User>(mm3);
                cn.Insert<User>(mm4);
                #endregion

                #region Bases
                //Бази
                RepBase rb1 = new RepBase()
                {
                    Address = testAddress,
                    CityId = c1.Id,
                    CreationDate = DateTime.Today,
                    Description = testDescription,
                    DistinctId = d1.Id,
                    Lat = 50 + r.NextDouble(),
                    Long = 30 + r.NextDouble(),
                    ManagerId = m1.Id,
                    Name = "Пьяный матрос"
                };
                RepBase rb4 = new RepBase()
                {
                    Address = testAddress,
                    CityId = c1.Id,
                    CreationDate = DateTime.Today,
                    Description = testDescription,
                    DistinctId = d2.Id,
                    Lat = 50 + r.NextDouble(),
                    Long = 30 + r.NextDouble(),
                    ManagerId = m2.Id,
                    Name = "Пьяный матрос"
                };
                RepBase rb2 = new RepBase()
                {
                    Address = testAddress,
                    CityId = c2.Id,
                    CreationDate = DateTime.Today,
                    Description = testDescription,
                    DistinctId = d3.Id,
                    Lat = 50 + r.NextDouble(),
                    Long = 30 + r.NextDouble(),
                    ManagerId = m3.Id,
                    Name = testAddress
                };
                RepBase rb3 = new RepBase()
                {
                    Address = "Ковальова, 47",
                    CityId = c2.Id,
                    CreationDate = DateTime.Today,
                    Description = testDescription,
                    DistinctId = d4.Id,
                    Lat = 50 + r.NextDouble(),
                    Long = 30 + r.NextDouble(),
                    ManagerId = m3.Id,
                    Name = "Трезвый матрос"
                };

                cn.Insert<RepBase>(rb1);
                cn.Insert<RepBase>(rb2);
                cn.Insert<RepBase>(rb3);
                cn.Insert<RepBase>(rb4);
                #endregion

                #region Rooms 
                Room r1 = new Room() { Description = testDescription, Name = "Комната", Price = 35, RepBaseId = rb1.Id };
                Room r2 = new Room() { Description = testDescription, Name = "Комната 2", Price = null, RepBaseId = rb1.Id };
                Room r3 = new Room() { Description = testDescription, Name = "Комната 3", Price = 20, RepBaseId = rb2.Id };
                Room r4 = new Room() { Description = testDescription, Name = "Комната 4", Price = 60, RepBaseId = rb3.Id };
                Room r5 = new Room() { Description = testDescription, Name = "Комната 5", Price = 80, RepBaseId = rb4.Id };

                cn.Insert<Room>(r1);
                cn.Insert<Room>(r2);
                cn.Insert<Room>(r3);
                cn.Insert<Room>(r4);
                cn.Insert<Room>(r5);

                Price pr1 = new Price() { EndTime = 24, StartTime = 20, RoomId = r2.Id, Sum = 45 };
                Price pr2 = new Price() { EndTime = 20, StartTime = 12, RoomId = r2.Id, Sum = 30 };
                Price pr3 = new Price() { EndTime = 12, StartTime = 0, RoomId = r2.Id, Sum = 10 };
                cn.Insert<Price>(pr1);
                cn.Insert<Price>(pr2);
                cn.Insert<Price>(pr3);
                #endregion

                #region Photo
                Photo ph1 = new Photo() { IsLogo = true, ImageSrc = testPhoto, ThumbnailSrc = testTphoto };
                Photo ph2 = new Photo() { IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto };
                Photo ph3 = new Photo() { IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto };
                Photo ph4 = new Photo() { IsLogo = true, ImageSrc = testPhoto, ThumbnailSrc = testTphoto };
                Photo ph5 = new Photo() { IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto };
                Photo ph6 = new Photo() { IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto };
                Photo ph7 = new Photo() { IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto };

                cn.Insert<Photo>(ph1);
                cn.Insert<Photo>(ph2);
                cn.Insert<Photo>(ph3);
                cn.Insert<Photo>(ph4);
                cn.Insert<Photo>(ph5);
                cn.Insert<Photo>(ph6);
                cn.Insert<Photo>(ph7);

                PhotoToRepBase phrep1 = new PhotoToRepBase() { PhotoId = ph1.Id, RepBaseId = rb1.Id };
                PhotoToRepBase phrep2 = new PhotoToRepBase() { PhotoId = ph2.Id, RepBaseId = rb1.Id };
                PhotoToRepBase phrep3 = new PhotoToRepBase() { PhotoId = ph3.Id, RepBaseId = rb2.Id };
                PhotoToRepBase phrep4 = new PhotoToRepBase() { PhotoId = ph4.Id, RepBaseId = rb2.Id };
                PhotoToRepBase phrep5 = new PhotoToRepBase() { PhotoId = ph5.Id, RepBaseId = rb3.Id };

                cn.Insert<PhotoToRepBase>(phrep1);
                cn.Insert<PhotoToRepBase>(phrep2);
                cn.Insert<PhotoToRepBase>(phrep3);
                cn.Insert<PhotoToRepBase>(phrep4);
                cn.Insert<PhotoToRepBase>(phrep5);

                PhotoToRoom phrm1 = new PhotoToRoom() { PhotoId = ph1.Id, RoomId = r1.Id };
                PhotoToRoom phrm2 = new PhotoToRoom() { PhotoId = ph2.Id, RoomId = r1.Id };
                PhotoToRoom phrm3 = new PhotoToRoom() { PhotoId = ph3.Id, RoomId = r2.Id };
                PhotoToRoom phrm4 = new PhotoToRoom() { PhotoId = ph4.Id, RoomId = r3.Id };
                PhotoToRoom phrm5 = new PhotoToRoom() { PhotoId = ph5.Id, RoomId = r4.Id };

                cn.Insert<PhotoToRoom>(phrm1);
                cn.Insert<PhotoToRoom>(phrm2);
                cn.Insert<PhotoToRoom>(phrm3);
                cn.Insert<PhotoToRoom>(phrm4);
                cn.Insert<PhotoToRoom>(phrm5);
                #endregion

                #region Comments
                Comment cm1 = new Comment() { ClientId = mm1.Id, Email = testMail, Name = "Вася", Rating = 3.4, RepBaseId = rb1.Id, Text = "121212121212" };
                Comment cm2 = new Comment() { ClientId = mm2.Id, Email = testMail, Name = "Вася", Rating = 3.0, RepBaseId = rb1.Id, Text = "121212121212" };
                Comment cm3 = new Comment() { ClientId = mm3.Id, Email = testMail, Name = "Вася", Rating = 2.4, RepBaseId = rb2.Id, Text = "121212121212" };
                Comment cm4 = new Comment() { ClientId = null, Email = testMail, Name = "Вася", Rating = 1.4, RepBaseId = rb2.Id, Text = "121212121212" };
                Comment cm5 = new Comment() { ClientId = null, Email = null, Name = "Вася", Rating = 0.4, RepBaseId = rb3.Id, Text = "121212121212" };

                cn.Insert<Comment>(cm1);
                cn.Insert<Comment>(cm2);
                cn.Insert<Comment>(cm3);
                cn.Insert<Comment>(cm4);
                cn.Insert<Comment>(cm5);
                #endregion

                #region Order
                #endregion
            }
        }

        /// <summary>
        /// Видаляє демо-данні
        /// </summary>
        public void DeleteDemoData()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"
DELETE FROM BlackLists
DELETE FROM Cities
DELETE FROM Distincts
DELETE FROM Users
DELETE FROM RepBases
DELETE FROM Rooms
DELETE FROM Comments
DELETE FROM Repetitions
DELETE FROM Photos
DELETE FROM PhotoToRepBase
DELETE FROM PhotoToRoom
DELETE FROM Prices";
                cn.Execute(sql);
            }
        }

        public User GetUser(string login)
        {
            string sql = string.Format("SELECT TOP 1 * FROM Users WHERE Users.Email = @Login OR Users.PhoneNumber = @Login");

            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                var User = cn.Query<User>(sql, new { Login = login }).FirstOrDefault();
                return User;
            }
        }

        public User CreateUser(User u)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                cn.Insert<User>(u);
                return u;
            }
        }

        public Profile GetProfile(int p)
        {
            string sql = @"SELECT 
u.Id, 
u.Name, 
u.BandName, 
u.PhoneNumber, 
u.Email, 
u.CityId as CityId, 
ISNULL((SELECT TOP 1 b.Id FROM BlackLists b WHERE b.Id = @Id), 0) as InBlackList
FROM Users u
WHERE u.Id = @Id";
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                return cn.Query<Profile>(sql, new { Id = p }).First();
            }
        }

        public void SaveUser (User u)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                cn.Update<User>(u);
            }
        }

        public List<ViewModel.Repetition> GetRepetitions(int userId)
        {
            DataTable t = new DataTable();
            List<aspdev.repaem.ViewModel.Repetition> reps = new List<ViewModel.Repetition>();
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"SELECT r.*, rb.Name as RepBase, rm.Name as Room
FROM Repetitions r
INNER JOIN RepBases rb ON rb.Id = r.RepBaseId
INNER JOIN Rooms rm ON rm.Id = r.RoomId
WHERE MusicianId = @Id";
                SqlCommand sq = new SqlCommand(sql, cn as SqlConnection);
                sq.Parameters.Add(new SqlParameter() { DbType = System.Data.DbType.Int32, ParameterName = "Id", Value = userId });
                SqlDataAdapter adapter = new SqlDataAdapter(sq);
                adapter.Fill(t);

                foreach (DataRow r in t.Rows)
                {
                    aspdev.repaem.ViewModel.Repetition rp = new ViewModel.Repetition();
                    rp.Date = ((DateTime)r["TimeStart"]).Date;
                    rp.Time.Begin = ((DateTime)r["TimeStart"]).Hour;
                    rp.Time.End = ((DateTime)r["TimeEnd"]).Hour;
                    rp.Status = (ViewModel.Status)r["Status"];//(ViewModel.Status)Enum.Parse(typeof(ViewModel.Status), r["Status"].ToString());
                    rp.Name = String.Format("База: {0}, комната: {1}", r["RepBase"], r["Room"]);
                    rp.Id = (int)r["Id"];
                    rp.Sum = (int)r["Sum"];
                    rp.Comment = r["Comment"].ToString();
                    reps.Add(rp);
                }
            }
            return reps;
        }

        public bool CheckUserPhoneExist(string Phone)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"
IF EXISTS (SELECT * FROM repaem.dbo.Users WHERE PhoneNumber = @P)
	SELECT cast(1 as bit)
ELSE
	SELECT cast(0 as bit)
";
                return cn.Query<bool>(sql, new { P = Phone }).First();
            }
        }

        public bool CheckUserEmailExist(string Email)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"
IF EXISTS (SELECT * FROM repaem.dbo.Users WHERE Email = @E)
	SELECT cast(1 as bit)
ELSE
	SELECT cast(0 as bit)
";
                return cn.Query<bool>(sql, new { E = Email }).First();
            }
        }

        public void SaveComment(Comment c)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                cn.Insert<Comment>(c);
            }
        }

        public void AddRepetition(Repetition r)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                cn.Insert<Repetition>(r);
            }
        }

        public int GetRepetitionSum(RepBaseBook rb)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                var sum = cn.Query<int>("spGetRepetitionSum", new
                {
                    RoomId = rb.Room.Value,
                    TimeStart = rb.Time.Begin,
                    TimeEnd = rb.Time.End
                }, CommandType.StoredProcedure).FirstOrDefault();
                return sum;
            }
        }

        public bool CheckRepetitionTime(RepBaseBook rb)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                return cn.Query<bool>("spCheckRepetitionTime",
                    new
                    {
                        TimeStart = rb.Time.Begin,
                        TimeEnd = rb.Time.End,
                        Date = rb.Date,
                        RoomId = rb.Room.Value
                    },
                          CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public User GetRepBaseMaster(int repBaseId)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"SELECT u.* FROM Users u 
INNER JOIN RepBases r ON u.Id = r.ManagerId 
WHERE r.Id = @repBaseId";
                return cn.Query<User>(sql, new { repBaseId = repBaseId }).FirstOrDefault();
            }
        }

        public T GetOne<T>() where T: class
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                return cn.GetList<T>().FirstOrDefault();
            }
        }

        public ViewModel.RepBase GetRepBase(int id)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"
SELECT 
    r.Id, 
    r.Name, 
    c.Name as City, 
    r.Address, 
    r.Lat, 
    r.Long,
    r.Description,
    (SELECT AVG(cm.Rating)
			FROM Comments cm 
			WHERE cm.RepBaseId = @Id 
			GROUP BY cm.RepBaseId) as Rating
FROM Repbases r 
INNER JOIN Cities c ON c.Id = r.CityId
WHERE r.Id = @Id";
                var rep = cn.Query<ViewModel.RepBase>(sql, new { Id = id }).FirstOrDefault();
                if (rep == null)
                    return null;

                sql = @"
SELECT i.Id, i.ImageSrc as Src, i.ThumbnailSrc
FROM Photos i
INNER JOIN PhotoToRepBase ph ON ph.PhotoId = i.Id AND ph.RepBaseId = @Id";
                rep.Images = cn.Query<Image>(sql, new { Id = id }).ToList();

                rep.Map = new GoogleMap();
                rep.Map.Coordinates.Add(new RepbaseInfo()
                {
                    Description = rep.Description,
                    Title = rep.Name,
                    Lat = rep.Lat,
                    Long = rep.Long
                });

                sql = @"SELECT rm.Id, rm.Name, rm.Description, rm.Price FROM Rooms rm WHERE rm.RepBaseId = @Id";
                rep.Rooms = cn.Query<RepBaseRoom>(sql, new { Id = id }).ToList();

                foreach (var room in rep.Rooms)
                {
                    sql = @"
SELECT i.Id, i.ImageSrc as Src, i.ThumbnailSrc
FROM Photos i
INNER JOIN PhotoToRoom ph ON ph.PhotoId = i.Id AND ph.RoomId = @Id";
                    room.Images = cn.Query<Image>(sql, new { Id = room.Id }).ToList();

                    sql = @"SELECT Id, StartTime, EndTime, Sum as Price FROM Prices WHERE RoomId = @Id";
                    room.Prices = cn.Query<ComplexPrice>(sql, new { Id = room.Id }).ToList();

                    room.Calendar = new Calendar();
                    room.Calendar.RoomId = room.Id;
                    DataTable t = new DataTable();
                    sql = @"SELECT r.*, u.Name as MusicianName, u.BandName
FROM Repetitions r
LEFT JOIN Users u ON u.Id = r.MusicianId
WHERE RoomId = @Id";
                    SqlCommand sq = new SqlCommand(sql, cn as SqlConnection);
                    sq.Parameters.Add(new SqlParameter() { DbType = System.Data.DbType.Int32, ParameterName = "Id", Value = room.Id });
                    SqlDataAdapter adapter = new SqlDataAdapter(sq);
                    adapter.Fill(t);

                    foreach (DataRow r in t.Rows)
                    {
                        aspdev.repaem.ViewModel.Repetition rp = new ViewModel.Repetition();
                        rp.Date = ((DateTime)r["TimeStart"]).Date;
                        rp.Time.Begin = ((DateTime)r["TimeStart"]).Hour;
                        rp.Time.End = ((DateTime)r["TimeEnd"]).Hour;
                        rp.Status = (ViewModel.Status)r["Status"];
                        rp.Name = String.Format("{0}, {1}", r["MusicianName"], r["BandName"]);
                        rp.Id = (int)r["Id"];
                        rp.Sum = (int)r["Sum"];
                        rp.Comment = r["Comment"].ToString();
                        room.Calendar.Events.Add(rp);
                    }
                }

                return rep;
            }
        }
    }

    public interface IDatabase
    {
        /// <summary>
        /// Перший ліпший запис певного типу. Для тестів
        /// </summary>
        /// <typeparam name="T">Тип запису</typeparam>
        /// <returns></returns>
        T GetOne<T>() where T : class;

        /// <summary>
        /// Дві останні нові бази
        /// </summary>
        /// <returns></returns>
        IEnumerable<RepBaseListItem> GetNewBases();

        /// <summary>
        /// Получить список значений для словаря
        /// </summary>
        /// <param name="tableName">Название словаря</param>
        /// <param name="fKey">Внешний ключ</param>
        /// <returns></returns>
        List<SelectListItem> GetDictionary(string tableName, int fKey = 0);

        /// <summary>
        /// Координати всіх репетеційних баз
        /// </summary>
        /// <returns></returns>
        List<RepbaseInfo> GetAllBasesCoordinates();

        /// <summary>
        /// Просто назва бази
        /// </summary>
        /// <param name="repId">Ід бази</param>
        /// <returns></returns>
        string GetBaseName(int repId);

        /// <summary>
        /// Витаскуємо всі бази
        /// </summary>
        /// <returns></returns>
        List<RepBaseListItem> GetAllBases();

        /// <summary>
        /// Вистаскуємо бази по фільтру
        /// </summary>
        /// <param name="f">ВьюМодел фільтра</param>
        /// <remarks>Використовується хранімка</remarks>
        /// <returns></returns>
        List<RepBaseListItem> GetBasesByFilter(RepBaseFilter f);

        //З цією функцією вийшло трошки тупо. По идее, треба вибирати координати баз разом у GetBasesByFilter... Але поки що буде так
        List<RepbaseInfo> GetBasesCoordinatesByList(List<RepBaseListItem> RepBases);

        /// <summary>
        /// Створює в базі тестові данні повязанні одні з одним
        /// </summary>
        void CreateDemoData();

        /// <summary>
        /// Видаляє демо-данні
        /// </summary>
        void DeleteDemoData();

        /// <summary>
        /// Отримуємо користувача по логіну. Логіном може бути Email або номер телефону
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        User GetUser(string login);

        User CreateUser(User u);

        /// <summary>
        /// Профіль користувача
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        Profile GetProfile(int p);

        void SaveUser(User u);

        /// <summary>
        /// Всі репетиції користувача
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<aspdev.repaem.ViewModel.Repetition> GetRepetitions(int userId);

        /// <summary>
        /// Шукаємо, чи є такий телефон в якогось юзера
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        bool CheckUserPhoneExist(string Phone);

        /// <summary>
        /// Шукаємо, чи є така пошта в якогось юзера
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        bool CheckUserEmailExist(string Email);

        void SaveComment(Comment c);

        /// <summary>
        /// Зберегти нову репетицію
        /// </summary>
        /// <param name="r"></param>
        void AddRepetition(Repetition r);

        /// <summary>
        /// Отримати вартість репетиції
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        int GetRepetitionSum(RepBaseBook rb);

        /// <summary>
        /// Перевіряємо, чи можна зарегати рєпу в це час
        /// </summary>
        /// <param name="rb"></param>
        /// <returns></returns>
        bool CheckRepetitionTime(RepBaseBook rb);

        /// <summary>
        /// Отримуємо данні про хазяїна репбази
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        User GetRepBaseMaster(int p);

        /// <summary>
        /// Инфо для страницы репбазы
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ViewModel.RepBase GetRepBase(int id);
    }

    public class CustomPluralizedMapper<T> : PluralizedAutoClassMapper<T> where T : class
    {
        public override void Table(string tableName)
        {
            if (tableName.Equals("PhotoToRepBase", StringComparison.CurrentCultureIgnoreCase))
            {
                TableName = "PhotoToRepBase";
                return;
            }

            if (tableName.Equals("PhotoToRoom", StringComparison.CurrentCultureIgnoreCase))
            {
                TableName = "PhotoToRoom";
                return;
            }

            base.Table(tableName);
        }

        //Скопировал с ClassMapper. Не придумал, как здесь можно дополнить изначальную функциональность без полного переписывания
        protected override void AutoMap(Func<Type, System.Reflection.PropertyInfo, bool> canMap)
        {
            Type type = typeof(T);
            bool hasDefinedKey = Properties.Any(p => p.KeyType != KeyType.NotAKey);
            PropertyMap keyMap = null;
            foreach (var propertyInfo in type.GetProperties())
            {
                if (Properties.Any(p => p.Name.Equals(propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                if ((canMap != null && !canMap(type, propertyInfo)))
                {
                    continue;
                }

                PropertyMap map = Map(propertyInfo);
                if (!hasDefinedKey)
                {
                    if (string.Equals(map.PropertyInfo.Name, "id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        keyMap = map;
                    }

                    //У меня все ключи - Id. Так что хватит предущего условия
                    //if (keyMap == null && map.PropertyInfo.Name.EndsWith("id", true, CultureInfo.InvariantCulture))
                    //{
                    //    keyMap = map;
                    //}
                }
            }

            if (keyMap != null)
            {
                //Все ключи - Identity
                keyMap.Key(KeyType.Identity);
            }
        }
    }
}
