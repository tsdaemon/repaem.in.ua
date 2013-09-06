using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using aspdev.repaem.Models.Data;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
	public class PhotosEdit : List<Photo>
	{
		public string RelationTo { get; set; }

		public int RelationId { get; set; }

		public PhotosEdit(IEnumerable<Photo> ph) : base(ph)
		{
		}
	}
}