using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Assignment2tga.Models;

namespace Assignment2tga.Controllers
{
    public class SpecialdealsController : Controller
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Tourguidedatabase;Integrated Security=True;";
        // GET: Specialdeals
        public ActionResult Index()
        {
            List<SpecialdealsModel> deals = new List<SpecialdealsModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT*FROM SpecialDeals";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        deals.Add(new SpecialdealsModel
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Discount = (decimal)reader["Discount"]
                        });
                    }
                }


            }
            return View(deals);
        } 
        //Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(SpecialdealsModel deal)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO SpecialDeals (Id,Title,Description,Discount) VALUES (@Id,@Title,@Description,@Discount)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", deal.Id);
                        cmd.Parameters.AddWithValue("@Title", deal.Title);
                        cmd.Parameters.AddWithValue("@Description", deal.Description);
                        cmd.Parameters.AddWithValue("@Discount", deal.Discount);
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Success"] = "Deal added!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

            //EDIT
            [HttpGet]
             public ActionResult Edit(int id)
            {
                SpecialdealsModel deals= new SpecialdealsModel();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT* FROM SpecialDeals WHERE Id =@Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                deals.Id = (int)reader["Id"];
                                deals.Title = reader["Title"].ToString();
                                deals.Description = reader["Description"].ToString();
                                deals.Discount = (decimal)reader["Discount"];
                            }
                            else
                            {
                                return HttpNotFound();

                            }
                        }
                    }
                    return View(deals);
                }
            }
        
            [HttpPost]
            public ActionResult Edit(SpecialdealsModel deal)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "UPDATE SpecialDeals SET Title = @Title,Description = @Description, Discount = @Discount Where Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Title", deal.Title);
                            cmd.Parameters.AddWithValue("@Description", deal.Description);
                            cmd.Parameters.AddWithValue("@Discount", deal.Discount);
                            cmd.Parameters.AddWithValue("@Id", deal.Id);
                            cmd.ExecuteNonQuery();


                        }
                    }
                    TempData["Success"] = "Deal updated!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View();
                } 
            }
        
                
                //DELETE
                public ActionResult Delete (int Id)
        {
            try
            {
                using (SqlConnection conn= new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM SpecialDeals WHERE Id =@Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();

                    }
                }
                TempData["Success"] = "Deal deleted.";
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
    
