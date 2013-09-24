using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using AutoMapper;
using aspdev.repaem.Areas.Admin.ViewModel;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.Models;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using System.Linq;

namespace aspdev.repaem.Areas.Admin.Services
{
	public class RepaemManagerLogicProvider
	{
		private Session _ss;
		private ILogger _log;
		private readonly Database _db;
		private readonly IUserService _us;
		private readonly RepaemLogicProvider _logic;
		private readonly IMessagesProvider _msg;

		public RepaemManagerLogicProvider(Session ss, IEmailSender email, ILogger log, ISmsSender sms, Database db,
		                                  IUserService us, RepaemLogicProvider logic, IMessagesProvider msg)
		{
			_ss = ss;
			_log = log;
			_db = db;
			_us = us;
			_logic = logic;
			_msg = msg;
		}

		public HomeIndex GetHomeIndex()
		{
			var hm = new HomeIndex
				{
					Comments = _db.GetCommentsByManager(_us.CurrentUser.Id),
					NewRepetitions = _db.GetAllRepetitionsByManager(_us.CurrentUser.Id).FindAll((rep)=>rep.Status == Status.ordered && rep.Date >= DateTime.Today)
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

				case "Repetition":
					var repetition = _db.GetOne<Models.Data.Repetition>(id);
					if (repetition == null) 
						throw new RepaemNotFoundException("Репетиция не найдена!");

					var room2 = _db.GetRoomEdit(repetition.RoomId);
					if(room2.ManagerId != _us.CurrentUser.Id)
						throw new RepaemAccessDeniedException();
					break;
			}
		}

		#region Repetition

		public RepetitionIndex GetRepetitionIndex()
		{
			var reps = _db.GetAllRepetitionsByManager(_us.CurrentUser.Id);
			var model = new RepetitionIndex()
				{
					WaitingToApproveRepetitions = reps.FindAll((rep) => rep.Status == Status.ordered 
						&& rep.Date >= DateTime.Today 
						&& rep.TimeStart < DateTime.Now.Hour), //новые репетиции, ожидающие подтверждения
					ApprovedRepetitions = reps.FindAll((rep) => rep.Status == Status.approoved && rep.Date >= DateTime.Today), //подтвержденные репетиции, сегодня и в будущем
					CancelledRepetitions = reps.FindAll((rep) => rep.Status == Status.cancelled && rep.Date >= DateTime.Today), //отмененные репетиции, сегодня и в будущем
					PastRepetitions = reps.FindAll((rep) => rep.Status == Status.approoved && rep.Date < DateTime.Today).Take(10), //последние прошедшие репетиции
				};
			return model;
		}

		public void ApproveRepetition(int id)
		{
			var repetition = _db.GetOne<Models.Data.Repetition>(id);
			var status = (Status) repetition.Status;
			if (status == Status.approoved)
				throw new RepaemRepetitionWrongStatusException() {Status = status};

			_db.SetRepetitionStatus(id, Status.approoved);

			var mus = _db.GetOne<User>(repetition.MusicianId);
			var room = _db.GetOne<Room>(repetition.RoomId);
			var repbase = _db.GetOne<Models.Data.RepBase>(room.RepBaseId);

			string msg = String.Format("Ваша репетиция {0} на базе {1} подтверждена!", repetition.Date, repbase.Name);
			_msg.SendMessage(msg, new[] { mus.PhoneNumber }, new[] { mus.Email } );
		}

		public void RejectRepetition(int id)
		{
			var rep = _db.GetRepetitionInfo(id);
			if(rep == null)
				throw new RepaemNotFoundException("Репетиция не найдена!");

			var status = (Status)rep.Status;

			var musician = _db.GetOne<User>(rep.MusicianId);
			var room = _db.GetOne<Room>(rep.RoomId);
			var repBase = _db.GetOne<Models.Data.RepBase>(rep.RepBaseId);

			string msg;

			switch (status)
			{
				case Status.approoved:
				case Status.ordered:
					msg = String.Format("Репетиция на базе {0}, комната {1}, время {2}.00-{3}.00 {4} отменена",
															repBase.Name, room.Name, rep.TimeStart, rep.TimeEnd, rep.Date);
					_db.SetRepetitionStatus(id, Status.cancelled);
					break;
				default:
					throw new RepaemRepetitionWrongStatusException("Невозможно отменить репетицию c неверным статусом")
					{
						Status = (Status)rep.Status
					};
			}
			//определяем кому слать оповещения

			_msg.SendMessage(msg, new[] { musician.PhoneNumber }, new[] { musician.Email });
		}

		public RepetitionEdit GetRepetitionEdit(int id)
		{
			var r = _db.GetOne<Models.Data.Repetition>(id);
			if(r == null)
				throw new RepaemNotFoundException("Репетиция не найдена!");

			var edit = new RepetitionEdit();
			Mapper.DynamicMap(r, edit);

			PrepareRepetitionEdit(edit);
			edit.Time = new TimeRange(r.TimeStart, r.TimeEnd);
			
			return edit;
		}

		internal void SaveRepetitionEdit(RepetitionEdit edit)
		{
			var r = _db.GetOne<Models.Data.Repetition>(edit.Id);
			if(r==null)
				throw new RepaemNotFoundException("Репетиция не найдена!");

			Mapper.DynamicMap(edit, r);
			r.TimeStart = edit.Time.Begin;
			r.TimeEnd = edit.Time.End;

			_db.SaveRepetition(r);
		}

		internal void PrepareRepetitionEdit(RepetitionEdit edit)
		{
			edit.RepBases = _db.GetDictionary("RepBases", _us.CurrentUser.Id);
			edit.Rooms = _db.GetDictionary("Rooms", edit.RepBaseId != 0 ? edit.RepBaseId : int.Parse(edit.RepBases[0].Value));
		}

		internal List<SelectListItem> GetRooms(int id)
		{
			return _db.GetDictionary("Rooms", id);
		}

		internal void CreateRepetition(RepetitionEdit edit)
		{
			var r = new Models.Data.Repetition();

			//резолвим MusicianId
			var user = _db.SearchUser(edit.PhoneNumber);
			var repbase = _db.GetOne<Models.Data.RepBase>(edit.RepBaseId);
			if (user == null)
			{
				user = new User()
				{
					CityId = repbase.CityId,
					Email = "----",
					Password = new Guid(),
					PhoneChecked = false,
					PhoneNumber = edit.PhoneNumber,
					Role = UserRole.Musician.ToString()
				};
				_db.CreateUser(user);
			}
			r.MusicianId = user.Id;

			Mapper.DynamicMap(edit, r);
			r.TimeEnd = edit.Time.End;
			r.TimeStart = edit.Time.Begin;

			r.Status = (int)Status.approoved;

			_db.AddRepetition(r);

			edit.Id = r.Id;
		}

		internal bool CheckRepetitionTime(RepetitionEdit edit)
		{
			return _db.CheckRepetitionTime(edit.Time, edit.Date, edit.RoomId);
		}

		internal IEnumerable<repaem.ViewModel.Repetition> GetHistory()
		{
			//выбираем все прошлые репетиции... 
			return _db.GetAllRepetitionsByManager(_us.CurrentUser.Id)
				.Where((rep) => rep.Date < DateTime.Today);
		}
		#endregion
	}
}