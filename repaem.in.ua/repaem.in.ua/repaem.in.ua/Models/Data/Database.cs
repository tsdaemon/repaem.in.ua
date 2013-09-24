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
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.ViewModel;
using RepBaseListItem = aspdev.repaem.ViewModel.RepBaseListItem;

namespace aspdev.repaem.Models.Data
{
	//соглашения по названиям методов:
	//Get - получить одну запись, если не найдена - эксепшен
	//Search - искать записи по признаку, может вернуть нулл
	//Save - обновить запись
	//Add - добавить новую
	//Set - изменить свойство записи
	public class Database : DbContext
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

		public Database(IDbConnectionFactory factory)
			: base(factory)
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
						sql += " WHERE RepBaseId = " + fKey;
						break;
					case "RepBases":
						sql += " WHERE ManagerId = " + fKey;
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
						CityId = f.CityId,
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
						Lat = 50 + r.NextDouble(),
						Long = 30 + r.NextDouble(),
						ManagerId = mm1.Id,
						Name = "Пьяный матрос"
					};
				var rb4 = new RepBase
					{
						Address = testAddress,
						CityId = c1.Id,
						CreationDate = DateTime.Today,
						Description = testDescription,
						Lat = 50 + r.NextDouble(),
						Long = 30 + r.NextDouble(),
						ManagerId = mm1.Id,
						Name = "Пьяный матрос"
					};
				var rb2 = new RepBase
					{
						Address = testAddress,
						CityId = c2.Id,
						CreationDate = DateTime.Today,
						Description = testDescription,
						Lat = 50 + r.NextDouble(),
						Long = 30 + r.NextDouble(),
						ManagerId = mm1.Id,
						Name = testAddress
					};
				var rb3 = new RepBase
					{
						Address = "Ковальова, 47",
						CityId = c2.Id,
						CreationDate = DateTime.Today,
						Description = testDescription,
						Lat = 50 + r.NextDouble(),
						Long = 30 + r.NextDouble(),
						ManagerId = mm1.Id,
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
						UserId = mm1.Id,
						Email = testMail,
						Name = "Вася",
						Rating = 3.4,
						RepBaseId = rb1.Id,
						Text = "121212121212",
						Date = DateTime.Now,
						Host = "127.0.0.1"
					};
				var cm2 = new Comment
					{
						UserId = mm1.Id,
						Email = testMail,
						Name = "Вася",
						Rating = 3.0,
						RepBaseId = rb1.Id,
						Text = "121212121212",
						Date = DateTime.Now,
						Host = "127.0.0.1"
					};
				var cm3 = new Comment
					{
						UserId = mm1.Id,
						Email = testMail,
						Name = "Вася",
						Rating = 2.4,
						RepBaseId = rb2.Id,
						Text = "121212121212",
						Date = DateTime.Now,
						Host = "127.0.0.1"
					};
				var cm4 = new Comment
					{
						UserId = null,
						Email = testMail,
						Name = "Вася",
						Rating = 1.4,
						RepBaseId = rb2.Id,
						Text = "121212121212",
						Date = DateTime.Now,
						Host = "127.0.0.1"
					};
				var cm5 = new Comment
					{
						UserId = null,
						Email = null,
						Name = "Вася",
						Rating = 0.4,
						RepBaseId = rb3.Id,
						Text = "121212121212",
						Date = DateTime.Now,
						Host = "127.0.0.1"
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
						MusicianId = m1.Id,
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
						MusicianId = m1.Id,
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
						MusicianId = m1.Id,
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
						MusicianId = m1.Id,
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

				#region Invoices

				var i = new Invoice()
					{
						Date = DateTime.Now,
						UserId = mm1.Id,
						Status = 0,
						Sum = 78
					};
				cn.Insert(i);

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

		public User SearchUser(string login)
		{
			string sql = string.Format("SELECT TOP 1 * FROM Users WHERE Users.Email = @Login OR Users.PhoneNumber = @Login");

			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				return cn.Query<User>(sql, new {Login = login}).FirstOrDefault();
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
						RoomId = rb.RoomId,
						TimeStart = rb.Time.Begin,
						TimeEnd = rb.Time.End
					}, CommandType.StoredProcedure).FirstOrDefault();
				return sum;
			}
		}

		/// <summary>
		/// Проверяет, есть ли свободное время
		/// </summary>
		/// <param name="range">Время</param>
		/// <param name="date">Дата</param>
		/// <param name="roomId">Комната</param>
		/// <returns>True если время свободно</returns>
		public bool CheckRepetitionTime(TimeRange range, DateTime date, int roomId)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				return cn.Query<bool>("spCheckRepetitionTime",
				                      new
					                      {
						                      TimeStart = range.Begin,
																	TimeEnd = range.End,
						                      Date = date,
						                      RoomId = roomId
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
					throw new RepaemNotFoundException("Репетиционная база не найдена!") {TableName = "RepBases", ItemId = id};

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
					room.Prices = cn.Query<Price>(sql, new {room.Id}).ToList();

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

		public Repetition GetRepetitionInfo(int id)
		{
			using (IDbConnection cn = ConnectionFactory.CreateAndOpen())
			{
				return cn.Get<Repetition>(id);
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
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"SELECT Name, Email, Text, Rating, UserId, Date FROM Comments WHERE RepBaseId = @Id";
				return cn.Query<ViewModel.Comment>(sql, new {Id = id}).ToList();
			}
		}

		public bool CheckCanCommentRepBase(int id, string p)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"
IF EXISTS (SELECT * FROM Comments WHERE Host = @Host AND RepBaseId = @Id)
  SELECT CAST(0 as bit)
ELSE 
  SELECT CAST(1 as bit)";
				return Query<bool>(sql, new {Host = p, Id = id}).First();
			}
		}

		public bool CheckCanCommentRepBase(int id, int userId, string p)
		{
			const string sql = @"
IF EXISTS (SELECT * FROM Comments WHERE (Host = @Host OR UserId = @Uid) AND RepBaseId = @Id)
  SELECT CAST(0 as bit)
ELSE 
  SELECT CAST(1 as bit)";
			return Query<bool>(sql, new {Host = p, Id = id, Uid = userId}).First();
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

		public List<ViewModel.Repetition> GetAllRepetitionsByManager(int userId)
		{
			const string sql = @"SELECT r.*, 
rb.Name as RepBaseName, 
rm.Name as RoomName, 
us.BandName, 
us.Name as UserName,
us.PhoneNumber as UserPhone,
r.Status
FROM Repetitions r
INNER JOIN RepBases rb ON rb.Id = r.RepBaseId
INNER JOIN Rooms rm ON rm.Id = r.RoomId
INNER JOIN Users us ON us.Id = r.MusicianId
WHERE rb.ManagerId = @Id
ORDER BY Date, TimeStart"; 

			var ls = Query<ViewModel.Repetition>(sql, new {Id = userId}).ToList();
			ls.ForEach((l) =>
				{
					l.Time.Begin = l.TimeStart;
					l.Time.End = l.TimeEnd;
				});
			return ls;
		}

		public List<RepBaseListItem> GetRepBaseListByManager(int userId)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"
SELECT Id, 
	Name, 
	(SELECT AVG(cm.Rating)
			FROM Comments cm 
			WHERE cm.RepBaseId = rp.Id 
			GROUP BY cm.RepBaseId) as Rating,
	rp.Address
FROM RepBases rp 
WHERE ManagerId = @Id
ORDER BY CreationDate DESC";
				return cn.Query<RepBaseListItem>(sql, new {Id = userId}).ToList();
			}
		}

		public RepBaseEdit GetRepBaseEdit(int id)
		{
			const string sql = @"SELECT TOP 1 *
FROM RepBases
WHERE Id = @Id";
			var repBase = Query<RepBaseEdit>(sql, new {Id = id}).FirstOrDefault();
			if (repBase == null)
				throw new RepaemNotFoundException("Репетиционная база не найдена!") {ItemId = id, TableName = "RepBases"};

			return repBase;
		}

		public PhotosEdit GetPhotos(string p, int id)
		{
			string sql = String.Format(@"SELECT ph.* FROM Photos ph 
INNER JOIN PhotoTo{0} pht ON pht.PhotoId = ph.Id AND pht.{0}Id = @Id", p);

			var photos = Query<Photo>(sql, new {Id = id});

			var edit = new PhotosEdit(photos) {RelationId = id, RelationTo = p};

			return edit;
		}

		public List<Room> GetRepBaseRooms(int id)
		{
			const string sql = "SELECT * FROM Rooms WHERE RepBaseId = @Id";
			return Query<Room>(sql, new {Id = id}).ToList();
		}

		public void SavePhoto(Photo ph, int id, string table)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Insert(ph);
				switch (table)
				{
					case "RepBase":
						var phr1 = new PhotoToRepBase() {PhotoId = ph.Id, RepBaseId = id};
						cn.Insert(phr1);
						break;
					case "Room":
						var phr2 = new PhotoToRoom() {PhotoId = ph.Id, RoomId = id};
						cn.Insert(phr2);
						break;
				}
			}
		}

		public bool CheckUserBills(int userId)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"
