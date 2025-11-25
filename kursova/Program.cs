using System;
using System.Collections.Generic;
using System.Text;
using Library.BLL.Entities;
using Library.BLL.Exceptions;
using Library.BLL.Interfaces;
using Library.BLL.Services;
using Library.DAL.Repositories;

namespace Library.ConsoleApp
{
    class Program
    {
        private static ILibraryService _service;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            ILibraryRepository repository = new LibraryRepository();
            _service = new LibraryService(repository);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА ОБЛІКУ БІБЛІОТЕКИ (Варіант 7) ===");
                Console.WriteLine("1. Управління читачами");
                Console.WriteLine("2. Управління документами");
                Console.WriteLine("3. Управління видачами");
                Console.WriteLine("4. Вихід");
                Console.Write("Ваш вибір: ");

                switch (Console.ReadLine())
                {
                    case "1": UsersMenu(); break;
                    case "2": DocsMenu(); break;
                    case "3": LoansMenu(); break;
                    case "4": return;
                }
            }
        }

        // --- МЕНЮ КОРИСТУВАЧІВ (ЧИТАЧІВ) ---
        static void UsersMenu()
        {
            Console.Clear();
            Console.WriteLine("--- ЧИТАЧІ (УПРАВЛІННЯ) ---");
            Console.WriteLine("1. Додати читача");
            Console.WriteLine("2. Переглянути всіх (опції сортування)");
            Console.WriteLine("3. Видалити читача");
            Console.WriteLine("4. Пошук читачів");
            Console.WriteLine("5. Назад");
            Console.Write("Ваш вибір: ");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        _service.AddUser(new User
                        {
                            Id = ReadInt("ID читача"),
                            FirstName = ReadStr("Ім'я"),
                            LastName = ReadStr("Прізвище"),
                            AcademicGroup = ReadStr("Академічна група")
                        });
                        break;
                    case "2":
                        Console.WriteLine("\nСортувати за: 1.Ім'ям 2.Прізвищем 3.Групою 0.Без сортування");
                        Console.Write("Опція сортування: ");
                        var sort = Console.ReadLine() switch { "1" => "name", "2" => "lastname", "3" => "group", _ => "" };
                        PrintList(_service.GetUsersSorted(sort));
                        break;
                    case "3": _service.RemoveUser(ReadInt("ID читача для видалення")); break;
                    case "4": PrintList(_service.SearchUsers(ReadStr("Ключове слово для пошуку"))); break;
                    case "5": return;
                }
                Console.WriteLine("\nОперація успішна.");
            }
            catch (Exception ex) { Console.WriteLine($"\nПомилка: {ex.Message}"); }
            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }

        // --- МЕНЮ ДОКУМЕНТІВ ---
        static void DocsMenu()
        {
            Console.Clear();
            Console.WriteLine("--- ДОКУМЕНТИ (УПРАВЛІННЯ) ---");
            Console.WriteLine("1. Додати документ");
            Console.WriteLine("2. Переглянути всі (опції сортування)");
            Console.WriteLine("3. Видалити документ");
            Console.WriteLine("4. Пошук документів");
            Console.WriteLine("5. Назад");
            Console.Write("Ваш вибір: ");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        _service.AddDocument(new Document
                        {
                            Id = ReadInt("ID документа"),
                            Title = ReadStr("Назва"),
                            Author = ReadStr("Автор"),
                            Year = ReadInt("Рік видання")
                        });
                        break;
                    case "2":
                        Console.WriteLine("\nСортувати за: 1.Назвою 2.Автором 0.Без сортування");
                        Console.Write("Опція сортування: ");
                        var sort = Console.ReadLine() switch { "1" => "title", "2" => "author", _ => "" };
                        PrintList(_service.GetDocumentsSorted(sort));
                        break;
                    case "3": _service.RemoveDocument(ReadInt("ID документа для видалення")); break;
                    case "4": PrintList(_service.SearchDocuments(ReadStr("Ключове слово для пошуку"))); break;
                    case "5": return;
                }
                Console.WriteLine("\nОперація успішна.");
            }
            catch (Exception ex) { Console.WriteLine($"\nПомилка: {ex.Message}"); }
            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }

        // --- МЕНЮ ВИДАЧІ ---
        static void LoansMenu()
        {
            Console.Clear();
            Console.WriteLine("--- ВИДАЧА ЛІТЕРАТУРИ (ФОРМУЛЯРИ) ---");
            Console.WriteLine("1. Оформити видачу");
            Console.WriteLine("2. Оформити повернення");
            Console.WriteLine("3. Переглянути позики читача");
            Console.WriteLine("4. Перевірити статус документа");
            Console.WriteLine("5. Переглянути активні позики (всі)");
            Console.WriteLine("6. Назад");
            Console.Write("Ваш вибір: ");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1": _service.LoanDocument(ReadInt("ID читача"), ReadInt("ID документа")); break;
                    case "2": _service.ReturnDocument(ReadInt("ID позики для повернення")); break;
                    case "3": PrintList(_service.GetUserLoans(ReadInt("ID читача"))); break;
                    case "4":
                        var loan = _service.GetDocumentStatus(ReadInt("ID документа"));
                        Console.WriteLine(loan != null ? $"Видано читачу з ID: {loan.UserId} (до {loan.DueDate:yyyy-MM-dd})" : "Доступно у фонді");
                        break;
                    case "5": PrintList(_service.GetActiveLoans()); break;
                    case "6": return;
                }
                Console.WriteLine("\nОперація завершена.");
            }
            catch (Exception ex) { Console.WriteLine($"\nПомилка: {ex.Message}"); }
            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }

        // --- ДОПОМІЖНІ МЕТОДИ (Helpers) ---
        static int ReadInt(string p)
        {
            while (true)
            {
                Console.Write($"Введіть {p}: ");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int v))
                    return v;

                Console.WriteLine("Некоректне введення. Будь ласка, введіть число.");
            }
        }

        static string ReadStr(string p)
        {
            Console.Write($"Введіть {p}: ");
            return Console.ReadLine();
        }

        static void PrintList<T>(System.Collections.Generic.List<T> list)
        {
            if (list.Count == 0)
                Console.WriteLine("Список порожній.");
            else
                list.ForEach(x => Console.WriteLine(x));
        }
    }
}