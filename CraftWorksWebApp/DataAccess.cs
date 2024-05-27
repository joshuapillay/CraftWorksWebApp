using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CraftWorksWebApp
{
    public class DataAccess
    {
        private readonly string _connectionString;

        public DataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void InsertProduct(string name, decimal price, string category, bool availability, string imageUrl)
        {
            string query = "INSERT INTO Products (Name, Price, Category, Availability, ImageUrl) VALUES (@Name, @Price, @Category, @Availability, @ImageUrl)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@Availability", availability);
                    command.Parameters.AddWithValue("@ImageUrl", string.IsNullOrEmpty(imageUrl) ? DBNull.Value : (object)imageUrl);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT ProductID, Name, Price, Category, Availability, ImageUrl FROM Products";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            Name = reader["Name"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Category = reader["Category"].ToString(),
                            Availability = Convert.ToBoolean(reader["Availability"]),
                            ImageUrl = reader["ImageUrl"].ToString()
                        };
                        products.Add(product);
                    }
                    reader.Close();
                }
            }
            return products;
        }

        public void DeleteProduct(string productName)
        {
            string deleteTransactionsQuery = "DELETE FROM Transactions WHERE ProductID IN (SELECT ProductID FROM Products WHERE Name = @ProductName)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteTransactionsQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            string deleteProductQuery = "DELETE FROM Products WHERE Name = @ProductName";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteProductQuery, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void PlaceOrder(int userID, int productID)
        {
            string query = "INSERT INTO Transactions (UserID, ProductID, TransactionDate) VALUES (@UserID, @ProductID, @TransactionDate)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@TransactionDate", DateTime.Now);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Transaction> GetUserOrders(int userID)
        {
            List<Transaction> orders = new List<Transaction>();

            string query = "SELECT * FROM Transactions WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userID);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Transaction order = new Transaction
                        {
                            TransactionID = Convert.ToInt32(reader["TransactionID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            ProductID = Convert.ToInt32(reader["ProductID"]),
                            TransactionDate = Convert.ToDateTime(reader["TransactionDate"])
                        };

                        orders.Add(order);
                    }
                    reader.Close();
                }
            }
            return orders;
        }

        public void UpdateProductPrice(int productId, decimal newPrice)
        {
            string query = "UPDATE Products SET Price = @NewPrice WHERE ProductID = @ProductId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewPrice", newPrice);
                    command.Parameters.AddWithValue("@ProductId", productId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}














