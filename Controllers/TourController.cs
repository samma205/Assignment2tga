using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Assignment2tga.Models;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Assignment2tga.Controllers
{
	public class TourController : Controller
	{
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Tourguidedatabase; Integrated Security = True; Connect Timeout = 30; Encrypt=False;Trust Server Certificate=False;Application Intent = ReadWrite; Multi Subnet Failover=False;";


        // READ - List all Tours
        public ActionResult Index()
        {
            List<TourModel> tours = new List<TourModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Tour";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tours.Add(new TourModel
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            TourDate = Convert.ToDateTime(reader["TourDate"]),
                            GuideID = Convert.ToInt32(reader["GuideID"]),
                            price = Convert.ToInt32(reader["price"])
                        });
                    }
                }
            }

            return View(tours);
        }

        // CREATE - Display form
        public ActionResult Create()
        {
            return View();
        }

        // CREATE - Handle form submit
        [HttpPost]
        public ActionResult Create(TourModel tour)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Tour (FirstName, LastName, TourDate, GuideID, Price) VALUES (@FirstName, @LastName, @TourDate, @GuideID, @Price)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", tour.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", tour.LastName);
                        cmd.Parameters.AddWithValue("@TourDate", tour.TourDate);
                        cmd.Parameters.AddWithValue("@GuideID", tour.GuideID);
                        cmd.Parameters.AddWithValue("@Price", tour.price);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // UPDATE - Show form
        public ActionResult Edit(int id)
        {
            TourModel tour = new TourModel();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Tour WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            tour.Id = (int)reader["Id"];
                            tour.FirstName = reader["FirstName"].ToString();
                            tour.LastName = reader["LastName"].ToString();
                            tour.TourDate = Convert.ToDateTime(reader["TourDate"]);
                            tour.GuideID = Convert.ToInt32(reader["GuideID"]);
                            tour.price = Convert.ToInt32(reader["price"]);
                        }
                    }
                }
            }

            return View(tour);
        }

        // UPDATE - Handle submit
        [HttpPost]
        public ActionResult Edit(TourModel tour)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Tour SET FirstName = @FirstName, LastName = @LastName, TourDate = @TourDate, GuideID = @GuideID, Price = @Price WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", tour.Id);
                        cmd.Parameters.AddWithValue("@FirstName", tour.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", tour.LastName);
                        cmd.Parameters.AddWithValue("@TourDate", tour.TourDate);
                        cmd.Parameters.AddWithValue("@GuideID", tour.GuideID);
                        cmd.Parameters.AddWithValue("@price", tour.price);
                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(tour);
            }
        }

        // DELETE
        public ActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Tour WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

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