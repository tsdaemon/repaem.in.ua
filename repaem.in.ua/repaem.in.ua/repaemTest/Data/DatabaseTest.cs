﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using aspdev.repaem.Models.Data;
using System.Collections.Generic;
using aspdev.repaem.ViewModel;
using Dapper.Data.SqlClient;
using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using System.Configuration;
using aspdev.repaem.Areas.Admin.ViewModel;

namespace repaemTest
{
	[TestClass]
	public class DatabaseTest
	{
		private Database db;

		[TestInitialize]
		public void Init()
		{
			db = new Database();

			DapperExtensions.DapperExtensions.DefaultMapper = typeof (CustomPluralizedMapper<>);
		}

		[TestMethod, TestCategory("Demo data")]
		public void ReCreateDemoData()
		{
			db.DeleteDemoData();
			db.CreateDemoData();
			Assert.IsTrue(true);
		}

		[TestMethod, TestCategory("Demo data")]
		public void CreateNewRepetitions()
		{
			
		}

		[TestMethod]
		public void GetNewBasesTest()
		{
			var t = new List<RepBaseListItem>(db.GetNewBases());
			Assert.AreNotEqual(t.Count, 0);
		}

		[TestMethod]
		public void GetBasesByFilterTest()
		{
			RepBaseFilter f = new RepBaseFilter()
				{
					Price = new Range() {Begin = 25, End = 75},
					Time = new TimeRange() {Begin = 2, End = 4},
					Date = new DateTime(1990, 09, 10)
				};
			var t = new List<RepBaseListItem>(db.GetBasesByFilter(f));
			Assert.AreNotEqual(t.Count, 0);
		}

		[TestMethod]
		public void GetUserByPhone()
		{
			var u = db.SearchUser("+380956956757");
			Assert.IsNotNull(u);
		}

		[TestMethod]
		public void GetUserByEmail()
		{
			var u = db.SearchUser("tsdaemon@gmail.com");
			Assert.IsNotNull(u);
		}

		[TestMethod]
		public void GetProfile()
		{
			var u = db.GetOne<User>();
			var p = db.GetProfile(u.Id);
			Assert.IsNotNull(p);
			Assert.IsTrue(p.CityId > 0);
		}

		[TestMethod]
		public void CheckEmailPhoneExist()
		{
			Assert.IsTrue(db.CheckUserEmailExist("tsdaemon@gmail.com"));
			Assert.IsTrue(db.CheckUserPhoneExist("+380956956757"));
		}

		[TestMethod]
		public void GetRepetitions()
		{
			var user = db.SearchUser("tsdaemon@gmail.com");
			var reps = db.GetRepetitions(user.Id);
			Assert.IsNotNull(reps);
		}

		[TestMethod]
		public void GetRepetitionSum()
		{
			RepBaseBook rb = new RepBaseBook()
				{
					Time = new TimeRange() {Begin = 12, End = 14},
					RoomId = db.GetOne<Room>().Id
				};

			Assert.IsNotNull(db.GetRepetitionSum(rb));
		}

		[TestMethod]
		public void CheckRepetitionTime()
		{
			RepBaseBook rb = new RepBaseBook()
				{
					Time = new TimeRange() {Begin = 12, End = 14},
					Date = DateTime.Today,
					RoomId = db.GetOne<Room>().Id
				};

			Assert.IsNotNull(db.CheckRepetitionTime(rb.Time, rb.Date, rb.RoomId));
		}

		[TestMethod]
		public void GetRepbaseMaster()
		{
			var repBase = db.GetOne<aspdev.repaem.Models.Data.RepBase>();
			Assert.IsNotNull(db.GetRepBaseMaster(repBase.Id));
		}

		[TestMethod]
		public void GetRepBase()
		{
			var repBase = db.GetOne<aspdev.repaem.Models.Data.RepBase>();
			var repInfo = db.GetRepBase(repBase.Id);
			Assert.IsNotNull(repInfo);
			Assert.IsNotNull(repInfo.Map);
		}

		[TestMethod]
		public void GetRepetitionInfo()
		{
			var rep = db.GetOne<aspdev.repaem.Models.Data.Repetition>();
			var info = db.GetRepetitionInfo(rep.Id);

			Assert.IsNotNull(info);
		}

		[TestMethod]
		public void SetRepetitionStatus()
		{
			var rep = db.GetOne<aspdev.repaem.Models.Data.Repetition>();
			Status s = (Status) rep.Status;
			db.SetRepetitionStatus(rep.Id, Status.approoved);
			rep = db.GetOne<aspdev.repaem.Models.Data.Repetition>(rep.Id);
			Assert.AreEqual(rep.Status, (int) Status.approoved);
			db.SetRepetitionStatus(rep.Id, s);
		}

		[TestMethod]
		public void GetRepBaseCommentsTest()
		{
			var rp = db.GetOne<aspdev.repaem.Models.Data.RepBase>();
			var cm = db.GetRepBaseComments(rp.Id);
			Assert.IsNotNull(cm);
		}

		[TestMethod]
		public void GetBaseCoordinatesTest()
		{
			var allCoor = db.GetAllBasesCoordinates();
			var coor = db.GetBasesCoordinatesByList(db.GetAllBases());
			Assert.IsTrue(allCoor.Count == coor.Count);
		}

		[TestMethod]
		public void CheckCanCommentTest()
		{
			var comment = db.GetOne<aspdev.repaem.Models.Data.Comment>();

			var can1 = db.CheckCanCommentRepBase(comment.RepBaseId, comment.Host);
			Assert.IsTrue(!can1);
			var can2 = db.CheckCanCommentRepBase(comment.RepBaseId, comment.UserId.Value, comment.Host);
			Assert.IsTrue(!can2);
		}

		[TestMethod]
		public void GetPhotosEdit()
		{
			var repbase = db.GetOne<aspdev.repaem.Models.Data.RepBase>();
			var room = db.GetOne<Room>();

			var ph1 = db.GetPhotos("RepBase", repbase.Id);
			var ph2 = db.GetPhotos("Room", repbase.Id);

			Assert.IsTrue(true);
		}

		[TestMethod]
		public void GetRepBaseEdit()
		{
			var repbase = db.GetOne<aspdev.repaem.Models.Data.RepBase>();

			var repBaseEdit = db.GetRepBaseEdit(repbase.Id);
		}

		[TestMethod]
		public void GetOrCreateCity()
		{
			var city = db.GetOne<City>();
			var id = db.GetOrCreateCity(city.Name);
			Assert.IsTrue(id == city.Id);

			id = db.GetOrCreateCity("Козловка");
			Assert.IsTrue(id != 0);
		}

		[TestMethod]
		public void GetRoomsByManager()
		{
			var user = db.GetManager();
			IEnumerable<RoomListItem> rooms = db.GetRoomsByManager(user.Id);

			Assert.IsNotNull(rooms);
			Assert.IsTrue((new List<RoomListItem>(rooms)).Count > 0);
		}

		[TestMethod]
		public void GetRoomEdit()
		{
			var room = db.GetOne<Room>();
			var roomEdit = db.GetRoomEdit(room.Id);

			Assert.IsNotNull(roomEdit);
			Assert.IsTrue(roomEdit.ManagerId > 0);
		}

		[TestMethod]
		public void GetRoomPrices()
		{
			var room = db.GetOne<Room>();
			var prices = db.GetRoomPrices(room.Id);
			Assert.IsNotNull(prices);
		}

		[TestMethod]
		public void SearchUserSession()
		{
			db.SearchUserInSession("", "");
		}
	}
}