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

                        // Check if user and tour exist
                        string checkQuery = "SELECT COUNT(*) FROM Users WHERE Id=@UserID; SELECT COUNT(*) FROM Tour WHERE Id=@TourID";
                        using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                        {
                            checkCmd.Parameters.AddWithValue("@UserID", booking.CustomerID);
                            checkCmd.Parameters.AddWithValue("@TourID", booking.TourID);

                            using (SqlDataReader reader = checkCmd.ExecuteReader())
                            {
                                reader.Read();
                                int userCount = reader.GetInt32(0);
                                reader.NextResult();
                                reader.Read();
                                int tourCount = reader.GetInt32(0);

                                if (userCount == 0 || tourCount == 0)
                                {
                                    ViewBag.Error = "Invalid user or tour.";
                                    return View();
                                }
                            }
                        }

                        // Insert booking
                        string insertQuery = "INSERT INTO Booking (UserID, TourID, BookingDate) VALUES (@UserID, @TourID, @BookingDate)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", booking.CustomerID);
                            cmd.Parameters.AddWithValue("@TourID", booking.TourID);
                            cmd.Parameters.AddWithValue("@BookingDate", booking.BookingDate);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    ViewBag.Message = "Booking confirmed!";
                    return RedirectToAction("Index"); // Show list
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View();
                }
            }

            // READ - List User Bookings
            public ActionResult Index(int userId)
            {
                List<BookingModel> bookings = new List<BookingModel>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Booking WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                bookings.Add(new BookingModel
                                {
                                    Id = (int)reader["Id"],
                                    CustomerID = (int)reader["UserID"],
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
