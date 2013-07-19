using aspdev.repaem.Models.Data;
using aspdev.repaem.Services;
using aspdev.repaem.ViewModel;
using aspdev.repaem.ViewModel.PageModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace aspdev.repaem.Controllers
{
    //Контроллер для работы с пользователями
    public class AccountController : LogicControllerBase
    {
        ISession Session;
        IUserService us;
        ISmsSender sms;

        public AccountController(IRepaemLogicProvider _lg, ISession _ss, IUserService _us, ISmsSender _sm) : base(_lg) { Session = _ss; us = _us; sms = _sm; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthOrRegister()
        {
            return View(new AuthOrRegister());
        }

        public ActionResult CapchaImage()
        {
            bool noisy = true;
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session.Capcha = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((System.Drawing.Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        Rectangle rect = new Rectangle(x-r, y-r, r, r);

                        gfx.DrawEllipse(pen, rect);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }

        public ActionResult Register()
        {
            return View(new Register());
        }

        [HttpPost]
        public ActionResult Register(Register reg)
        {
            if (reg.Capcha.Value != (int)HttpContext.Session["Capcha"])
            {
                ModelState.AddModelError("Capcha", "Неправильная капча!");
            }

            if (ModelState.IsValid)
            {
                us.CreateUser(reg);
                return RedirectToAction("GetCode");
            }
            else return View(reg);
        }

        [HttpPost]
        public ActionResult Auth(Auth a)
        {
            if (ModelState.IsValid)
            {
                if(us.Login(a.Login, a.Password)) 
                {
                    if (Session.BookBaseId != null)
                        return RedirectToAction("Book", "RepBase", new { id = Session.BookBaseId });
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Login", "Неправильные данные!");
                    return View(a);
                }
            }
            else return View(a);
        }

        public ActionResult Auth()
        {
            return View(new Auth());
        }

        [Authorize]
        public ActionResult GetCode()
        {
            sms.SendSms();
            return View(new Code());
        }

        [HttpPost, Authorize]
        public ActionResult GetCode(Code c)
        {
            if (Session.Sms != c.Value)
            {
                ModelState.AddModelError("Value", "Неправильный код!");
                return View(c);
            }
            else
            {
                ViewBag.Message = "Правильно!";
                if (Session.BookBaseId.HasValue)
                    return RedirectToAction("Book", "RepBase", new { id = Session.BookBaseId });
                else
                    return RedirectToAction("Index", "Home");
            }
        }

        //Профиль музыканта
        [Authorize]
        public ActionResult Profile() 
        {
            var prof = Logic.GetProfile();
            return View(prof);
        }

        [HttpPost, Authorize]
        public ActionResult Profile(Profile prof)
        {
            if (ModelState.IsValid)
            {
                Logic.SaveProfile(prof);
            }
            return View(prof);
        }

        //Репетиции музыканта
        [Authorize]
        public ActionResult Repetitions()
        {
            var reps = Logic.GetRepetitions();
            return View(reps);
        }
    }
}
