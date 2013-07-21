using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using aspdev.repaem.Models.Data;
using System.Collections.Generic;
using aspdev.repaem.ViewModel;
using Dapper.Data.SqlClient;
using Ninject;

namespace repaemTest
{
    [TestClass]
    public class DatabaseTest
    {
        IDatabase db;

        [TestInitialize] 
        public void Init() {
            var module = new TestModule();
            var kernel = new StandardKernel(module);
            db = kernel.Get<IDatabase>();
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(CustomPluralizedMapper<>);
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
                Price = new Range() { Begin = 25, End = 75 },
                Time = new TimeRange() { Begin = 2, End = 4 },
                Date = new DateTime(1990, 09, 10)
            };
            var t = new List<RepBaseListItem>(db.GetBasesByFilter(f));
            Assert.AreNotEqual(t.Count, 0);

        }

        [TestMethod]
        public void ReCreateDemoData()
        {
            db.DeleteDemoData();
            db.CreateDemoData();
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetUserByPhone()
        {
            var u = db.GetUser("+380956956757");
            Assert.IsNotNull(u);
        }

        [TestMethod]
        public void GetUserByEmail()
        {
            var u = db.GetUser("tsdaemon@gmail.com");
            Assert.IsNotNull(u);
        }

        [TestMethod]
        public void GetProfile()
        {
            var p = db.GetProfile(8);
            Assert.IsNotNull(p);
            Assert.IsTrue(p.City.Value > 0);
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
            var user = db.GetUser("tsdaemon@gmail.com");
            var reps = db.GetRepetitions(user.Id);
            Assert.IsNotNull(reps);
        }

        [TestMethod]
        public void GetRepetitionSum()
        {
            RepBaseBook rb = new RepBaseBook() { 
                Time = new TimeRange() { Begin = 12, End = 14 }, 
                Room = new Dictionary() { Value = 18 } 
            };

            Assert.IsNotNull(db.GetRepetitionSum(rb));
        }

        [TestMethod]
        public void CheckRepetitionTime()
        {
            RepBaseBook rb = new RepBaseBook()
            {
                Time = new TimeRange() { Begin = 12, End = 14 },
                Date = DateTime.Today,
                Room = new Dictionary() { Value = 18 }
            };

            Assert.IsNotNull(db.CheckRepetitionTime(rb));
        }

        [TestMethod]
        public void GetRepbaseMaster()
        {
            var repBase = db.GetOne<aspdev.repaem.Models.Data.RepBase>();
            Assert.IsNotNull(db.GetRepBaseMaster(repBase.Id));
        }
    }
}
