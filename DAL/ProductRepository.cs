using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace OnlineStoreCLI.DAL
{
    public class ProductRepository : IProductRepository // Реализует интерфейс, если он есть
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NpgsqlConnection? ConnectToDb()
        {
            """Устанавливает соединение с базой данных PostgreSQL."""
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(_connectionString);
                conn.Open();
                return conn;
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine($"Ошибка подключения к базе данных: {e.Message}");
                return null;
            }
        }

        public (DataTable?, List<string>?) ExecuteQuery(NpgsqlConnection conn, string query, NpgsqlParameter[]? parameters = null)
        {
            """Выполняет SQL-запрос и возвращает результат."""
            DataTable? dataTable = null;
            List<string>? columnNames = null;

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        dataTable = new DataTable();
                        dataTable.Load(reader);

                        // Get column names
                        columnNames = new List<string>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columnNames.Add(reader.GetName(i));
                        }
                    }
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine($"Ошибка выполнения запроса: {e.Message}");
                return (null, null);
            }

            return (dataTable, columnNames);
        }

        public List<Product> GetAll()
        {
            string query = "SELECT ProductId, Name, Price FROM Products;";
            using var conn = ConnectToDb(); // Используем using, чтобы гарантировать закрытие соединения
            if (conn == null) return new List<Product>();
            (DataTable? dataTable, List<string>? headers) = ExecuteQuery(conn, query);
            conn.Close();

            List<Product> products = new List<Product>();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    Product product = new Product
                    {
                        ProductId = Convert.ToInt32(row["ProductId"]),
                        Name = row["Name"].ToString(),
                        Price = Convert.ToDecimal(row["Price"])
                    };
                    products.Add(product);
                }
            }
            return products;
        }

        public Product? GetById(int productId)
        {
            string query = "SELECT ProductId, Name, Price FROM Products WHERE ProductId = @productId;";
            NpgsqlParameter[] parameters = { new NpgsqlParameter("@productId", productId) };
            using var conn = ConnectToDb();
            if (conn == null) return null;
            (DataTable? dataTable, List<string>? headers) = ExecuteQuery(conn, query, parameters);
            conn.Close();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Product
                {
                    ProductId = Convert.ToInt32(row["ProductId"]),
                    Name = row["Name"].ToString(),
                    Price = Convert.ToDecimal(row["Price"])
                };
            }
            return null;
        }

        public void Add(Product product)
        {
            string query = """
                INSERT INTO Products (Name, Description, Price, ImageUrl, CategoryId, BrandId)
                VALUES (@name, @description, @price, @imageurl, @categoryid, @brandid);
            """;

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@name", product.Name),
                new NpgsqlParameter("@description", product.Description),
                new NpgsqlParameter("@price", product.Price),
                new NpgsqlParameter("@imageurl", product.ImageUrl),
                new NpgsqlParameter("@categoryid", product.CategoryId),
                new NpgsqlParameter("@brandid", product.BrandId)
            };
            using var conn = ConnectToDb();
            if (conn == null) return;
            ExecuteQuery(conn, query, parameters);
            conn.Close();
        }

        public int UpdatePrice(int productId, decimal newPrice)
        {
            string query = "UPDATE Products SET Price = @newPrice WHERE ProductId = @productId;";

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@newPrice", newPrice),
                new NpgsqlParameter("@productId", productId)
            };

            using var conn = ConnectToDb();
            if (conn == null) return 0;
            (DataTable? dt, List<string>? headers) = ExecuteQuery(conn, query, parameters);
            conn.Close();
            return dt != null ? 1 : 0;
        }

        public int Delete(int productId)
        {
            string query = "DELETE FROM Products WHERE ProductId = @productId;";
            NpgsqlParameter[] parameters = { new NpgsqlParameter("@productId", productId) };

            using var conn = ConnectToDb();
            if (conn == null) return 0;
            (DataTable? dt, List<string>? headers) = ExecuteQuery(conn, query, parameters);
            conn.Close();
            return dt != null ? 1 : 0;
        }
    }
}
