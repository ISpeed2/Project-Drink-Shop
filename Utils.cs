using System;
using System.Data;
using System.Linq;
using ConsoleTables;  // Добавлено: Необходимо для работы с ConsoleTables

namespace OnlineStoreCLI
{
    public static class Utils
    {
        public static void DisplayTable(DataTable data, List<string> headers)
        {
            """Выводит данные в табличном формате."""
            var table = new ConsoleTable(headers.ToArray());

            foreach (DataRow row in data.Rows)
            {
                table.AddRow(row.ItemArray);
            }

            table.Write(ConsoleTables.Format.Alternative); // Необходимо указывать ConsoleTables.Format
        }

        public static decimal ValidateDecimalInput(string prompt)
        {
            """Запрашивает у пользователя ввод числа с плавающей точкой и проверяет его корректность."""
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

        public static int ValidateIntInput(string prompt)
        {
            """Запрашивает у пользователя ввод целого числа и проверяет его корректность."""
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