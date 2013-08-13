using Microsoft.VisualStudio.TestTools.UnitTesting;
using aspdev.repaem.Controllers;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;

namespace repaem.UnitTest
{
	[TestClass]
	public class HomeControllerTest
	{
		private HomeController _hm;

		[TestInitialize]
		public void Init()
		{
			var logic = new Mock<IRepaemLogicProvider>();
			logic.Setup(m => m.GetHomeIndexModel()).Returns(() =>
				{
					var m = new HomeIndexModel();
					return m;
				});
			logic.Setup(m => m.GetDictionaryValues("Distincts", 1)).Returns(() =>
				{
					var ls = new List<SelectListItem>();
					return ls;
				});

			_hm = new HomeController(logic.Object);
		}

		[TestMethod]
		public void IndexTest()
		{
			var result = _hm.Index();
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void GetDistinctsTest()
		{
			var result = _hm.GetDistincts(1);
			Assert.IsNotNull(result.Data);
		}
	}
}