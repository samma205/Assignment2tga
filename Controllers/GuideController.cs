using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Assignment2tga.Models;
using System.Web.Mvc;

namespace Assignment2tga.Controllers
{
        public class GuideController : Controller
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Tourguidedatabase;Integrated Security=True;";

            // READ - List all guides
            public ActionResult Index()
            {
                List<GuideModel> guides = new List<GuideModel>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Guide";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        guides.Add(new GuideModel
                        {
                            Id = (int)reader["Id"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString()
                        });
                    }
                }

                return View(guides);
            }

            // CREATE - Display form
            public ActionResult Create()
            {
                return View();
            }

            // CREATE - Save new guide
            [HttpPost]
            public ActionResult Create(GuideModel guide)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Guide (FirstName, LastName) VALUES (@FirstName, @LastName)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", guide.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guide.LastName);
                    cmd.ExecuteNonQuery();
                }

                TempData["Message"] = "Guide added.";
                return RedirectToAction("Index");
            }

            // EDIT - Show form
            public ActionResult Edit(int id)
            {
                GuideModel guide = new GuideModel();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Guide WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        guide.Id = (int)reader["Id"];
                        guide.FirstName = reader["FirstName"].ToString();
                        guide.LastName = reader["LastName"].ToString();
                    }
                }

                return View(guide);
            }

            // EDIT - Update guide
            [HttpPost]
            public ActionResult Edit(GuideModel guide)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Guide SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", guide.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", guide.LastName);
                    cmd.Parameters.AddWithValue("@Id", guide.Id);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }

            // DELETE
            public ActionResult Delete(int id)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Guide WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }


    }
        }