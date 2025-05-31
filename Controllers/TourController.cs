using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Assignment2tga.Models;

namespace Assignment2tga.Controllers
{
    public class TourController : Controller
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Tourguidedatabase;Integrated Security=True;";

        // READ - List all tours
        public ActionResult Index()
        {
            List<TourModel> tours = new List<TourModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tour";
                using (SqlCommand cmd = new SqlCommand(query, connection))
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
                            GuideID = (int)reader["GuideID"],
                            price = (int)reader["price"]
                        });
                    }
                }
            }

            return View(tours);
        }

        // CREATE - Show form
        public ActionResult Create()
        {
            return View();
        }

        // CREATE - Save form
        [HttpPost]
        public ActionResult Create(TourModel tour)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Tour (FirstName, LastName, TourDate, GuideID, price) VALUES (@FirstName, @LastName, @TourDate, @GuideID, @price)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
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

        // UPDATE - Show form with data
        public ActionResult Edit(int id)
        {
            TourModel tour = new TourModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tour WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
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
                            tour.GuideID = (int)reader["GuideID"];
                            tour.price = (int)reader["price"];
                        }
                    }
                }
            }

            return View(tour);
        }

        // UPDATE - Save changes
        [HttpPost]
        public ActionResult Edit(TourModel tour)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Tour SET FirstName = @FirstName, LastName = @LastName, TourDate = @TourDate, GuideID = @GuideID, price = @price WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
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

        // DELETE
        public ActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Tour WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
