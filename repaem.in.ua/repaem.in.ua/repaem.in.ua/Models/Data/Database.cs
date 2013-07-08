﻿using aspdev.repaem.ViewModel;
using Dapper.Data;
using DapperExtensions;
using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace aspdev.repaem.Models.Data
{
    public class Database : DbContext
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
			ph.ThumbnailSrc as ImageSrc,
            rp.Address as Address
FROM RepBases rp 
LEFT JOIN PhotoToRepBase phrb ON phrb.RepBaseId = rp.Id
LEFT JOIN Photos ph ON ph.Id = phrb.PhotoId AND ph.IsLogo = 1
ORDER BY rp.CreationDate DESC";
        

        public Database()
            : base(connection)
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
        internal List<SelectListItem> GetDictionary(string tableName, int fKey = 0)
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

        internal List<RepbaseInfo> GetAllBasesCoordinates()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = @"SELECT Id, Name, LEFT(Description, 256) Description, Lat, Long FROM RepBases";
                var result = cn.Query<RepbaseInfo>(sql).ToList();
                return result;
            }
        }

        internal void InsertComment(ViewModel.Comment cm)
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                var com = new Comment { 
                    Rating = cm.Rating, Text = cm.Text, Email = cm.Text, Name = cm.Name, RepBaseId = cm.RepBaseId, ClientId = cm.UserId
                };
                
                cn.Insert<Comment>(com);
            }
        }

        /// <summary>
        /// Просто назва бази
        /// </summary>
        /// <param name="repId">Ід бази</param>
        /// <returns></returns>
        internal string GetBaseName(int repId)
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
        internal List<RepBaseListItem> GetAllBases()
        {
            using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
            {
                string sql = String.Format(sqlGetBases, "");
                var rp = cn.Query<RepBaseListItem>(sql).ToList();
                return rp;
            }
        }

        /// <summary>
        /// Вистаскуємо бази по фільтру
        /// </summary>
        /// <param name="f">ВьюМодел фільтра</param>
        /// <remarks>Використовується хранімка</remarks>
        /// <returns></returns>
        internal List<RepBaseListItem> GetBasesByFilter(RepBaseFilter f)
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
        internal List<RepbaseInfo> GetBasesCoordinatesByList(List<RepBaseListItem> RepBases)
        {
            if(RepBases == null||RepBases.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (var rb in RepBases)
                sb.Append(rb.Id + ", ");
            sb.Remove(sb.Length - 1, 1);

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
                const string testPhone = "0956956757";
                const string testAddress = "Красноткацкая, 14, кв.22";
                Guid testPaswordhash = new Guid("b5c70c396da07c37e5980f7fe4cb5357");
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
                Distinct d3 = new Distinct() { CityId = c1.Id, Name = "Автозаводской" };
                Distinct d4 = new Distinct() { CityId = c1.Id, Name = "Крюковский" };

                cn.Insert<Distinct>(d1);
                cn.Insert<Distinct>(d2);
                cn.Insert<Distinct>(d3);
                cn.Insert<Distinct>(d4);

                Manager m1 = new Manager() { CityId = c1.Id, Email = testMail, Name = "Вася", Password = testPaswordhash, PhoneNumber = testPhone };
                Manager m2 = new Manager() { CityId = c1.Id, Email = testMail, Name = "Коля", Password = testPaswordhash, PhoneNumber = testPhone };
                Manager m3 = new Manager() { CityId = c2.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone };
                Manager m4 = new Manager() { CityId = c2.Id, Email = testMail, Name = "Слава", Password = testPaswordhash, PhoneNumber = testPhone };

                cn.Insert<Manager>(m1);
                cn.Insert<Manager>(m2);
                cn.Insert<Manager>(m3);
                cn.Insert<Manager>(m4);

                Musician mm1 = new Musician() { BandName = testBandName, CityId = c1.Id, Email = testMail, Name = "Вася", Password = testPaswordhash, PhoneNumber = testPhone };
                Musician mm2 = new Musician() { BandName = testBandName, CityId = c1.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone };
                Musician mm3 = new Musician() { BandName = testBandName, CityId = c2.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone };
                Musician mm4 = new Musician() { BandName = testBandName, CityId = c2.Id, Email = testMail, Name = "Петя", Password = testPaswordhash, PhoneNumber = testPhone };
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

                Price pr1 = new Price() { EndTime = 24, StartTime = 20, RoomId = r1.Id, Sum = 45 };
                Price pr2 = new Price() { EndTime = 20, StartTime = 12, RoomId = r1.Id, Sum = 30 };
                Price pr3 = new Price() { EndTime = 0, StartTime = 12, RoomId = r1.Id, Sum = 10 };
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
                string sql = @"DELETE FROM Cities
DELETE FROM Distincts
DELETE FROM Musicians
DELETE FROM Managers
DELETE FROM RepBases
DELETE FROM Rooms
DELETE FROM Comments
DELETE FROM Orders
DELETE FROM Photos
DELETE FROM PhotoToRepBase
DELETE FROM PhotoToRoom";
                cn.Execute(sql);
            }
        }
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
