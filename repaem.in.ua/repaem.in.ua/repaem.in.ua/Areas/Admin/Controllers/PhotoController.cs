using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.Areas.Admin.Services;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Areas.Admin.Controllers
{
	public class PhotoController : RepaemAdminControllerBase
	{
		private const int IMAGE_WIDTH = 150;
		private const string IMAGE_PATH = "~/Images/upload/";

		public PhotoController(IManagerLogicProvider logic) : base(logic)
		{
		}

		[HttpPost]
		public ActionResult Upload(int id, string table)
		{
			Logic.CheckPermissions(id, table);

			if (Request.Files.Count > 0)
			{
				foreach (var file in Request.Files.AllKeys.Select(sfile => Request.Files.Get(sfile)))
				{
					using (var img = Image.FromStream(file.InputStream))
					{
						if (img.RawFormat.Equals(ImageFormat.Png) ||
						    img.RawFormat.Equals(ImageFormat.Gif) ||
						    img.RawFormat.Equals(ImageFormat.Jpeg))
						{
							string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
							string thFileName = "th" + fileName;
							string path = Server.MapPath(IMAGE_PATH + fileName);
							string thPath = Server.MapPath(IMAGE_PATH + thFileName);
							string url = IMAGE_PATH.Remove(0, 1) + fileName;
							string thUrl = IMAGE_PATH.Remove(0, 1) + thFileName;

							img.Save(path);
							int height = Convert.ToInt32((Convert.ToDouble(img.Height)/Convert.ToDouble(img.Width))*IMAGE_WIDTH);
							using (var thumbImg = img.GetThumbnailImage(IMAGE_WIDTH, height, null, IntPtr.Zero))
							{
								thumbImg.Save(thPath);
							}

							var ph = Logic.SaveImage(id, table, url, thUrl);
							return PartialView("DisplayTemplates/Photo", ph);
						}
					}
				}
			}

			return null;
		}

		[HttpDelete]
		public bool Delete(int id)
		{
			Photo ph;

			try
			{
				ph = Logic.DeletePhoto(id);
			}
			catch (RepaemException)
			{
				return true;
			}
			try 
			{
				if (System.IO.File.Exists(Server.MapPath(ph.ImageSrc)))
					System.IO.File.Delete(Server.MapPath(ph.ImageSrc));
				if (System.IO.File.Exists(Server.MapPath(ph.ThumbnailSrc)))
					System.IO.File.Delete(Server.MapPath(ph.ThumbnailSrc));
			}
			catch (Exception)
			{
				
			}

			return true;
		}
	}
}
