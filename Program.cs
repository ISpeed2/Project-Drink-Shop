using OnlineStoreCLI.BLL;
using OnlineStoreCLI.DAL;
using System;
using System.Collections.Generic;
using ConsoleTables;

namespace OnlineStoreCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Host=localhost;Database=your_db_name;Username=your_db_user;Password=your_db_password"; // Замените

            ProductRepository productRepository = new ProductRepository(connectionString);
            ProductService productService = new ProductService(productRepository); // Создаем экземпляр BLL

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
                        List<Product> products = productService.GetAllProducts(); // Используем BLL
                        if (products != null && products.Count > 0)
                        {
                            var table = new ConsoleTable("ProductId", "Name", "Price");
                            foreach (var product in products)
                            {
                                table.AddRow(product.ProductId, product.Name, product.Price);
                            }
                            table.Write(ConsoleTables.Format.Alternative);
                        }
                        else
                        {
                            Console.WriteLine("Нет данных о товарах.");
                        }
                        break;

                    case "2":
                        Console.Write("Введите название товара: ");
                        string? name = Console.ReadLine();

                        Console.Write("Введите описание товара: ");
                        string? description = Console.ReadLine();

                        decimal price = ValidateDecimalInput("Введите цену товара: ");

                        Console.Write("Введите URL изображения товара: ");
                        string? imageurl = Console.ReadLine();

                        int categoryid = ValidateIntInput("Введите ID категории: ");
                        int brandid = ValidateIntInput("Введите ID бренда: ");

                        Product newProduct = new Product
                        {
                            Name = name,
                            Description = description,
                            Price = price,
                            ImageUrl = imageurl,
                            CategoryId = categoryid,
                            BrandId = brandid
                        };

                        try
                        {
                            productService.AddProduct(newProduct); // Используем BLL
                            Console.WriteLine("Товар успешно добавлен!");
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Ошибка добавления товара: {e.Message}");
                        }
                        break;

                    case "3":
                        int productId = ValidateIntInput("Введите ID товара: ");
                        Product? product = productService.GetProductById(productId); // Используем BLL

                        if (product != null)
                        {
                            var table = new ConsoleTable("ProductId", "Name", "Price");
                            table.AddRow(product.ProductId, product.Name, product.Price);
                            table.Write(ConsoleTables.Format.Alternative);
                        }
                        else
                        {
                            Console.WriteLine("Товар с таким ID не найден.");
                        }
                        break;

                    case "4":
                        int productIdToUpdate = ValidateIntInput("Введите ID товара для обновления цены: ");
                        decimal newPrice = ValidateDecimalInput("Введите новую цену товара: ");
                        try
                        {
                            productService.UpdateProductPrice(productIdToUpdate, newPrice); // Используем BLL
                            Console.WriteLine("Цена товара успешно обновлена!");
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Ошибка обновления цены товара: {e.Message}");
                        }
                        break;

                    case "5":
                        int productIdToDelete = ValidateIntInput("Введите ID товара для удаления: ");
                        productService.DeleteProduct(productIdToDelete); // Используем BLL
                        Console.WriteLine("Товар успешно удален!");
                        break;

                    case "0":
                        Console.WriteLine("Выход из программы.");
                        return;

                    default:
                        Console.WriteLine("Некорректный выбор. Пожалуйста, выберите действие из меню.");
                        break;
                }
            }
        }

        // Вспомогательные методы (из Utils.cs или просто здесь)
        static decimal ValidateDecimalInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (decimal.TryParse(input, out decimal value) && value >= 0)
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите положительное число.");
                }
            }
        }

        static int ValidateIntInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= 0)
                {
                    return value;
                }
                else
                {
                    Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
                }
            }
        }
    }
}