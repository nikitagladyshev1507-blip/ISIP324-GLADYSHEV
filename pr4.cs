using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace praktika_4
{
    internal class Program
    {
        enum Genre
        {
            Fiction,
            Fantasy,
            Detective,
            ScienceFiction
        };
        class Book
        {
            private int _id;
            public int Id { get => _id; }
            private string _title;
            public string Title
            {
                get { return _title; }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("У книги должно быть название");
                    }
                    else
                    {
                        _title = value;
                    }
                }
            }
            private string _author;
            public string Author
            {
                get { return _author; }
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("У книги должен быть автор. ");
                    }
                    else
                    {
                        _author = value;
                    }
                }
            }
            public Genre BookGenre { get; set; }
            private int _year;
            public int Year
            {
                get
                { return _year; }
                set
                {
                    if (value >= 1000 && value <= 2026)
                    {
                        _year = value;
                    }
                    else
                    {
                        throw new ArgumentException("Год издания книги должен быть в диапазоне от 1000 г. до 2026 г.");
                    }
                }
            }
            private decimal _price;
            public decimal Price
            {
                get { return _price; }
                set
                {
                    if (value < 0)
                    {
                        throw new ArgumentException("Цена книги не может быть отрицательной");
                    }
                    else
                    {
                        _price = value;
                    }
                }
            }
            public Book(Genre genre, int year, decimal price, string author, string title, int id)
            {
                Price = price;
                Author = author;
                Title = title;
                Year = year;
                BookGenre = genre;
                _id = id;
            }
            public override string ToString() => $"[{Id}] \"{Title}\" — {Author} ({BookGenre}, {Year}) — {Price} руб.";

        }
        class LibraryService
        {
            private readonly List<Book> _books = new List<Book>();
            private int _nextId = 1;
            public Book AddBook(string title, string author, decimal price, int year, Genre genre)
            {
                Book book = new Book(genre, year, price, author, title, _nextId);
                _books.Add(book);
                _nextId++;
                return book;
            }
            public List<Book> GetAll()
            {
                return _books;
            }
            public bool DeleteById(int id) //метод удаления через перебор параметра id
            {
                Book book = _books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    return false;
                }
                else
                {
                    _books.Remove(book);
                    return true;
                }
            }
            public List<Book> SearchByTitle(string part)
            {
                return _books.Where(b => b.Title.ToLower().Contains(part.ToLower())).ToList();
            }
            public List<Book> SearchByAuthor(string part)
            {
                return _books.Where(b => b.Author.ToLower().Contains(part.ToLower())).ToList();
            }
            public List<Book> SearchByGenre(Genre genre)
            {
                return _books.Where(b => b.BookGenre == genre).ToList();
            }
            public List<Book> SortByTitle()
            {
                return _books.OrderBy(b => b.Title).ToList();
            }
            public List<Book> SortByYear()
            {
                return _books.OrderBy(b => b.Year).ToList();
            }
            public Book GetMostExpensive()
            {
                return _books.OrderByDescending(b => b.Price).FirstOrDefault();
            }
            public Book GetMostCheapest()
            {
                return _books.OrderBy(b => b.Price).FirstOrDefault();
            }
            public List<IGrouping<string, Book>> GroupByAuthor()
            {
                return _books.GroupBy(b => b.Author).ToList();
                
                {
                
            }
        }
        }
        static class ConsoleHelper
        {
            public static string ReadNonEmptyString(string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        return input;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: поле не может быть пустым. Повторите.");
                    }
                }
            }
            public static int ReadInt(int min, int max, string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out int number) && number >= min && number <= max)
                    {
                        return number;
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: введите число от {min} до {max}. Повторите.");
                    }
                }
            }
            public static decimal ReadDecimal(decimal min, string prompt)
            {
                while (true)
                {
                    Console.Write(prompt);
                    string input = Console.ReadLine();
                    if (decimal.TryParse(input, out decimal number) && number >= min)
                    {
                        return number;
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка: введите число не меньше {min}. Повторите.");
                    }
                }
            }
            public static Genre ReadGenre() //метод для выбора жанра из списка
            {
                Console.WriteLine("Выберите жанр: ");
                Genre[] all = (Genre[])Enum.GetValues(typeof(Genre));
                for (int i = 0; i < all.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {all[i]}");
                }
                int choice = ReadInt(1, all.Length, "Ваш выбор: ");
                return all[choice - 1];
            }
            public static void PrintBooks(IEnumerable<Book> books)
            {
                if (!books.Any())
                {
                    Console.WriteLine("Ничего не найдено.");
                    return;
                }
                foreach (Book b in books)
                    Console.WriteLine(b);
            }
            public static void Pause()
            {
                Console.WriteLine();
                Console.Write("Нажмите Enter, чтобы продолжить...");
                Console.ReadLine();
            }
        }
        static class SeedData
        {
            public static void Fill(LibraryService service)
            {
                service.AddBook("Мастер и Маргарита", "Булгаков", 250, 1967, Genre.Fiction);
                service.AddBook("1984", "Оруэлл", 700, 1949, Genre.ScienceFiction);
                service.AddBook("Властелин колец", "Толкин", 1200, 1954, Genre.Fantasy);
                service.AddBook("Убийство в Восточном экспрессе", "Кристи", 550, 1934, Genre.Detective);
                service.AddBook("Гарри Поттер и философский камень", "Роулинг", 950, 1997, Genre.Fantasy);
            }
        }

        static void Main(string[] args)
        {
            LibraryService service = new LibraryService();
            SeedData.Fill(service);
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== БИБЛИОТЕКА =====");
                Console.WriteLine("1.Добавить книгу");
                Console.WriteLine("2.Удалить книгу по Id");
                Console.WriteLine("3.Найти книгу");
                Console.WriteLine("4.Сортировать книги");
                Console.WriteLine("5.Самая дорогая и самая дешёвая");
                Console.WriteLine("6.Группировка по авторам");
                Console.WriteLine("7.Показать все книги");
                Console.WriteLine("0.Выход");

                int choice = ConsoleHelper.ReadInt(0, 7, "Ваш выбор: ");
                try
                {
                    switch (choice)
                    {
                        case 1: HandleAdd(service); break;

                        case 2: HandleDelete(service); break;

                        case 3: HandleSearch(service); break;

                        case 4: HandleSort(service); break;

                        case 5: HandleMinMax(service); break;

                        case 6: HandleGroupBy(service); break;

                        case 7: HandleShowAll(service); break;

                        case 0: return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
                ConsoleHelper.Pause();
            }
        }
        static void HandleShowAll(LibraryService service) //обработчик для 7 пункта меню
        {
            ConsoleHelper.PrintBooks(service.GetAll());
        }
        static void HandleMinMax(LibraryService service) //обработчик для 5 пункта меню
        {
            Book max = service.GetMostExpensive();
            Book min = service.GetMostCheapest();
            if (max == null || min == null)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }
            Console.WriteLine($"Самая дорогая книга: {max}");
            Console.WriteLine($"Самая дешевая книга: {min}");
        }
        static void HandleGroupBy(LibraryService service) //обработчик для 6 пункта
        {
            List<IGrouping<string, Book>> groups = service.GroupByAuthor();
            if (groups.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }
            foreach (IGrouping<string, Book> group in groups)
            {

                // group.Count() — сколько книг у этого автора (LINQ-метод Count)
                Console.WriteLine("Автор: " + group.Key + " - " + group.Count() + " книг");
            }
        }
        static void HandleAdd(LibraryService service)
        {
            string title = ConsoleHelper.ReadNonEmptyString("Название: ");
            string author = ConsoleHelper.ReadNonEmptyString("Автор: ");
            Genre genre = ConsoleHelper.ReadGenre();
            int year = ConsoleHelper.ReadInt(1000, 2026, "Год издания: ");
            decimal price = ConsoleHelper.ReadDecimal(0m, "Цена: ");
            Book book = service.AddBook(title, author, price, year, genre);
            Console.WriteLine($"Книга добавлена: {book}");
        }
        static void HandleDelete(LibraryService service)
        {
            int id = ConsoleHelper.ReadInt(1, int.MaxValue, "ID для удаления: ");
            bool ok = service.DeleteById(id);
            if (ok == true)
            {
                Console.WriteLine("Книга удалена.");
            }
            else
            {
                Console.WriteLine("Книга с таким Id не найдена.");
            }
        }
        static void HandleSearch(LibraryService service)
        {
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По автору");
            Console.WriteLine("3. По жанру");
            int choice = ConsoleHelper.ReadInt(1, 3, "Выбор: ");
            switch (choice)
            {
                case 1:
                    string titlepart = ConsoleHelper.ReadNonEmptyString("Часть названия: ");
                    ConsoleHelper.PrintBooks(service.SearchByTitle(titlepart));
                    break;
                case 2:
                    string authorpart = ConsoleHelper.ReadNonEmptyString("Часть автора: ");
                    ConsoleHelper.PrintBooks(service.SearchByAuthor(authorpart));
                    break;
                case 3:
                    Genre genre = ConsoleHelper.ReadGenre();
                    ConsoleHelper.PrintBooks(service.SearchByGenre(genre));
                    break;
            }
        }
        static void HandleSort(LibraryService service)
        {
            Console.WriteLine("1. По названию");
            Console.WriteLine("2. По году");
            int choice = ConsoleHelper.ReadInt(1, 2, "Выбор: ");
            switch (choice)
            {
                case 1:
                    ConsoleHelper.PrintBooks(service.SortByTitle());
                    break;
                case 2:
                    ConsoleHelper.PrintBooks(service.SortByYear());
                    break;
            }
        }
    }
}
