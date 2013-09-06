using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class PhotoController : RepaemAdminControllerBase
	{
		private const int imageWidth = 150;
		private const string imagePath = "~/Images/upload/";

		public PhotoController(IManagerLogicProvider logic) : base(logic)
		{
		}

		[HttpPost]
		public ActionResult Upload(int id, string table)
		{
			if (Request.Files.Count > 0)
			{
				foreach (var sfile in Request.Files.AllKeys)
				{
					var file = Request.Files.Get(sfile);
					using (var img = Image.FromStream(file.InputStream))
					{
						if (img.RawFormat.Equals(ImageFormat.Png) ||
						    img.RawFormat.Equals(ImageFormat.Gif) ||
						    img.RawFormat.Equals(ImageFormat.Jpeg))
						{
							string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
							string thFileName = "th" + fileName;
							string path = Server.MapPath(imagePath + fileName);
							string thPath = Server.MapPath(imagePath + thFileName);
							string url = imagePath.Remove(0, 1) + fileName;
							string thUrl = imagePath.Remove(0, 1) + thFileName;

							img.Save(path);
							int height = Convert.ToInt32((Convert.ToDouble(img.Height)/Convert.ToDouble(img.Width))*imageWidth);
							using (var thumbImg = img.GetThumbnailImage(imageWidth, height, null, IntPtr.Zero))
							{
								thumbImg.Save(thPath);
							}

							var ph = Logic.SaveImage(id, table, url, thUrl);
							return PartialView("DisplayTemplates/Photo", ph);
						}
						else
							return null;
					}
				}
			}

			return null;
		}

		[HttpDelete]
		public bool Delete(int id)
		{
			Logic.DeletePhoto(id);
			return true;
		}
	}
}
