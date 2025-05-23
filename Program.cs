using System;
using Npgsql;
using System.Data;
using System.Collections.Generic;

namespace OnlineStoreCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Основная функция приложения.
            NpgsqlConnection conn = DbUtils.ConnectToDb();
            if (conn == null)
            {
                return; // Exit if we can't connect to the database
            }

            while (true)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Показать все товары");
                Console.WriteLine("2. Добавить новый товар");
                Console.WriteLine("3. Показать товар по ID");
                Console.WriteLine("4. Обновить цену товара");
                Console.WriteLine("5. Удалить товар");
                Console.WriteLine("0. Выход");

                Console.Write("Выберите действие: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        (DataTable allProductsData, List<string> allProductsHeaders) = DbUtils.GetAllProducts(conn);
                        if (allProductsData != null && allProductsHeaders != null)
                        {
                            Utils.DisplayTable(allProductsData, allProductsHeaders);
                        }
                        else
                        {
                            Console.WriteLine("Нет данных о товарах.");
                        }
                        break;

                    case "2":
                        Console.Write("Введите название товара: ");
                        string name = Console.ReadLine();

                        Console.Write("Введите описание товара: ");
                        string description = Console.ReadLine();

                        decimal price = Utils.ValidateDecimalInput("Введите цену товара: ");

                        Console.Write("Введите URL изображения товара: ");
                        string imageurl = Console.ReadLine();

                        int categoryid = Utils.ValidateIntInput("Введите ID категории: ");
                        int brandid = Utils.ValidateIntInput("Введите ID бренда: ");

                        DbUtils.AddNewProduct(conn, name, description, price, imageurl, categoryid, brandid);
                        Console.WriteLine("Товар успешно добавлен!");
                        break;

                    case "3":
                        int productId = Utils.ValidateIntInput("Введите ID товара: ");
                        (DataTable productData, List<string> productHeaders) = DbUtils.GetProductById(conn, productId);
                        if (productData != null && productHeaders != null && productData.Rows.Count > 0)
                        {
                            Utils.DisplayTable(productData, productHeaders);
                        }
                        else
                        {
                            Console.WriteLine("Товар с таким ID не найден.");
                        }
                        break;

                    case "4":
                        int productIdToUpdate = Utils.ValidateIntInput("Введите ID товара для обновления цены: ");
                        decimal newPrice = Utils.ValidateDecimalInput("Введите новую цену товара: ");

                        int rowsUpdated = DbUtils.UpdateProductPrice(conn, productIdToUpdate, newPrice);
                        if (rowsUpdated > 0)
                        {
                            Console.WriteLine("Цена товара успешно обновлена!");
                        }
                        else
                        {
                            Console.WriteLine("Товар с таким ID не найден.");
                        }
                        break;

                    case "5":
                        int productIdToDelete = Utils.ValidateIntInput("Введите ID товара для удаления: ");
                        int rowsDeleted = DbUtils.DeleteProduct(conn, productIdToDelete);
                        if (rowsDeleted > 0)
                        {
                            Console.WriteLine("Товар успешно удален!");
                        }
                        else
                        {
                            Console.WriteLine("Товар с таким ID не найден.");
                        }
                        break;

                    case "0":
                        Console.WriteLine("Выход из программы.");
                        conn.Close();
                        return;

                    default:
                        Console.WriteLine("Некорректный выбор. Пожалуйста, выберите действие из меню.");
                        break;
                }
            }
        }
    }
}