IF EXISTS (SELECT * FROM Invoices WHERE UserId = @Id AND Status = @S)
  SELECT CAST(1 as bit)
ELSE 
  SELECT CAST(0 as bit)";
				return Query<bool>(sql, new {Id = userId, S = (int) InvoiceStatus.Billed}).First();
			}
		}

		public List<ViewModel.Comment> GetCommentsByManager(int id)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"
SELECT TOP 5 cm.*, cm.Name, rb.Name as RepBaseName, rb.Id as RepBaseId
FROM Comments cm
INNER JOIN RepBases rb ON rb.Id = cm.RepBaseId AND rb.ManagerId = @Id 
ORDER BY cm.Date DESC";

				return cn.Query<ViewModel.Comment>(sql, new {Id = id}).ToList();
			}
		}

		public void DeletePhoto(int id)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = "DELETE FROM Photos WHERE Id = @Id";
				const string sql2 = "DELETE FROM PhotoToRepBase WHERE PhotoId = @Id";
				const string sql3 = "DELETE FROM PhotoToRoom WHERE PhotoId = @Id";
				cn.Execute(sql, new {Id = id});
				cn.Execute(sql, new {Id = id});
				cn.Execute(sql, new {Id = id});
			}
		}

		public int GetOrCreateCity(string cityName)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				const string sql = @"SELECT * FROM Cities WHERE Name = @Name";
				var city = Query<City>(sql, new { Name = cityName }).FirstOrDefault();
				if (city == null)
				{
					city = new City() { Name = cityName };
					cn.Insert(city);
				}
				return city.Id;
			}
		}

		public void SaveRepBase(RepBase rb)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Update(rb);
			}
		}

		public void AddRepBase(RepBase rb)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				rb.CreationDate = DateTime.Now;
				cn.Insert(rb);
			}
		}

		public IEnumerable<RoomListItem> GetRoomsByManager(int id)
		{
			const string sql = @"SELECT rm.*, rb.Name as RepBaseName 
FROM Rooms rm
INNER JOIN RepBases rb ON rb.Id = rm.RepBaseId 
WHERE rb.ManagerId = @Id";
			return Query<RoomListItem>(sql, new { Id = id });
		}

		public RoomEdit GetRoomEdit(int id)
		{
			const string sql = @"SELECT rm.*, rb.ManagerId 
FROM Rooms rm
INNER JOIN RepBases rb ON rb.Id = rm.RepBaseId
WHERE rm.Id = @Id";

			var edit = Query<RoomEdit>(sql, new { Id = id }).FirstOrDefault();
			if (edit==null)
				throw new RepaemNotFoundException("Комната не найдена!");

			return edit;
		}

		public IEnumerable<Price> GetRoomPrices(int id)
		{
			const string sql = @"SELECT * FROM Prices WHERE RoomId = @Id";
			return Query<Price>(sql, new { Id = id });
		}

		public void DeletePrice(int id)
		{
			const string sql = "DELETE FROM Prices WHERE Id = @Id";
			Execute(sql, new { Id = id });
		}

		public void AddPrice(Price price)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Insert(price);
			}
		}

		public void SaveRoom(Room room)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Update(room);
			}
		}

		public void SavePrices(IEnumerable<Price> prices)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				foreach (var price in prices)
				{
					cn.Update(price);
				}
			}
		}

		public void AddRoom(Room r)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Insert(r);
			}
		}

		#endregion

		public void CancelFixedRepOneTime(int id)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				var rep = cn.Get<Repetition>(id);
				//тепер треба отримати дату для такого ж дня на цьому тижні
				DateTime dt = GetNextWeekDay(rep.Date);

				var rep2 = new Repetition()
					{
						Comment = "Отмена постоянной репетиции",
						MusicianId = rep.MusicianId,
						RepBaseId = rep.RepBaseId,
						RoomId = rep.RoomId,
						Status = 3,
						TimeEnd = rep.TimeEnd,
						TimeStart = rep.TimeStart,
						Date = dt
					};
				cn.Insert(rep2);
			}
		}

		public User GetManager()
		{
			const string sql = @"SELECT TOP 1 * FROM Users WHERE Role = 'Manager'";
			return Query<User>(sql).FirstOrDefault();
		}

		private static DateTime GetNextWeekDay(DateTime dateTime)
		{
			int now = (int)DateTime.Now.DayOfWeek;
			int then = (int)dateTime.DayOfWeek;
			int diff = then - now;
			if (diff >= 0)
				return DateTime.Now.AddDays(diff);
			else
				return DateTime.Now.AddDays(7 + diff);
		}

		internal void SaveRepetition(Repetition r)
		{
			using (var cn = ConnectionFactory.CreateAndOpen())
			{
				cn.Update(r);
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