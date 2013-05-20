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
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthOrRegister()
        {
            return View(new AuthOrRegister());
        }

        //TODO: BY AST Когда буду делать проверку не забыть о капче

        public ActionResult CapchaImage()
        {
            bool noisy = true;
            var rand = new Random((int)DateTime.Now.Ticks);

            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["Capcha"] = a + b;

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
            string capcha;
            try {
                capcha= Session["Capcha"].ToString();
            }
            catch { capcha = ""; }

            if (ModelState.IsValid && reg.Capcha.Value == capcha)
            {
                //TODO TO KCH добавить юзера
                return RedirectToAction("GetCode");
            }
            else
            {
                if (reg.Capcha.Value != capcha)
                {
                    ModelState.AddModelError("Capcha", "Неправильная капча!");
                }

                return View(reg);
            }
        }

        [HttpPost]
        public ActionResult Auth(Auth a)
        {
            a.Count++;
            //TODO: Проверить аутентификацию, проверить есть ли проверенный номер
            //return RedirectToAction("GetCode");
            if (Session["base_id"] != null)
                return RedirectToAction("Book", "RepBase", new { id = int.Parse(Session["base_id"].ToString()) });
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult Auth()
        {
            return View(new Auth());
        }

        [Authorize]
        public ActionResult GetCode()
        {
            //TODO: SendSMS function. Write SMS code in Session, send it in SMS. Check right
            return View(new Code());
        }

        [HttpPost, Authorize]
        public ActionResult GetCode(Code c)
        {
            //TODO: Check code
            ViewBag.Message = "Правильно!";
            if (Session["base_id"] != null)
                return RedirectToAction("Book", "RepBase", new { id = int.Parse(Session["base_id"].ToString()) });
            else
                return RedirectToAction("Index", "Home");
        }

        //Профиль музыканта
        [Authorize]
        public ActionResult Profile() 
        {
            //TODO: Получить инфо о профиле пользователя
            var prof = new Profile();
            return View(prof);
        }

        [HttpPost, Authorize]
        public ActionResult Profile(Profile prof)
        {
            //TODO: Проверить и сохранить данные
            return View(prof);
        }

        //Репетиции музыканта
        //[Authorize]
        public ActionResult Repetitions()
        {
            List<Repetition> reps = new List<Repetition>();
            reps.Add(new Repetition() { Time = new TimeRange(1, 2), Date = DateTime.Today, Name = "dsfsdfsdf", Status = Status.approoved, Id = 1 });
            reps.Add(new Repetition() { Time = new TimeRange(1, 2), Date = DateTime.Today, Name = "dsfsdfsdf", Status = Status.cancelled, Id = 2 });
            reps.Add(new Repetition() { Time = new TimeRange(1, 2), Date = DateTime.Today, Name = "dsfsdfsdf", Status = Status.constant, Id = 3 });
            reps.Add(new Repetition() { Time = new TimeRange(1, 2), Date = DateTime.Today, Name = "dsfsdfsdf", Status = Status.ordered, Id = 4 });
            return View(reps);
        }
    }
}
