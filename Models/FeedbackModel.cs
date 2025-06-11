using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment2tga.Models
{
	public class FeedbackModel
	{
        public int FeedbackId { get; set; }
        public int TourID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
} 