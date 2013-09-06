using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel
{
	/// <summary>
	///   Класс словарей. Предназначен для отображения данных типа ид - текст.
	/// </summary>
	public class Dictionary
	{
		public List<SelectListItem> Items { get; set; }

		private int _val;
		public int Value {
			get { return _val; }
			set
			{
				if(Items != null & Items.Count != 0) Items.Find((i) => i.Value == value.ToString(CultureInfo.InvariantCulture)).Selected = true;
				_val = value;
			}
		}

		public string Display
		{
			get
			{
				if (Items != null)
				{
					SelectListItem item = Items.Find((s) => s.Value == Value.ToString());
					if (item != null) return item.Text;
					else return "";
				}
				else return "";
			}
		}

		public override string ToString()
		{
			return Display;
		}

		public Dictionary()
		{
			Items = new List<SelectListItem>();
		}
	}
}