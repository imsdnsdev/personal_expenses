using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


namespace ExpenseTracker
{
    public class Expense
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class Income
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }

    public class DataStorage
    {
        public List<Expense> Expenses { get; set; } = new();
        public List<Income> Incomes { get; set; } = new();
    }

    class Program
    {
        static string filePath = "data.json";
        static DataStorage data = new();

        static void Main()
        {
            LoadData();

            while (true)
            {
                Console.WriteLine("\n==== Меню ====");
                Console.WriteLine("[1] Добавить трату");
                Console.WriteLine("[2] Добавить доход");
                Console.WriteLine("[3] Показать все траты и доходы");
                Console.WriteLine("[4] Показать остаток");
                Console.WriteLine("[5] Сохранить в файл");
                Console.WriteLine("[6] Загрузить из файла");
                Console.WriteLine("[0] Выход");
                Console.Write("Выберите пункт: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddExpense();
                        break;
                    case "2":
                        AddIncome();
                        break;
                    case "3":
                        ShowAll();
                        break;
                    case "4":
                        ShowBalance();
                        break;
                    case "5":
                        SaveData();
                        break;
                    case "6":
                        LoadData();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }

        static void AddExpense()
        {
            Console.Write("Категория: ");
            string category = Console.ReadLine();

            Console.Write("Сумма: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Ошибка ввода суммы.");
                return;
            }

            data.Expenses.Add(new Expense
            {
                Category = category,
                Amount = amount,
                Date = DateTime.Now
            });

            Console.WriteLine("Трата добавлена.");
        }

        static void AddIncome()
        {
            Console.Write("Сумма дохода: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("Ошибка ввода суммы.");
                return;
            }

            data.Incomes.Add(new Income
            {
                Amount = amount,
                Date = DateTime.Now
            });

            Console.WriteLine("Доход добавлен.");
        }

        static void ShowAll()
        {
            Console.WriteLine("\n📉 Траты:");
            foreach (var e in data.Expenses)
                Console.WriteLine($"{e.Date:g} - {e.Category} - {e.Amount} руб.");

            Console.WriteLine("\n📈 Доходы:");
            foreach (var i in data.Incomes)
                Console.WriteLine($"{i.Date:g} - {i.Amount} руб.");
        }

        static void ShowBalance()
        {
            decimal totalIncome = 0;
            decimal totalExpenses = 0;

            foreach (var i in data.Incomes) totalIncome += i.Amount;
            foreach (var e in data.Expenses) totalExpenses += e.Amount;

            Console.WriteLine($"\nДоходы: {totalIncome} руб.");
            Console.WriteLine($"Расходы: {totalExpenses} руб.");
            Console.WriteLine($"📊 Остаток: {totalIncome - totalExpenses} руб.");
        }

        static void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(filePath, JsonSerializer.Serialize(data, options));
            Console.WriteLine("Данные сохранены.");
        }

        static void LoadData()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    data = JsonSerializer.Deserialize<DataStorage>(json) ?? new DataStorage();
                    Console.WriteLine("Данные загружены.");
                }
                catch
                {
                    Console.WriteLine("Ошибка при загрузке данных.");
                }
            }
            else
            {
                Console.WriteLine("Файл не найден, начнём с пустой базы.");
                data = new DataStorage();
            }
        }
    }
}
