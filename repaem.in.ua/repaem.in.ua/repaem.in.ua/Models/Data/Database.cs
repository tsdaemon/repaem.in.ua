using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using Dapper.Data;
using DapperExtensions;
using DapperExtensions.Mapper;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Models.Data
{
	public class Database : DbContext, IDatabase
	{
		//Використовується в двух місцях, отже перенесемо сюди
		private const string sqlGetBases = @"
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

		private const string sqlGetBasesCoordinates = @"SELECT DISTINCT 
	rb.Id, 
	rb.Name as Title, 
	LEFT(Description, 256) as Description, 
	rb.Lat, 
	rb.Long, 
	ph.ThumbnailSrc as ThumbSrc
	FROM RepBases rb
LEFT JOIN PhotoToRepBase phr ON phr.RepBaseId = rb.Id
LEFT JOIN Photos ph ON ph.Id = phr.PhotoId
{0}";

		private const string connection = "localhost";

		public Database()
			: base(connection)
		{
		}

		public Database(IDbConnectionFactory factory) : base(factory)
		{
		}

		public T GetOne<T>(int? id = null) where T : class
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				if (id.HasValue)
					return cn.Get<T>(id);
				else
					return cn.GetList<T>().FirstOrDefault();
			}
		}

		/// <summary>
		///   Получить две последние добавленные базы с рейтингом и количеством голосов
		/// </summary>
		/// <returns></returns>
		public IEnumerable<RepBaseListItem> GetNewBases()
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				string sql = String.Format(sqlGetBases, "TOP 2");
				IEnumerable<RepBaseListItem> rp = cn.Query<RepBaseListItem>(sql);
				return rp;
			}
		}

		/// <summary>
		///   Получить список значений для словаря
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
					case "Rooms":
						sql += " WHERE RepBaseId = " + fKey.ToString();
						break;
					case "Distincts":
						sql += " WHERE CityId = " + fKey.ToString();
						break;
				}
				List<SelectListItem> result = cn.Query<SelectListItem>(sql).ToList();
				return result;
			}
		}

		public List<RepbaseInfo> GetAllBasesCoordinates()
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				string sql = String.Format(sqlGetBasesCoordinates, "");
				return cn.Query<RepbaseInfo>(sql).ToList();
			}
		}

		/// <summary>
		///   Просто назва бази
		/// </summary>
		/// <param name="repId">Ід бази</param>
		/// <returns></returns>
		public string GetBaseName(int repId)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				string sql = string.Format("SELECT TOP 1 Name FROM RepBases WHERE Id = {0}", repId);
				return cn.Query<string>(sql).First();
			}
		}

		/// <summary>
		///   Витаскуємо всі бази
		/// </summary>
		/// <returns></returns>
		public List<RepBaseListItem> GetAllBases()
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				string sql = String.Format(sqlGetBases, "");
				List<RepBaseListItem> rp = cn.Query<RepBaseListItem>(sql).ToList();
				return rp;
			}
		}

		public List<RepBaseListItem> GetBasesByFilter(RepBaseFilter f)
		{
			if (f == null)
				return null;

			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				List<RepBaseListItem> rp = cn.Query<RepBaseListItem>("spGetRepBases", new
					{
						f.Name,
						CityId = f.City.Value,
						DistinctId = f.Distinct.Value,
						f.Date,
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
			if (RepBases == null || RepBases.Count == 0)
				return null;

			var sb = new StringBuilder();
			foreach (RepBaseListItem rb in RepBases)
				sb.Append(rb.Id + ", ");
			sb.Remove(sb.Length - 2, 1);

			string sql = string.Format(sqlGetBasesCoordinates, " WHERE rb.Id IN (" + sb + ")");

			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				List<RepbaseInfo> coords = cn.Query<RepbaseInfo>(sql).ToList();
				return coords;
			}
		}

		/// <summary>
		///   Створює в базі тестові данні повязанні одні з одним
		/// </summary>
		public void CreateDemoData()
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string testMail = "tsdaemon@gmail.com";
				const string testPhone = "+380956956757";
				const string testAddress = "Красноткацкая, 14, кв.22";

				var md5 = MD5.Create();
				var testPaswordhash = new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes("123")));

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

				var r = new Random();

				//Database constaints:
				//Repetition.TimeEnd > Repetition.TimeStart
				//Repetition.RepBaseId = Room.RepBaseId

				#region Other stuff

				var c1 = new City {Name = "Киев"};
				var c2 = new City {Name = "Кременчуг"};

				cn.Insert(c1);
				cn.Insert(c2);

				var d1 = new Distinct {CityId = c1.Id, Name = "Дарницкий"};
				var d2 = new Distinct {CityId = c1.Id, Name = "Соломенский"};
				var d3 = new Distinct {CityId = c2.Id, Name = "Автозаводской"};
				var d4 = new Distinct {CityId = c2.Id, Name = "Крюковский"};

				cn.Insert(d1);
				cn.Insert(d2);
				cn.Insert(d3);
				cn.Insert(d4);

				var m1 = new User
					{
						CityId = c1.Id,
						Email = testMail,
						Name = "Вася",
						Password = testPaswordhash,
						PhoneNumber = testPhone,
						Role = UserRole.Musician.ToString()
					};

				cn.Insert(m1);

				var mm1 = new User
					{
						CityId = c1.Id,
						Email = "tsdaemon@yandex.ru",
						Name = "Коля",
						Password = testPaswordhash,
						PhoneNumber = testPhone,
						Role = UserRole.Manager.ToString()
					};

				cn.Insert(mm1);

				#endregion

				#region Bases

				//Бази
				var rb1 = new RepBase
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
				var rb4 = new RepBase
					{
						Address = testAddress,
						CityId = c1.Id,
						CreationDate = DateTime.Today,
						Description = testDescription,
						DistinctId = d2.Id,
						Lat = 50 + r.NextDouble(),
						Long = 30 + r.NextDouble(),
						ManagerId = m1.Id,
						Name = "Пьяный матрос"
					};
				var rb2 = new RepBase
					{
						Address = testAddress,
						CityId = c2.Id,
						CreationDate = DateTime.Today,
						Description = testDescription,
						DistinctId = d3.Id,
						Lat = 50 + r.NextDouble(),
						Long = 30 + r.NextDouble(),
						ManagerId = m1.Id,
						Name = testAddress
					};
				var rb3 = new RepBase
					{
						Address = "Ковальова, 47",
						CityId = c2.Id,
						CreationDate = DateTime.Today,
						Description = testDescription,
						DistinctId = d4.Id,
						Lat = 50 + r.NextDouble(),
						Long = 30 + r.NextDouble(),
						ManagerId = m1.Id,
						Name = "Трезвый матрос"
					};

				cn.Insert(rb1);
				cn.Insert(rb2);
				cn.Insert(rb3);
				cn.Insert(rb4);

				#endregion

				#region Rooms 

				var r1 = new Room {Description = testDescription, Name = "Комната", Price = 35, RepBaseId = rb1.Id};
				var r2 = new Room {Description = testDescription, Name = "Комната 2", Price = null, RepBaseId = rb1.Id};
				var r3 = new Room {Description = testDescription, Name = "Комната 3", Price = 20, RepBaseId = rb2.Id};
				var r4 = new Room {Description = testDescription, Name = "Комната 4", Price = 60, RepBaseId = rb3.Id};
				var r5 = new Room {Description = testDescription, Name = "Комната 5", Price = 80, RepBaseId = rb4.Id};

				cn.Insert(r1);
				cn.Insert(r2);
				cn.Insert(r3);
				cn.Insert(r4);
				cn.Insert(r5);

				var pr1 = new Price {EndTime = 24, StartTime = 20, RoomId = r2.Id, Sum = 45};
				var pr2 = new Price {EndTime = 20, StartTime = 12, RoomId = r2.Id, Sum = 30};
				var pr3 = new Price {EndTime = 12, StartTime = 0, RoomId = r2.Id, Sum = 10};
				cn.Insert(pr1);
				cn.Insert(pr2);
				cn.Insert(pr3);

				#endregion

				#region Photo

				var ph1 = new Photo {IsLogo = true, ImageSrc = testPhoto, ThumbnailSrc = testTphoto};
				var ph2 = new Photo {IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto};
				var ph3 = new Photo {IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto};
				var ph4 = new Photo {IsLogo = true, ImageSrc = testPhoto, ThumbnailSrc = testTphoto};
				var ph5 = new Photo {IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto};
				var ph6 = new Photo {IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto};
				var ph7 = new Photo {IsLogo = false, ImageSrc = testPhoto, ThumbnailSrc = testTphoto};

				cn.Insert(ph1);
				cn.Insert(ph2);
				cn.Insert(ph3);
				cn.Insert(ph4);
				cn.Insert(ph5);
				cn.Insert(ph6);
				cn.Insert(ph7);

				var phrep1 = new PhotoToRepBase {PhotoId = ph1.Id, RepBaseId = rb1.Id};
				var phrep2 = new PhotoToRepBase {PhotoId = ph2.Id, RepBaseId = rb1.Id};
				var phrep3 = new PhotoToRepBase {PhotoId = ph3.Id, RepBaseId = rb2.Id};
				var phrep4 = new PhotoToRepBase {PhotoId = ph4.Id, RepBaseId = rb2.Id};
				var phrep5 = new PhotoToRepBase {PhotoId = ph5.Id, RepBaseId = rb3.Id};

				cn.Insert(phrep1);
				cn.Insert(phrep2);
				cn.Insert(phrep3);
				cn.Insert(phrep4);
				cn.Insert(phrep5);

				var phrm1 = new PhotoToRoom {PhotoId = ph1.Id, RoomId = r1.Id};
				var phrm2 = new PhotoToRoom {PhotoId = ph2.Id, RoomId = r1.Id};
				var phrm3 = new PhotoToRoom {PhotoId = ph3.Id, RoomId = r2.Id};
				var phrm4 = new PhotoToRoom {PhotoId = ph4.Id, RoomId = r3.Id};
				var phrm5 = new PhotoToRoom {PhotoId = ph5.Id, RoomId = r4.Id};

				cn.Insert(phrm1);
				cn.Insert(phrm2);
				cn.Insert(phrm3);
				cn.Insert(phrm4);
				cn.Insert(phrm5);

				#endregion

				#region Comments

				var cm1 = new Comment
					{
						ClientId = mm1.Id,
						Email = testMail,
						Name = "Вася",
						Rating = 3.4,
						RepBaseId = rb1.Id,
						Text = "121212121212"
					};
				var cm2 = new Comment
					{
						ClientId = mm1.Id,
						Email = testMail,
						Name = "Вася",
						Rating = 3.0,
						RepBaseId = rb1.Id,
						Text = "121212121212"
					};
				var cm3 = new Comment
					{
						ClientId = mm1.Id,
						Email = testMail,
						Name = "Вася",
						Rating = 2.4,
						RepBaseId = rb2.Id,
						Text = "121212121212"
					};
				var cm4 = new Comment
					{
						ClientId = null,
						Email = testMail,
						Name = "Вася",
						Rating = 1.4,
						RepBaseId = rb2.Id,
						Text = "121212121212"
					};
				var cm5 = new Comment
					{
						ClientId = null,
						Email = null,
						Name = "Вася",
						Rating = 0.4,
						RepBaseId = rb3.Id,
						Text = "121212121212"
					};

				cn.Insert(cm1);
				cn.Insert(cm2);
				cn.Insert(cm3);
				cn.Insert(cm4);
				cn.Insert(cm5);

				#endregion

				#region Repetitions

				var rep1 = new Repetition
					{
						Comment = "23",
						MusicianId = mm1.Id,
						RepBaseId = rb1.Id,
						RoomId = r1.Id,
						Status = (int) Status.ordered,
						Sum = 90,
						TimeEnd = 4,
						TimeStart = 1,
						Date = DateTime.Today
					};
				var rep2 = new Repetition
					{
						Comment = "23",
						MusicianId = mm1.Id,
						RepBaseId = rb2.Id,
						RoomId = r4.Id,
						Status = (int) Status.ordered,
						Sum = 90,
						TimeEnd = 6,
						TimeStart = 2,
						Date = DateTime.Today
					};
				var rep3 = new Repetition
					{
						Comment = "23",
						MusicianId = mm1.Id,
						RepBaseId = rb3.Id,
						RoomId = r2.Id,
						Status = (int) Status.ordered,
						Sum = 90,
						TimeEnd = 3,
						TimeStart = 2,
						Date = DateTime.Today
					};
				var rep4 = new Repetition
					{
						Comment = "23",
						MusicianId = mm1.Id,
						RepBaseId = rb4.Id,
						RoomId = r3.Id,
						Status = (int) Status.ordered,
						Sum = 90,
						TimeEnd = 10,
						TimeStart = 8,
						Date = DateTime.Today
					};
				cn.Insert(rep1);
				cn.Insert(rep2);
				cn.Insert(rep3);
				cn.Insert(rep4);

				#endregion
			}
		}

		/// <summary>
		///   Видаляє демо-данні
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
				User User = cn.Query<User>(sql, new {Login = login}).FirstOrDefault();
				return User;
			}
		}

		public User CreateUser(User u)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Insert(u);
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
				return cn.Query<Profile>(sql, new {Id = p}).First();
			}
		}

		public void SaveUser(User u)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Update(u);
			}
		}

		public List<ViewModel.Repetition> GetRepetitions(int userId)
		{
			const string sql = @"SELECT r.*, rb.Name as RepBase, rm.Name as Room
FROM Repetitions r
INNER JOIN RepBases rb ON rb.Id = r.RepBaseId
INNER JOIN Rooms rm ON rm.Id = r.RoomId
WHERE MusicianId = @Id";
			return GetRepetitions(userId, sql);
		}

		//special method to parse sql result into the list
		public List<ViewModel.Repetition> GetRepetitions(int id, string sql)
		{
			var t = new DataTable();
			var reps = new List<ViewModel.Repetition>();
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				var sq = new SqlCommand(sql, cn as SqlConnection);
				sq.Parameters.Add(new SqlParameter {DbType = DbType.Int32, ParameterName = "Id", Value = id});
				var adapter = new SqlDataAdapter(sq);
				adapter.Fill(t);

				reps.AddRange(from DataRow r in t.Rows
				              select new ViewModel.Repetition
					              {
						              Date = ((DateTime) r["Date"]), 
													Time = {Begin = ((int) r["TimeStart"]), End = ((int) r["TimeEnd"])}, 
													Status = (Status) r["Status"], 
													Name = String.Format("База: {0}, комната: {1}", r["RepBase"], r["Room"]), 
													Id = (int) r["Id"], 
													Sum = (int) r["Sum"], 
													Comment = r["Comment"].ToString()
					              });
			}
			return reps;
		}

		public bool CheckUserPhoneExist(string phone)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				string sql = @"
