using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using aspdev.repaem.Controllers;
using Ninject.MockingKernel.Moq;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;
using repaem.UnitTest.Modules;
using Moq;

namespace repaem.UnitTest
{
    [TestClass]
    public class HomeControllerTest
    {
        HomeController hm;

        [TestInitialize]
        public void Init()
        {
            var logic = new Mock<IRepaemLogicProvider>();
            logic.Setup(m => m.GetHomeIndexModel()).Returns(() => { HomeIndexModel m = new HomeIndexModel(); return m; });
            logic.Setup(m => m.GetDictionaryValues("Distincts", 1)).Returns(() => { List<SelectListItem> ls = new List<SelectListItem>(); return ls; });

            hm = new HomeController(logic.Object);
        }

        [TestMethod]
        public void IndexTest()
        {
            var result = hm.Index();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetDistinctsTest()
        {
            var result = hm.GetDistincts(1);
            Assert.IsNotNull(result.Data);
        }
    }
}
