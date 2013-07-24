﻿using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel.PageModel;

namespace aspdev.repaem.Models.Data
{
    public interface IRepaemLogicProvider
    {
        List<RepbaseInfo> GetAllBasesCoordinates();

        Register GetRegisterModel();

        RepBaseFilter GetFilter();

        Profile GetProfile();

        List<SelectListItem> GetDictionaryValues(string name);

        List<SelectListItem> GetDictionaryValues(string name, int fKey);

        RepBaseList GetAllRepBasesList();

        RepBaseList GetRepBasesByFilter(RepBaseFilter f);

        RepBaseFilter LoadFilterDictionaries(RepBaseFilter f);

        RepBaseBook GetRepBaseBook(int id);

        bool TryDemoData();

        HomeIndexModel GetHomeIndexModel();

        Profile GetUserProfile();

        void SaveProfile(Profile p);

        IUserService UserData { get; }

        List<aspdev.repaem.ViewModel.Repetition> GetRepetitions();

        void SaveComment(ViewModel.Comment c);

        AuthOrRegister GetAuthOrRegister();

        bool SaveBook(RepBaseBook rb);

        ViewModel.RepBase GetRepBase(int id);

        string GetRepBaseName(int id);
    }

    public class RepaemLogicProvider : IRepaemLogicProvider
    {
        IDatabase db;
        ISession ss;
        ISmsSender sms;
        IEmailSender email;

        public IUserService UserData { get; private set; }

        public RepaemLogicProvider(IDatabase _db, ISession _ss, IUserService _us, ISmsSender _sms, IEmailSender _email)
        {
            db = _db;
            ss = _ss;
            sms = _sms;
            email = _email;
            UserData = _us;
        }

        public List<SelectListItem> GetDictionaryValues(string name)
        {
            if (HttpContext.Current.Cache[name] == null)
            {
                var ls = db.GetDictionary(name);
                ls.Insert(0, new SelectListItem() { Text = "", Value = "0" });
                HttpContext.Current.Cache[name] = ls;
            }
            return HttpContext.Current.Cache[name] as List<SelectListItem>;
        }

        public List<SelectListItem> GetDictionaryValues(string name, int fKey)
        {
            //смотрим есть ли в кеше
            string n = name + fKey.ToString("D3"); 
            if (HttpContext.Current.Cache[n] == null)
            {
                var ls = db.GetDictionary(name, fKey);
                ls.Insert(0, new SelectListItem() { Text = "", Value = "0" });

                HttpContext.Current.Cache[n] = ls;
            }
            return HttpContext.Current.Cache[n] as List<SelectListItem>;
        }
    
        public Register GetRegisterModel()
        {
            Register r = new Register();
            r.City.Items = GetDictionaryValues("Cities");
            return r;
        }

        public RepBaseFilter GetFilter()
        {
            var f = new RepBaseFilter();
            f.City.Items = GetDictionaryValues("Cities");
            f.Distinct.Items.Add(new SelectListItem() { Text = "", Value = "0" });
            return f;
        }

        public Profile GetProfile()
        {
            var p = db.GetProfile(UserData.CurrentUser.Id);
            p.City.Items = GetDictionaryValues("Cities");
            return p;
        }

        public RepBaseList GetAllRepBasesList()
        {
            RepBaseList l = new RepBaseList();
            l.Filter = GetFilter();
            l.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            l.Map = new GoogleMap();
            l.Map.Coordinates = db.GetAllBasesCoordinates();
            l.RepBases = db.GetAllBases();
            return l;
        }

        public RepBaseFilter LoadFilterDictionaries(RepBaseFilter f)
        {
            if (f.City.Value != 0)
            {
                f.City.Items = GetDictionaryValues("Cities");
                if (f.Distinct.Value != 0)
                {
                    f.Distinct.Items = GetDictionaryValues("Distincts", f.City.Value);
                }
            }
            return f;
        }