IF EXISTS (SELECT * FROM Users WHERE PhoneNumber = @P)
	SELECT cast(1 as bit)
ELSE
	SELECT cast(0 as bit)
";
				return cn.Query<bool>(sql, new {P = phone}).First();
			}
		}

		public bool CheckUserEmailExist(string email)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				string sql = @"
IF EXISTS (SELECT * FROM Users WHERE Email = @E)
	SELECT cast(1 as bit)
ELSE
	SELECT cast(0 as bit)
";
				return cn.Query<bool>(sql, new {E = email}).First();
			}
		}

		public void SaveComment(Comment c)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Insert(c);
			}
		}

		public void AddRepetition(Repetition r)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Insert(r);
			}
		}

		public int GetRepetitionSum(RepBaseBook rb)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				int sum = cn.Query<int>("spGetRepetitionSum", new
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
						                      rb.Date,
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
				return cn.Query<User>(sql, new {repBaseId}).FirstOrDefault();
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
			GROUP BY cm.RepBaseId) as Rating,
		(SELECT COUNT(cm.RepBaseId) FROM Comments cm 
			WHERE cm.RepBaseId = r.Id 
			GROUP BY cm.RepBaseId) as RatingCount
FROM Repbases r 
INNER JOIN Cities c ON c.Id = r.CityId
WHERE r.Id = @Id";
				ViewModel.RepBase rep = cn.Query<ViewModel.RepBase>(sql, new {Id = id}).FirstOrDefault();
				if (rep == null)
					return null;

				sql = @"
