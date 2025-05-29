using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment2tga.Models
{
	public class TourModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime TourDate { get; set; }
		public int GuideID { get; set; }
		public int price { get; set; }

	}
}