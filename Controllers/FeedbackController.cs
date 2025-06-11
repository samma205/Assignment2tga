using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment2tga.Models;

namespace Assignment2tga.Controllers
{
    public class FeedbackController : Controller
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Tourguidedatabase;Integrated Security=True;";

        
        public ActionResult Index()
        {
            List<FeedbackModel> feedbacks = new List<FeedbackModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Feedback";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            feedbacks.Add(new FeedbackModel
                            {
                                FeedbackId = (int)reader["FeedbackId"],
                                TourID = (int)reader["TourID"],
                                Rating = (int)reader["Rating"],
                                Comment = reader["Comment"].ToString()
                            });
                        }
                    }
                }
            }

            return View(feedbacks);
        }

        // CREATE
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FeedbackModel feedback)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Feedback (TourID, Rating, Comment) VALUES (@TourID, @Rating, @Comment)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TourID", feedback.TourID);
                        cmd.Parameters.AddWithValue("@Rating", feedback.Rating);
                        cmd.Parameters.AddWithValue("@Comment", feedback.Comment);
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Success"] = "Feedback submitted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }

        }

        // EDIT
        [HttpGet]
        public ActionResult Edit(int id)
        {
            FeedbackModel feedback = new FeedbackModel();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Feedback WHERE FeedbackId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            feedback.FeedbackId = (int)reader["FeedbackId"];
                            feedback.TourID = (int)reader["TourID"];
                            feedback.Rating = (int)reader["Rating"];
                            feedback.Comment = reader["Comment"].ToString();
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }
            }

            return View(feedback);
        }

        [HttpPost]
        public ActionResult Edit(FeedbackModel feedback)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Feedback SET TourID = @TourID, Rating = @Rating, Comment = @Comment WHERE FeedbackId = @FeedbackId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TourID", feedback.TourID);
                        cmd.Parameters.AddWithValue("@Rating", feedback.Rating);
                        cmd.Parameters.AddWithValue("@Comment", feedback.Comment);
                        cmd.Parameters.AddWithValue("@FeedbackId", feedback.FeedbackId);
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Success"] = "Feedback updated!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
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
                    string query = "DELETE FROM Feedback WHERE FeedbackId = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                TempData["Success"] = "Feedback deleted.";
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