SELECT i.Id, i.ImageSrc as Src, i.ThumbnailSrc
FROM Photos i
INNER JOIN PhotoToRepBase ph ON ph.PhotoId = i.Id AND ph.RepBaseId = @Id";
				rep.Images = cn.Query<Image>(sql, new {Id = id}).ToList();

				rep.Map = new GoogleMap();
				rep.Map.Coordinates.Add(new RepbaseInfo
					{
						Description = rep.Description,
						Title = rep.Name,
						Lat = rep.Lat,
						Long = rep.Long
					});

				sql = @"SELECT rm.Id, rm.Name, rm.Description, rm.Price FROM Rooms rm WHERE rm.RepBaseId = @Id";
				rep.Rooms = cn.Query<RepBaseRoom>(sql, new {Id = id}).ToList();

				foreach (RepBaseRoom room in rep.Rooms)
				{
					sql = @"
SELECT i.Id, i.ImageSrc as Src, i.ThumbnailSrc
FROM Photos i
INNER JOIN PhotoToRoom ph ON ph.PhotoId = i.Id AND ph.RoomId = @Id";
					room.Images = cn.Query<Image>(sql, new {room.Id}).ToList();

					sql = @"SELECT Id, StartTime, EndTime, Sum as Price FROM Prices WHERE RoomId = @Id";
					room.Prices = cn.Query<ComplexPrice>(sql, new {room.Id}).ToList();

					room.Calendar = new Calendar {RoomId = room.Id};
					var t = new DataTable();
					sql = @"SELECT r.*, u.Name as MusicianName, u.BandName
FROM Repetitions r
LEFT JOIN Users u ON u.Id = r.MusicianId
WHERE RoomId = @Id";
					var sq = new SqlCommand(sql, cn as SqlConnection);
					sq.Parameters.Add(new SqlParameter {DbType = DbType.Int32, ParameterName = "Id", Value = room.Id});
					var adapter = new SqlDataAdapter(sq);
					adapter.Fill(t);

					foreach (DataRow r in t.Rows)
					{
						var rp = new ViewModel.Repetition
							{
								Date = ((DateTime) r["Date"]),
								Time = {Begin = ((int) r["TimeStart"]), End = ((int) r["TimeEnd"])},
								Status = (Status) r["Status"],
								Name =
									(r["BandName"] is DBNull || String.IsNullOrEmpty((string) r["BandName"]))
										? (string) r["MusicianName"]
										: String.Format("{0}, {1}", r["MusicianName"], r["BandName"]),
								Id = (int) r["Id"],
								Sum = (int) r["Sum"],
								Comment = r["Comment"].ToString()
							};
						room.Calendar.Events.Add(rp);
					}
				}

				return rep;
			}
		}

		public dynamic GetRepetitionInfo(int id)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"
