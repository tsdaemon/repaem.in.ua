using aspdev.repaem.Infrastructure.Logging;
using aspdev.repaem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace aspdev.repaem.Models.Data
{
    public interface IUserService
    {
        bool ChangePassword(string login, string oldPassw, string newPassw);
        bool ValidateUser(string login, string passw);
        bool UserIsInRole(string login, string role);
        bool Login(string login, string passw);
        void Logout();
        User CreateUser(Register r);
        void SaveProfile(Profile p);

        User CurrentUser { get; }
    }

    public class RepaemUserService : IUserService
    {
        ILogger lg;
        IDatabase db;
        User u;

        public RepaemUserService(IDatabase _db, ILogger _lg) 
        {
            lg = _lg;
            db = _db;
        }

        public bool ChangePassword(string login, string oldPassw, string newPassw)
        {
            var user = db.GetUser(login);
            if (GenerateMD5(oldPassw) == user.Password)
            {
                user.Password = GenerateMD5(newPassw);
                return true;
            }
            return false;
        }

        public bool ValidateUser(string login, string passw)
        {
            var user = db.GetUser(login);
            return GenerateMD5(passw) == user.Password;
        }

        public bool UserIsInRole(string login, string role)
        {
            var user = db.GetUser(login);
            return user.Role.IndexOf(role) > -1;
        }

        Guid GenerateMD5(string pass)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(pass);
            byte[] hash = md5.ComputeHash(inputBytes);

            Guid g = new Guid(hash);
            return g;
        }

        public bool Login(string login, string passw)
        {
            var user = db.GetUser(login);
            if (GenerateMD5(passw) == user.Password)
            {
                CurrentUser = user;
                return true;
            }
            else return false;
        }

        public void Logout()
        {
            HttpCookie c = HttpContext.Current.Request.Cookies.Get("user_session");
            if(c != null) 
            {
                HttpContext.Current.Cache.Remove(c.Value);
                HttpContext.Current.Response.Cookies.Remove("user_session");
            }
        }

        public User CurrentUser
        {
            get
            {
                if (u == null)
                {
                    HttpCookie c = HttpContext.Current.Request.Cookies["user_session"];
                    if (c != null)
                    {
                        u = HttpContext.Current.Cache[c.Value] as User;
                        return u;
                    }
                    else return null;
                }
                else return u;
            }

            private set
            {
                Guid gg = Guid.NewGuid();
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("user_session", gg.ToString()));
                HttpContext.Current.Cache.Insert(gg.ToString(), value, null, DateTime.MaxValue, new TimeSpan(1,0,0,0));
            }
        }

        public User CreateUser(Register r)
        {
            //TODO: Вопрос - музыкант или менеджер?
            var user = new User() { CityId = r.City.Value, Email = r.Email, Name = r.Name, Password = GenerateMD5(r.Password), PhoneChecked = false, PhoneNumber = r.Phone, Role = "Musician" };
            db.CreateUser(user);
            CurrentUser = user;
            return user;
        }

        public void SaveProfile(Profile p)
        {
            User u = CurrentUser;
            if(u==null)
                throw new Exception("User is null!");

            u.BandName = p.BandName;
            u.Password = GenerateMD5(p.Password);
            u.PhoneChecked = u.PhoneNumber == p.PhoneNumber;
            u.PhoneNumber = p.PhoneNumber;
            u.Name = p.Name;
            u.CityId = p.City.Value;
            u.Email = p.Email;

            db.SaveUser(u);
        }
    }
}