        public RepBaseList GetRepBasesByFilter(RepBaseFilter f)
        {
            f = LoadFilterDictionaries(f);
            RepBaseList l = new RepBaseList();
            l.Filter = f;
            l.Filter.DisplayTpe = RepBaseFilter.DisplayType.inline;
            l.RepBases = db.GetBasesByFilter(f);
            l.Map.Coordinates = db.GetBasesCoordinatesByList(l.RepBases);

            ss.BookDate = f.Date;
            ss.BookTime = f.Time;

            return l;
        }

        public bool TryDemoData()
        {
            try
            {
                db.DeleteDemoData();
                db.CreateDemoData();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public RepBaseBook GetRepBaseBook(int id)
        {
            RepBaseBook b = new RepBaseBook();
            b.Date = ss.BookDate.HasValue ? ss.BookDate.Value : DateTime.Today;
            b.Time = (ss.BookTime ?? new TimeRange(12, 18));
            b.RepBaseName = db.GetBaseName(id);
            b.RepBaseId = id;
            b.Room.Items = GetDictionaryValues("Rooms", id);
            b.Room.Items.RemoveAt(0); //что бы пустого не было

            return b;
        }

        public List<RepbaseInfo> GetAllBasesCoordinates()
        {
            return db.GetAllBasesCoordinates();
        }

        public HomeIndexModel GetHomeIndexModel()
        {
            HomeIndexModel m = new HomeIndexModel();
            m.NewBases = db.GetNewBases().ToList();
            m.Map = new GoogleMap() { Coordinates = db.GetAllBasesCoordinates() };
            m.Filter = GetFilter();
            m.Filter.DisplayTpe = RepBaseFilter.DisplayType.square;
            return m;
        }

        public Profile GetUserProfile()
        {
            if (UserData.CurrentUser != null)
            {
                var pf = db.GetProfile(UserData.CurrentUser.Id);
                pf.City.Items = GetDictionaryValues("Cities");
                return pf;
            }
            else throw new Exception("User is null!");
        }

        public void SaveProfile(Profile p)
        {
            UserData.SaveProfile(p);
            p.City.Items = GetDictionaryValues("Cities");
        }

        public List<aspdev.repaem.ViewModel.Repetition> GetRepetitions()
        {
            var reps = db.GetRepetitions(UserData.CurrentUser.Id);
            return reps;
        }

        public void SaveComment(ViewModel.Comment c)
        {
            Comment c1 = new Comment();
            if (UserData.CurrentUser != null)
            {
                c1.ClientId = UserData.CurrentUser.Id;
            }
            c1.Email = c.Email;
            c1.Name = c.Name;
            c1.Rating = c.Rating;
            c1.RepBaseId = c.RepBaseId;
            c1.Text = c.Text;
            
            db.SaveComment(c1);
        }

        public AuthOrRegister GetAuthOrRegister()
        {
            var au = new AuthOrRegister();
            au.Register.City.Items = GetDictionaryValues("Cities");
            return au;
        }

        public bool SaveBook(RepBaseBook rb)
        {
            if (db.CheckRepetitionTime(rb))
            {
                Repetition r = new Repetition()
                {
                    Comment = rb.Comment,
                    MusicianId = UserData.CurrentUser.Id,
                    RepBaseId = rb.RepBaseId,
                    RoomId = rb.Room.Value,
                    Status = (int)ViewModel.Status.ordered,
                    TimeStart = rb.Date.AddHours(rb.Time.Begin),
                    TimeEnd = rb.Date.AddHours(rb.Time.End),
                    Sum = db.GetRepetitionSum(rb)
                };
                db.AddRepetition(r);

                rb.Room.Items = db.GetDictionary("Rooms", rb.RepBaseId);
                sms.SendRepetitionIsBooked(rb, rb.Room.Display, db.GetRepBaseMaster(rb.RepBaseId).PhoneNumber);

                return true;
            }
            else return false;
        }

        public ViewModel.RepBase GetRepBase(int id)
        {
            ViewModel.RepBase info = db.GetRepBase(id);
            return info;
        }

        public string GetRepBaseName(int id)
        {
            return db.GetBaseName(id);
        }
    }
}
