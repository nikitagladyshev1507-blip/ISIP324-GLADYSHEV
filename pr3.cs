using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Stat
    {
        public string Text { get; set; }
        public int Couth { get; set; }
        public string Korot { get; set; }
        public string Dlin { get; set; }
        public int Kol { get; set; }
        public int Glas { get; set; }
        public int Soglas { get; set; }
        public Dictionary<char, int> Chastota { get; set; }


    }


    internal class Program
    {
        public static Dictionary<char, int> Statistika(string Text)
        {
            Dictionary<char, int> letterCount = new Dictionary<char, int>();

            foreach (char c in Text)
            {
                char carattere = char.ToLower(c);
                if (char.IsLetter(carattere))
                {
                    if (letterCount.ContainsKey(carattere))
                        letterCount[carattere]++;
                    else
                        letterCount[carattere] = 1;
                }
            }
            return letterCount;
        }

        static int Glas(string Text)
        {
            int kol = 0;
            string glas = "аеёиоуыэюяaeiouyАЕЁИОУЫЭЮЯAEIOUY";
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                for (int j = 0; j < glas.Length; j++)
                {
                    if (c == glas[j]) kol++;
                }
            }
            return kol;
        }

        static int Soglas(string Text)
        {
            int kol = 0;
            string soglas = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyzБВГДЖЗЙКЛМНПРСТФХЦЧШЩBCDFGHJKLMNPQRSTVWXYZ";
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                for (int j = 0; j < soglas.Length; j++)
                {
                    if (c == soglas[j]) kol++;
                }
            }
            return kol;
        }

        static int WordCouth(string Text)
        {
            char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '\n', '\t' };
            string[] words = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        static string Korot(string Text)
        {
            char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '\n', '\t' };
            string[] words = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0) return "";

            string min = words[0];
            foreach (string x in words)
            {
                if (x.Length < min.Length)
                {
                    min = x;
                }
            }
            return min;
        }

        static string Dlin(string Text)
        {
            char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '\n', '\t' };
            string[] words = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0) return "";

            string maxWord = words[0];

            foreach (string x in words)
            {
                if (x.Length > maxWord.Length)
                {
                    maxWord = x;
                }
            }
            return maxWord;
        }

        static int Sentences(string Text)
        {
            char[] separators = { '.', '!', '?' };
            string[] parts = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length;
        }
        static void PrintStat(Stat s)
        {
            Console.WriteLine($"текст: {s.Text}");
            Console.WriteLine($"количество слов: {s.Couth}");
            Console.WriteLine($"самое короткое слово: \"{s.Korot}\"");
            Console.WriteLine($"самое длинное слово: \"{s.Dlin}\"");
            Console.WriteLine($"количество предложений: {s.Kol}");
            Console.WriteLine($"гласных букв: {s.Glas}");
            Console.WriteLine($"согласных букв: {s.Soglas}");
            Console.WriteLine($"частота встречаемости букв:");

            var pairs = new KeyValuePair<char, int>[s.Chastota.Count];
            int index = 0;
            foreach (var p in s.Chastota)
            {
                pairs[index] = p;
                index++;
            }

            foreach (var pair in pairs)
            {
                Console.WriteLine($" '{pair.Key}' - {pair.Value}");
            }
        }

        static void Main(string[] args)
        {
            List<Stat> history = new List<Stat>();

            while (true)
            {
                Console.WriteLine("\nвыбери \n 1. текст новый \n 2. история \n 3. выход");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        string inputText = "";

                        while (true)
                        {
                            Console.WriteLine("текст 100");
                            inputText = Console.ReadLine();

                            if (inputText != null && inputText.Length >= 100)
                            {
                                break;
                            }
                            Console.WriteLine("мало минимум 100");
                        }

                        Stat s = new Stat
                        {
                            Text = inputText,
                            Couth = WordCouth(inputText),
                            Korot = Korot(inputText),
                            Dlin = Dlin(inputText),
                            Kol = Sentences(inputText),
                            Glas = Glas(inputText),
                            Soglas = Soglas(inputText),
                            Chastota = Statistika(inputText)
                        };

                        history.Add(s);

                        Console.WriteLine("\nстатистика текста");
                        PrintStat(s);
                        break;

                    case "2":
                        if (history.Count == 0)
                        {
                            Console.WriteLine("истории нет");
                        }
                        else
                        {
                            Console.WriteLine($"\n история всего {history.Count}) ");
                            for (int i = 0; i < history.Count; i++)
                            {
                                Console.WriteLine($"\nзапись {i + 1}");
                                PrintStat(history[i]);
                            }
                        }
                        break;

                    case "3":
                        Console.WriteLine("конец");
                        return;

                    default:
                        Console.WriteLine("отказ");
                        break;
                }
            }
        }
    }
}