SELECT u.PhoneNumber, u.Email, rb.Name as RepBaseName, rm.Name as RoomName, rep.TimeStart, rep.TimeEnd
FROM Repetitions rep 
INNER JOIN Rooms rm ON rm.Id = rep.RoomId
INNER JOIN RepBases rb ON rb.Id = rm.RepBaseId
INNER JOIN Users u ON u.Id = rb.ManagerId
WHERE rep.Id = @Id";
				return cn.Query<dynamic>(sql, new {Id = id}).FirstOrDefault();
			}
		}

		public void SetRepetitionStatus(int id, Status s)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				var rep = cn.Get<Repetition>(id);
				rep.Status = (int) s;
				cn.Update(rep);
			}
		}

		public List<ViewModel.Comment> GetRepBaseComments(int id)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"SELECT Name, Email, Text, Rating, ClientId as UserId FROM Comments WHERE RepBaseId = @Id";
				return cn.Query<ViewModel.Comment>(sql, new {Id = id}).ToList();
			}
		}

		#region ManagerPart
		public List<RepbaseInfo> GetBasesCoordinatesByManager(int userId)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				var sql = String.Format(sqlGetBasesCoordinates, "WHERE rb.ManagerId = @Id");
				return cn.Query<RepbaseInfo>(sql, new {Id = userId}).ToList();
			}
		}

		public List<ViewModel.Repetition> GetNewRepetitionsByManager(int userId)
		{
			var sql = @"SELECT r.*, rb.Name as RepBase, rm.Name as Room
FROM Repetitions r
INNER JOIN RepBases rb ON rb.Id = r.RepBaseId
INNER JOIN Rooms rm ON rm.Id = r.RoomId
WHERE rb.ManagerId = @Id AND Status = " + (int)Status.ordered;
			return GetRepetitions(userId, sql);
		}

		public List<RepBaseListItem2> GetRepBasesByManager(int userId)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"
