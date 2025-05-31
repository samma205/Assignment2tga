using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment2tga.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Assignment2tga.Controllers
{

    public class HomeController : Controller
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog = Tourguidedatabase; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False; MultiSubnetFailover=False;";

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult IndexCustomer()
        {
            List<CustomersModel> customers = new List<CustomersModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Customer";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new CustomersModel
                        {
                            id = (int)reader["ID"],
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Phone = Convert.ToInt32(reader["Phone"]),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }

            return View(customers);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

       

            // CREATE - Display Form
            
            public ActionResult CreateCustomer()
            {
                return View();
            }

            // CREATE - Handle Form Submission
            [HttpPost]
            public ActionResult Create(CustomersModel customer)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string insertQuery = "INSERT INTO Customer (FirstName, LastName, Phone, Email) VALUES (@FirstName, @LastName, @Phone, @Email)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                            cmd.Parameters.AddWithValue("@Email", customer.Email);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    ViewBag.Message = "Customer added successfully!";
                    return RedirectToAction("ListCustomers");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View();
                }
            }

            
            

            // UPDATE - Show Form
            public ActionResult UpdateCustomer(int id)
            {
                CustomersModel customer = new CustomersModel();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Customer WHERE ID = @ID";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                customer.id = (int)reader["ID"];
                                customer.FirstName = reader["FirstName"].ToString();
                                customer.LastName = reader["LastName"].ToString();
                                customer.Phone = Convert.ToInt32(reader["Phone"]);
                                customer.Email = reader["Email"].ToString();
                            }
                        }
                    }
                }

                return View(customer); // Create UpdateCustomer.cshtml view
            }

            // UPDATE - Save Changes
            [HttpPost]
            public ActionResult UpdateCustomer(CustomersModel customer)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, Phone = @Phone, Email = @Email WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID", customer.id);
                            cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                            cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                            cmd.Parameters.AddWithValue("@Email", customer.Email);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("ListCustomers");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View(customer);
                }
            }

            // DELETE
            public ActionResult DeleteCustomer(int id)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Customer WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("ListCustomers");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                    return View("Error");
                }
            }
        }
    }

