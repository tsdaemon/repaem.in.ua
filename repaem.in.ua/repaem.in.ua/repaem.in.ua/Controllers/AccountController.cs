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
        //
        // GET: /Account/

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
                return RedirectToAction("Index", "Home");
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

        public ActionResult Auth(Auth a)
        {
            a.Count++;
            return RedirectToAction("Index", "Home");
            
        }
    }
}
