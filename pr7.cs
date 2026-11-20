using ConsoleApp8;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GmwogMarketplace
{
    class Program
    {
        private static GmwogMarketplace.User _currentUser;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== GMWOG MARKETPLACE ===");
                if (_currentUser != null)
                    Console.WriteLine($"Пользователь: {_currentUser.Nickname}\n");

                Console.WriteLine("1. Регистрация");
                Console.WriteLine("2. Войти");
                Console.WriteLine("3. Каталог товаров");
                if (_currentUser != null)
                {
                    Console.WriteLine("4. Корзина");
                    Console.WriteLine("5. Добавить в корзину");
                    Console.WriteLine("6. Купить товар");
                    Console.WriteLine("7. Оформить корзину");
                    Console.WriteLine("8. История заказов");
                    Console.WriteLine("9. Выйти");
                }
                Console.WriteLine("0. Выход");
                Console.Write("\nВыбор: ");

                var key = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (key)
                {
                    case '1': Register(); break;
                    case '2': Login(); break;
                    case '3': ShowProducts(); break;
                    case '4' when _currentUser != null: ShowCart(); break;
                    case '5' when _currentUser != null: AddToCart(); break;
                    case '6' when _currentUser != null: BuySingle(); break;
                    case '7' when _currentUser != null: CheckoutCart(); break;
                    case '8' when _currentUser != null: ShowHistory(); break;
                    case '9' when _currentUser != null: _currentUser = null; break;
                    case '0': running = false; break;
                }

                if (key != '0')
                {
                    Console.WriteLine("\nНажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }

        static void Register()
        {
            Console.Write("Никнейм: ");
            var nick = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(nick)) { Console.WriteLine("Пусто!"); return; }

            string pass = ReadPass("Пароль: ");
            string pass2 = ReadPass("Повторите пароль: ");

            if (pass != pass2) { Console.WriteLine("Пароли не совпадают!"); return; }

            using (var db = new pr7GEntities())
            {
                if (db.User.Any(u => u.Nickname == nick))
                { Console.WriteLine("Ник занят!"); return; }

                db.User.Add(new User
                {
                    Nickname = nick,
                    PasswordHash = Hash(pass),
                    CreatedAt = DateTime.Now
                });
                db.SaveChanges();
                Console.WriteLine("Регистрация успешна!");
            }
        }

        static void Login()
        {
            Console.Write("Никнейм: ");
            var nick = Console.ReadLine()?.Trim();
            string pass = ReadPass("Пароль: ");

            using (var db = new pr7GEntities())
            {
                var user = db.User.FirstOrDefault(u => u.Nickname == nick);
                if (user == null || user.PasswordHash != Hash(pass))
                { Console.WriteLine("Неверно!"); return; }

                _currentUser = user;
                Console.WriteLine($"Привет, {user.Nickname}!");
            }
        }

        static void ShowProducts()
        {
            using (var db = new pr7GEntities())
            {
                var list = db.Product.Where(p => p.IsAvailable && p.Stock > 0).ToList();
                if (!list.Any()) { Console.WriteLine("Нет товаров"); return; }

                Console.WriteLine("\n{0,-5} {1,-25} {2,-10} {3,-5}", "ID", "Название", "Цена", "Ост.");
                Console.WriteLine(new string('-', 50));
                foreach (var p in list)
                    Console.WriteLine("{0,-5} {1,-25} {2,-10} {3,-5}", p.Id, p.Name, p.Price, p.Stock);
            }
        }

        static void AddToCart()
        {
            ShowProducts();
            Console.Write("\nID товара: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0) return;

            using (var db = new pr7GEntities())
            {
                var prod = db.Product.FirstOrDefault(p => p.Id == id && p.Stock > 0);
                if (prod == null) { Console.WriteLine("Нет товара"); return; }
                if (prod.Stock < qty) { Console.WriteLine("Мало на складе"); return; }

                var cart = db.Cart.FirstOrDefault(c => c.UserId == _currentUser.Id && c.ProductId == id);
                if (cart != null)
                    cart.Quantity += qty;
                else
                    db.Cart.Add(new Cart { UserId = _currentUser.Id, ProductId = id, Quantity = qty });

                db.SaveChanges();
                Console.WriteLine("Добавлено!");
            }
        }

        static void ShowCart()
        {
            using (var db = new pr7GEntities())
            {
                var items = db.Cart.Where(c => c.UserId == _currentUser.Id).ToList();
                if (!items.Any()) { Console.WriteLine("Корзина пуста"); return; }

                decimal total = 0;
                Console.WriteLine("\nКорзина:");
                foreach (var item in items)
                {
                    var prod = db.Product.First(p => p.Id == item.ProductId);
                    var sum = prod.Price * item.Quantity;
                    total += sum;
                    Console.WriteLine($"- {prod.Name} x{item.Quantity} = {sum:F2}");
                }
                Console.WriteLine($"Итого: {total:F2}");
            }
        }

        static void BuySingle()
        {
            ShowProducts();
            Console.Write("\nID товара: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) return;

            Console.Write("Количество: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0) return;

            using (var db = new pr7GEntities())
            {
                var prod = db.Product.FirstOrDefault(p => p.Id == id && p.Stock >= qty);
                if (prod == null) { Console.WriteLine("Ошибка"); return; }

                var pp = SelectPickupPoint(db);  
                if (pp == null) return;

                var order = new Order
                {
                    UserId = _currentUser.Id,
                    PickupPointId = pp.Id,
                    OrderDate = DateTime.Now,
                    TotalAmount = prod.Price * qty
                };
                db.Order.Add(order);
                db.SaveChanges();

                db.OrderItem.Add(new OrderItem  
                {
                    OrderId = order.Id,
                    ProductId = prod.Id,
                    Quantity = qty,
                    Price = prod.Price
                });
                prod.Stock -= qty;
                db.SaveChanges();

                Console.WriteLine($"Заказ #{order.Id} оформлен! Сумма: {order.TotalAmount:F2}");
                Console.WriteLine($"ПВЗ: {pp.Name}, {pp.Address}");
            }
        }

        static void CheckoutCart()
        {
            using (var db = new pr7GEntities())
            {
                var items = db.Cart.Where(c => c.UserId == _currentUser.Id).ToList();
                if (!items.Any()) { Console.WriteLine("Корзина пуста"); return; }

                foreach (var item in items)
                {
                    var prod = db.Product.First(p => p.Id == item.ProductId);
                    if (prod.Stock < item.Quantity)
                    { Console.WriteLine($"Мало '{prod.Name}'"); return; }
                }

                var pp = SelectPickupPoint(db);
                if (pp == null) return;

                decimal total = items.Sum(i => db.Product.First(p => p.Id == i.ProductId).Price * i.Quantity);

                var order = new Order
                {
                    UserId = _currentUser.Id,
                    PickupPointId = pp.Id,
                    OrderDate = DateTime.Now,
                    TotalAmount = total
                };
                db.Order.Add(order);
                db.SaveChanges();

                foreach (var item in items)
                {
                    var prod = db.Product.First(p => p.Id == item.ProductId);
                    db.OrderItem.Add(new OrderItem
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = prod.Price
                    });
                    prod.Stock -= item.Quantity;
                }

                db.Cart.RemoveRange(items);  
                db.SaveChanges();

                Console.WriteLine($"Заказ #{order.Id} оформлен! Сумма: {total:F2}");
                Console.WriteLine($"ПВЗ: {pp.Name}, {pp.Address}");
            }
        }

        static void ShowHistory()
        {
            using (var db = new pr7GEntities())
            {
                var orders = db.Order.Where(o => o.UserId == _currentUser.Id).ToList();
                if (!orders.Any()) { Console.WriteLine("Нет заказов"); return; }

                Console.WriteLine("1. Сначала новые\n2. Сначала старые");
                Console.Write("Выбор: ");
                bool asc = Console.ReadLine()?.Trim() == "2";
                orders = asc ? orders.OrderBy(o => o.OrderDate).ToList() : orders.OrderByDescending(o => o.OrderDate).ToList();

                foreach (var order in orders)
                {
                    var pp = db.PickupPoint.First(p => p.Id == order.PickupPointId);
                    var items = db.OrderItem.Where(i => i.OrderId == order.Id).ToList();

                    Console.WriteLine($"\nЗаказ #{order.Id} от {order.OrderDate:dd.MM.yyyy HH:mm}");
                    Console.WriteLine($"ПВЗ: {pp.Name}, {pp.Address}");
                    foreach (var item in items)
                    {
                        var prod = db.Product.First(p => p.Id == item.ProductId);
                        Console.WriteLine($"  {prod.Name} x{item.Quantity} = {item.Price * item.Quantity:F2}");
                    }
                    Console.WriteLine($"Итого: {order.TotalAmount:F2}");
                }
            }
        }

        static PickupPoint SelectPickupPoint(pr7GEntities db)
        {
            var points = db.PickupPoint.ToList();
            if (!points.Any()) { Console.WriteLine("Нет ПВЗ"); return null; }

            Console.WriteLine("\nПВЗ:");
            for (int i = 0; i < points.Count; i++)
                Console.WriteLine($"{i + 1}. {points[i].Name} - {points[i].Address}");

            Console.Write("Выбор: ");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > points.Count)
            { Console.WriteLine("Ошибка"); return null; }

            return points[choice - 1];
        }

        static string Hash(string pass)
        {
            using (var sha = SHA256.Create())
                return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(pass)));
        }

        static string ReadPass(string prompt)
        {
            Console.Write(prompt);
            var sb = new StringBuilder();
            while (true)
            {
                var k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
                if (k.Key == ConsoleKey.Backspace && sb.Length > 0)
                { sb.Remove(sb.Length - 1, 1); Console.Write("\b \b"); }
                else if (!char.IsControl(k.KeyChar))
                { sb.Append(k.KeyChar); Console.Write("*"); }
            }
            return sb.ToString();
        }
    }
}
