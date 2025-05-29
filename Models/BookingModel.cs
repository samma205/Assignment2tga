using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment2tga.Models
{
	public class BookingModel
	{

       
            public int Id { get; set; }
            public int CustomerID { get; set; }
            public int TourID { get; set; }
            public DateTime BookingDate { get; set; }
        }
    }