SELECT rp.Id as Id, 
	rp.Name as Name, 
	CAST(rp.Description as nvarchar(256)) + '...'  as Description,
	(SELECT AVG(cm.Rating)
			FROM Comments cm 
			WHERE cm.RepBaseId = rp.Id 
			GROUP BY cm.RepBaseId) as Rating
FROM RepBases rp 
WHERE rp.ManagerId = @Id
ORDER BY rp.CreationDate DESC";
				return cn.Query<RepBaseListItem2>(sql, new {Id = userId}).ToList();
			}
		}

		public bool CheckManagerInvoices(int userId)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"
IF EXISTS (SELECT * FROM Invoices WHERE ManagerId = @Id AND Status = @S)
  SELECT CAST(1 as bit)
ELSE 
  SELECT CAST(0 as bit)";
				return Query<bool>(sql, new {Id = userId, S = (int) InvoiceStatus.Billed}).First();
			}
		}
		#endregion
	}

	public interface IDatabase
	{
		/// <summary>
		///   Перший ліпший запис певного типу. Для тестів
		/// </summary>
		/// <typeparam name="T">Тип запису</typeparam>
		/// <returns></returns>
		T GetOne<T>(int? id = null) where T : class;

		/// <summary>
		///   Дві останні нові бази
		/// </summary>
		/// <returns></returns>
		IEnumerable<RepBaseListItem> GetNewBases();

		/// <summary>
		///   Получить список значений для словаря
		/// </summary>
		/// <param name="tableName">Название словаря</param>
		/// <param name="fKey">Внешний ключ</param>
		/// <returns></returns>
		List<SelectListItem> GetDictionary(string tableName, int fKey = 0);

		/// <summary>
		///   Координати всіх репетеційних баз
		/// </summary>
		/// <returns></returns>
		List<RepbaseInfo> GetAllBasesCoordinates();

		/// <summary>
		///   Просто назва бази
		/// </summary>
		/// <param name="repId">Ід бази</param>
		/// <returns></returns>
		string GetBaseName(int repId);

		/// <summary>
		///   Витаскуємо всі бази
		/// </summary>
		/// <returns></returns>
		List<RepBaseListItem> GetAllBases();

		/// <summary>
		///   Вистаскуємо бази по фільтру
		/// </summary>
		/// <param name="f">ВьюМодел фільтра</param>
		/// <remarks>Використовується хранімка</remarks>
		/// <returns></returns>
		List<RepBaseListItem> GetBasesByFilter(RepBaseFilter f);

		//З цією функцією вийшло трошки тупо. По идее, треба вибирати координати баз разом у GetBasesByFilter... Але поки що буде так
		List<RepbaseInfo> GetBasesCoordinatesByList(List<RepBaseListItem> RepBases);

		/// <summary>
		///   Створює в базі тестові данні повязанні одні з одним
		/// </summary>
		void CreateDemoData();

		/// <summary>
		///   Видаляє демо-данні
		/// </summary>
		void DeleteDemoData();

		/// <summary>
		///   Отримуємо користувача по логіну. Логіном може бути Email або номер телефону
		/// </summary>
		/// <param name="login"></param>
		/// <returns></returns>
		User GetUser(string login);

		User CreateUser(User u);

		/// <summary>
		///   Профіль користувача
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		Profile GetProfile(int p);

		void SaveUser(User u);

		/// <summary>
		///   Всі репетиції користувача
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		List<ViewModel.Repetition> GetRepetitions(int userId);

		/// <summary>
		///   Шукаємо, чи є такий телефон в якогось юзера
		/// </summary>
		/// <param name="Email"></param>
		/// <returns></returns>
		bool CheckUserPhoneExist(string phone);

		/// <summary>
		///   Шукаємо, чи є така пошта в якогось юзера
		/// </summary>
		/// <param name="Phone"></param>
		/// <returns></returns>
		bool CheckUserEmailExist(string email);

		void SaveComment(Comment c);

		/// <summary>
		///   Зберегти нову репетицію
		/// </summary>
		/// <param name="r"></param>
		void AddRepetition(Repetition r);

		/// <summary>
		///   Отримати вартість репетиції
		/// </summary>
		/// <param name="rb"></param>
		/// <returns></returns>
		int GetRepetitionSum(RepBaseBook rb);

		/// <summary>
		///   Перевіряємо, чи можна зарегати рєпу в це час
		/// </summary>
		/// <param name="rb"></param>
		/// <returns></returns>
		bool CheckRepetitionTime(RepBaseBook rb);

		/// <summary>
		///   Отримуємо данні про хазяїна репбази
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		User GetRepBaseMaster(int p);

		/// <summary>
		///   Инфо для страницы репбазы
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		ViewModel.RepBase GetRepBase(int id);

		/// <summary>
		///   Інформація по репетиції
		/// </summary>
		/// <param name="id"></param>
		/// <returns>
		///   Email - емейл менеджера
		///   PhoneNumber - номер менеджера
		///   RoomName
		///   RepBaseName
		///   TimeStart, DateTime
		///   TimeEnd, DateTime
		/// </returns>
		dynamic GetRepetitionInfo(int id);

		/// <summary>
		///   Встановлює статус репетиції
		/// </summary>
		/// <param name="id"></param>
		void SetRepetitionStatus(int id, Status s);

		/// <summary>
		///   Комментарі до репбази
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		List<ViewModel.Comment> GetRepBaseComments(int id);

		/// <summary>
		/// Координати реп баз менеджера
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		List<RepbaseInfo> GetBasesCoordinatesByManager(int userId);

		/// <summary>
		/// Нові репетиції на базах менеджера
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		List<ViewModel.Repetition> GetNewRepetitionsByManager(int userId);

		/// <summary>
		/// Репетиційні бази менеджера
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		List<Areas.Admin.ViewModel.RepBaseListItem2> GetRepBasesByManager(int userId);

		/// <summary>
		/// Перевіряє, чи є у користувача неоплачені рахунки
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		bool CheckManagerInvoices(int userId);
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
		protected override void AutoMap(Func<Type, PropertyInfo, bool> canMap)
		{
			Type type = typeof (T);
			bool hasDefinedKey = Properties.Any(p => p.KeyType != KeyType.NotAKey);
			PropertyMap keyMap = null;
			foreach (PropertyInfo propertyInfo in type.GetProperties())
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