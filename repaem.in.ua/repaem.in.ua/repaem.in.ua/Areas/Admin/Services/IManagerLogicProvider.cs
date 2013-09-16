using System;
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
using System.Linq;

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

		void AddRepBase(RepBaseEdit edit);

		IEnumerable<RoomListItem> GetRooms();

		RoomEdit GetRoomEdit(int id);

		void CheckPermissions(int id, string table);

		void DeletePrice(int id);

		Price CreatePrice(int id);

		void PrepareRoomEdit(RoomEdit edit);

		void SaveRoom(RoomEdit edit);

		void AddRoom(RoomEdit edit);

		RoomEdit CreateRoomEdit(int? id);

		RepetitionIndex GetRepetitionIndex();
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

		public RepaemManagerLogicProvider(ISession ss, IEmailSender email, ILogger log, ISmsSender sms, IDatabase db,
		                                  IUserService us, IRepaemLogicProvider logic)
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
					NewRepetitions = _db.GetAllRepetitionsByManager(_us.CurrentUser.Id).FindAll((rep)=>rep.Status == Status.ordered)
				};
			return hm;
		}

		#region Photo

		public Photo SaveImage(int id, string table, string fileName, string thFileName)
		{
			Photo ph = new Photo() {ImageSrc = fileName, ThumbnailSrc = thFileName};
			_db.SavePhoto(ph, id, table);
			return ph;
		}

		public Photo DeletePhoto(int id)
		{
			//безопасность не прописана...
			var ph = _db.GetOne<Photo>(id);
			if (ph == null)
				throw new RepaemNotFoundException("Такого фото не существует!");

			_db.DeletePhoto(id);
			return ph;
		}

		#endregion

		#region RepBase

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
			_db.SaveRepBase(rb);
		}

		public void PrepareRepBaseEdit(RepBaseEdit edit)
		{
			if (edit.Id != 0)
			{

				edit.CityName = _db.GetDictionary("Cities").Find((ss) => ss.Value == edit.CityId.ToString()).Text;
				edit.Photos = _db.GetPhotos("RepBase", edit.Id);
				edit.Rooms = _db.GetRepBaseRooms(edit.Id);
			}
			if (edit.Lat != 0 && edit.Long != 0)
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
			}
			else
			{
				edit.Map = new GoogleMap() {EditMode = true};
			}
		}

		public void AddRepBase(RepBaseEdit edit)
		{
			edit.CityId = _db.GetOrCreateCity(edit.CityName);
			edit.ManagerId = _us.CurrentUser.Id;

			Models.Data.RepBase rb = new Models.Data.RepBase();
			Mapper.DynamicMap(edit, rb);
			_db.AddRepBase(rb);

			edit.Id = rb.Id;
		}

		#endregion

		#region Room

		public IEnumerable<RoomListItem> GetRooms()
		{
			return _db.GetRoomsByManager(_us.CurrentUser.Id);
		}

		public RoomEdit GetRoomEdit(int id)
		{
			RoomEdit model = _db.GetRoomEdit(id);
			if (model.ManagerId != _us.CurrentUser.Id)
				throw new RepaemAccessDeniedException();

			model.ComplexPrice = !model.Price.HasValue;

			PrepareRoomEdit(model);

			return model;
		}

		public void PrepareRoomEdit(RoomEdit model)
		{
			model.Prices = _db.GetRoomPrices(model.Id);

			model.Photos = _db.GetPhotos("Room", model.Id);

			model.RepBases = _db.GetDictionary("RepBases", _us.CurrentUser.Id);
			model.RepBases.Find((rb) => rb.Value == model.RepBaseId.ToString()).Selected = true;
		}

		public void DeletePrice(int id)
		{
			_db.DeletePrice(id);
		}

		public Price CreatePrice(int id)
		{
			var pr = new Price() {RoomId = id};

			if (id != 0)
				_db.AddPrice(pr);

			return pr;
		}

		public void SaveRoom(RoomEdit edit)
		{
			var room = _db.GetOne<Room>(edit.Id);
			Mapper.DynamicMap(edit, room);
			if (edit.ComplexPrice)
				room.Price = null;

			_db.SaveRoom(room);

			if (edit.ComplexPrice)
			{
				foreach (var price in edit.Prices)
					price.RoomId = edit.Id;
				_db.SavePrices(edit.Prices);
			}
		}

		public void AddRoom(RoomEdit edit)
		{
			Room r = new Room();
			Mapper.DynamicMap(edit, r);
			_db.AddRoom(r);
			edit.Id = r.Id;
		}

		public RoomEdit CreateRoomEdit(int? id)
		{
			RoomEdit model = new RoomEdit {RepBases = _db.GetDictionary("RepBases", _us.CurrentUser.Id)};

			if (id.HasValue)
			{
				model.RepBaseId = id.Value;
				model.RepBases.Find((rb) => rb.Value == model.RepBaseId.ToString()).Selected = true;
			}

			return model;
		}

		#endregion

		public void CheckPermissions(int id, string table)
		{
			switch (table)
			{
				case "RepBase":
					var edit = _db.GetRepBaseEdit(id);
					if (edit.ManagerId != _us.CurrentUser.Id)
						throw new RepaemAccessDeniedException();
					break;
				case "Room":
					var edit2 = _db.GetRoomEdit(id);
					if (edit2.ManagerId != _us.CurrentUser.Id)
						throw new RepaemAccessDeniedException();
					break;
				case "Price":
					var price = _db.GetOne<Price>(id);
					var room = _db.GetRoomEdit(price.RoomId);
					if (room.ManagerId != _us.CurrentUser.Id)
						throw new RepaemAccessDeniedException();
					break;
			}
		}

		public RepetitionIndex GetRepetitionIndex()
		{
			var reps = _db.GetAllRepetitionsByManager(_us.CurrentUser.Id);
			var model = new RepetitionIndex()
				{
					WaitingToApproveRepetitions = reps.FindAll((rep)=>rep.Status == Status.ordered),
					ApprovedRepetitions = reps.FindAll((rep) => rep.Status == Status.approoved && rep.Date >= DateTime.Today),
					CancelledRepetitions = reps.FindAll((rep) => rep.Status == Status.cancelled && rep.Date >= DateTime.Today),
					PastRepetitions = reps.FindAll((rep) => rep.Date < DateTime.Today).Take(10)
				};
			return model;
		}
	}
}