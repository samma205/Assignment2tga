using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Assignment2tga.Models;
using System.Web.Mvc;

namespace Assignment2tga.Controllers
{
    
        public class BookingController : Controller
        {
            static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Tourguidedatabase;Integrated Security=True;";

            // CREATE Booking
            [HttpGet]
            public ActionResult Create()
            {
                return View();
            }

        [HttpPost]
        public ActionResult Create(BookingModel booking)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                

                    // Insert booking
                    string insertQuery = "INSERT INTO Booking (Id, CustomerID, TourID, BookingDate) VALUES (@Id, @CustomerID, @TourID, @BookingDate)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", booking.Id);
                        cmd.Parameters.AddWithValue("@CustomerID", booking.CustomerID);
                        cmd.Parameters.AddWithValue("@TourID", booking.TourID);
                        cmd.Parameters.AddWithValue("@BookingDate", booking.BookingDate);
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Success"] = "Booking confirmed!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }


        // READ - List User Bookings
        public ActionResult Index()
            {
                List<BookingModel> bookings = new List<BookingModel>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Booking";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bookings.Add(new BookingModel
                                {
                                    Id = (int)reader["Id"],
                                    CustomerID = (int)reader["CustomerID "],
                                    TourID = (int)reader["TourID"],
                                    BookingDate = (DateTime)reader["BookingDate"]
                                });
                            }
                        }
                    }
                }

                return View(bookings);
            }

            // DELETE Booking
            public ActionResult Delete(int id)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM Booking WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    ViewBag.Message = "Booking cancelled.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
        }
    }
