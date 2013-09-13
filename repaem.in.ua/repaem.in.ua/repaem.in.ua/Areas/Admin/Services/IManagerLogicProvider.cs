using System.Collections.Generic;
using System.IO;
using AutoMapper;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;

namespace aspdev.repaem.Areas.Admin.Services
{
	public interface IManagerLogicProvider
	{
		HomeIndex GetHomeIndex();

		Comments GetRepBaseComments(int id);

		List<RepBaseListItem> GetRepBasesList();

		RepBaseEdit GetRepBaseEditModel(int id);

		Photo SaveImage(int id, string table, string fileName, string thFileName);

		Photo DeletePhoto(int id);

		void SaveRepBase(RepBaseEdit edit);

		void PrepareRepBaseEdit(RepBaseEdit edit);
	}

	public class RepaemManagerLogicProvider : IManagerLogicProvider
	{
		private ISession _ss;
		private IEmailSender _email;
		private ILogger _log;
		private ISmsSender _sms;
		private readonly IDatabase _db;
		private readonly IUserService _us;
		private readonly IRepaemLogicProvider _logic;

		public RepaemManagerLogicProvider(ISession ss, IEmailSender email, ILogger log, ISmsSender sms, IDatabase db, IUserService us, IRepaemLogicProvider logic) 
		{
			_ss = ss;
			_email = email;
			_log = log;
			_sms = sms;
			_db = db;
			_us = us;
			_logic = logic;
		}

		public HomeIndex GetHomeIndex()
		{
			var hm = new HomeIndex
				{
					Comments = _db.GetCommentsByManager(_us.CurrentUser.Id),
					NewRepetitions = _db.GetNewRepetitionsByManager(_us.CurrentUser.Id)
				};
			return hm;
		}

		public Comments GetRepBaseComments(int id)
		{
			return _logic.GetRepBaseComments(id);
		}

		public List<RepBaseListItem> GetRepBasesList()
		{
			return _db.GetRepBaseListByManager(_us.CurrentUser.Id);
		}

		public RepBaseEdit GetRepBaseEditModel(int id)
		{
			var edit = _db.GetRepBaseEdit(id);
			if (edit.ManagerId != _us.CurrentUser.Id)
				throw new RepaemAccessDeniedException("Вы не можете редактировать эту базу!");

			PrepareRepBaseEdit(edit);
			return edit;
		}

		public Photo SaveImage(int id, string table, string fileName, string thFileName)
		{
			Photo ph = new Photo() {ImageSrc = fileName, ThumbnailSrc = thFileName};
			_db.SavePhoto(ph, id, table);
			return ph;
		}

		public Photo DeletePhoto(int id)
		{
			var ph = _db.GetOne<Photo>(id);
			if (ph == null)
				throw new RepaemNotFoundException("Такого фото не существует!");

			_db.DeletePhoto(id);
			return ph;
		}

		public void SaveRepBase(RepBaseEdit edit)
		{
			var rb = _db.GetOne<Models.Data.RepBase>(edit.Id);

			//Проверка безопасности
			if (rb.ManagerId != _us.CurrentUser.Id)
				throw new RepaemAccessDeniedException("Вы не можете редактировать эту базу!");

			//так как это не передается пльзователю, то нужно его задать заново
			edit.ManagerId = rb.ManagerId;
			Mapper.DynamicMap(edit, rb);

			//Резолвим город
			int cityId = _db.GetOrCreateCity(edit.CityName);
			rb.CityId = cityId;
			edit.CityId = cityId;

			//Сохраняем репбазу
			_db.SaveDatabase(rb);
		}

		public void PrepareRepBaseEdit(RepBaseEdit edit)
		{
			edit.Map = new GoogleMap
			{
				CenterLat = edit.Lat,
				CenterLon = edit.Long,
				Coordinates = new List<RepbaseInfo>()
						{
							new RepbaseInfo()
								{
									Description = edit.Description,
									Id = edit.Id,
									Lat = edit.Lat,
									Long = edit.Long,
									Title = edit.Name
								}
						},
				EditMode = true
			};
			edit.CityName = _db.GetDictionary("Cities").Find((ss) => ss.Value == edit.CityId.ToString()).Text;
			edit.Photos = _db.GetPhotos("RepBase", edit.Id);
			edit.Rooms = _db.GetRepBaseRooms(edit.Id);
		}
	}
}