Fdh
using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Stat
    {
        public string Text { get; set; }
        public int Couth { get; set; }
        public string Korot { get; set; }
        public string Dlin { get; set; }
        public int Kol { get; set; }
        public int Glas { get; set; }
        public int Soglas { get; set; }
        public Dictionary<char, int> Chastota { get; set; }


    }
  

    internal class Program
    {
        public static Dictionary<char, int> Statistika(string Text)
        {
            Dictionary<char, int> letterCount = new Dictionary<char, int>();

            foreach (char c in Text)
            {
                char carattere = char.ToLower(c);
                if (char.IsLetter(carattere))
                {
                    if (letterCount.ContainsKey(carattere))
                        letterCount[carattere]++;
                    else
                        letterCount[carattere] = 1;
                }
            }
            return letterCount;
        }

        static int Glas(string Text)
        {
            int kol = 0;
            string glas = "аеёиоуыэюяaeiouy";
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                for (int j = 0; j < glas.Length; j++)
                {
                    if (c == glas[j]) kol++;
                }
            }
            return kol;
        }

        static int Soglas(string Text)
        {
            int kol = 0;
            string soglas = "бвгджзйклмнпрстфхцчшщbcdfghjklmnpqrstvwxyzБВГДЖЗЙКЛМНПРСТФХЦЧШЩBCDFGHJKLMNPQRSTVWXYZ";
            for (int i = 0; i < Text.Length; i++)
            {
                char c = Text[i];
                for (int j = 0; j < soglas.Length; j++)
                {
                    if (c == soglas[j]) kol++;
                }
            }
            return kol;
        }

        static int WordCouth(string Text)
        {
            char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '\n', '\t' };
            string[] words = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        static string Korot(string Text)
        {
            char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '\n', '\t' };
            string[] words = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0) return "";

            string min = words[0];
            foreach (string x in words)
            {
                if (x.Length < min.Length)
                {
                    min = x;
                }
            }
            return min;
        }

        static string Dlin(string Text)
        {
            char[] separators = { ' ', '.', ',', '!', '?', ';', ':', '\n', '\t' };
            string[] words = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0) return "";

            string maxWord = words[0];

            foreach (string x in words)
            {
                if (x.Length > maxWord.Length)
                {
                    maxWord = x;
                }
            }
            return maxWord;
        }

        static int Sentences(string Text)
        {
            char[] separators = { '.', '!', '?' };
            string[] parts = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length;
        }
        static void PrintStat(Stat s)
        {
            Console.WriteLine($"текст: {s.Text}");
            Console.WriteLine($"количество слов: {s.Couth}");
            Console.WriteLine($"самое короткое слово: \"{s.Korot}\"");
            Console.WriteLine($"самое длинное слово: \"{s.Dlin}\"");
            Console.WriteLine($"количество предложений: {s.Kol}");
            Console.WriteLine($"гласных букв: {s.Glas}");
            Console.WriteLine($"согласных букв: {s.Soglas}");
            Console.WriteLine($"частота встречаемости букв:");

            var pairs = new KeyValuePair<char, int>[s.Chastota.Count];
            int index = 0;
            foreach (var p in s.Chastota)
            {
                pairs[index] = p;
                index++;
            }

            foreach (var pair in pairs)
            {
                Console.WriteLine($" '{pair.Key}' - {pair.Value}");
            }
        }

        static void Main(string[] args)
        {
            List<Stat> history = new List<Stat>();

            while (true)
            {
                Console.WriteLine("\nвыбери \n 1. текст новый \n 2. история \n 3. выход");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        string inputText = "";

                        while (true)
                        {
                            Console.WriteLine("текст 100");
                            inputText = Console.ReadLine();

                            if (inputText != null && inputText.Length >= 100)
                            {
                                break;
                            }
                            Console.WriteLine("мало минимум 100");
                        }

                        Stat s = new Stat
                        {
                            Text = inputText,
                            Couth = WordCouth(inputText),
                            Korot = Korot(inputText),
                            Dlin = Dlin(inputText),
                            Kol = Sentences(inputText),
                            Glas = Glas(inputText),
                            Soglas = Soglas(inputText),
                            Chastota = Statistika(inputText)
                        };

                        history.Add(s);

                        Console.WriteLine("\nстатистика текста");
                        PrintStat(s);
                        break;

                    case "2":
                        if (history.Count == 0)
                        {
                            Console.WriteLine("истории нет");
                        }
                        else
                        {
                            Console.WriteLine($"\n история всего {history.Count}) ");
                            for (int i = 0; i < history.Count; i++)
                            {
                                Console.WriteLine($"\nзапись {i + 1}");
                                PrintStat(history[i]);
                            }
                        }
                        break;

                    case "3":
                        Console.WriteLine("конец");
                        return;

                    default:
                        Console.WriteLine("ошибка");
                        break;
                }
            }
        }
    }
}


