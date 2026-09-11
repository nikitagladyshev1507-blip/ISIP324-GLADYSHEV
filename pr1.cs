using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        List<(string Name, int Amount)> expenses = new();

        Console.Write("Введите количество операций (от 2 до 40): ");
        int count = int.Parse(Console.ReadLine());

        for (int i = 1; i <= count; i++)
        {
            Console.Write($"Трата {i}: Название: ");
            string name = Console.ReadLine();

            Console.Write("Сумма в рублях: ");
            int amount = int.Parse(Console.ReadLine());

            expenses.Add((name, amount));
        }

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика");
            Console.WriteLine("3. Сортировка по цене");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    foreach (var e in expenses)
                        Console.WriteLine($"{e.Name} - {e.Amount}");
                    break;

                case 2:
                    var sum = expenses.Sum(x => x.Amount);
                    var avg = expenses.Average(x => x.Amount);
                    var max = expenses.Max(x => x.Amount);
                    var min = expenses.Min(x => x.Amount);

                    Console.WriteLine($"Среднее: {avg:F2}, Максимум: {max}, Минимум: {min}, Сумма: {sum}");
                    break;

                case 3:
                    var sortedExpenses = expenses.OrderBy(e => e.Amount).ToList();
                    foreach (var e in sortedExpenses)
                        Console.WriteLine($"{e.Name} - {e.Amount}");
                    break;

                case 4:
                    Console.Write("Курс конвертации: ");
                    double rate = double.Parse(Console.ReadLine());
                    foreach (var e in expenses)
                        Console.WriteLine($"{e.Name} - {e.Amount * rate:F2}");
                    break;

                case 5:
                    Console.Write("Поиск по названию: ");
                    string searchTerm = Console.ReadLine().ToLower();
                    var found = expenses.Where(e => e.Name.ToLower().Contains(searchTerm)).ToList();
                    if (found.Count > 0)
                        foreach (var f in found) Console.WriteLine(f.Name + " - " + f.Amount);
                    else
                        Console.WriteLine("Не найдено.");
                    break;

                case 0:
                    return;
            }
        }
    }
}
