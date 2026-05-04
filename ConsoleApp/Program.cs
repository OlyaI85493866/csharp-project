// Console.WriteLine("\nЗадание 2а");
// Console.Write("Введите размер массива: ");
// int size = int.Parse(Console.ReadLine());
// int[] numbers = new int[size];
// for (int i = 0; i < size; i++)
// {
//     Console.Write($"Введите элемент [{i}]: ");
//     numbers[i] = int.Parse(Console.ReadLine());
// }

// int max = numbers[0];
// foreach (int num in numbers)
// {
//     if (num > max)
//         max = num;
// }
// Console.WriteLine($"Максимум: {max}");

// Console.WriteLine("\nЗадание 2б");
// string surname = "Изместьева"; 
// string[] texts = new string[numbers.Length];
// for (int i = 0; i < numbers.Length; i++)
// {
//     texts[i] = surname + numbers[i];
//     Console.WriteLine(texts[i]);
// }

// Console.WriteLine("\nЗадание 2в");
// int maxIndex = 0;
// int currentIndex = 0;
// max = numbers[0];
// foreach (int num in numbers)
// {
//     if (num > max)
//     {
//         max = num;
//         maxIndex = currentIndex;
//     }
//     currentIndex++;
// }
// FamilyDocument document = new FamilyDocument(max, texts[maxIndex]);
// Console.WriteLine("Информация об объекте:");
// document.PrintInfo();

// Console.WriteLine("\nЗадание 4");
// Console.Write("Введите количество объектов: ");
// int n = int.Parse(Console.ReadLine());

// FamilyDocument[] docs = new FamilyDocument[n];

// for (int i = 0; i < n; i++)
// {
//     Console.WriteLine($"\nОбъект #{i+1}");

//     Console.Write("Введите Id: ");
//     int id = int.Parse(Console.ReadLine());

//     Console.Write("Введите Title: ");
//     string title = Console.ReadLine();

//     docs[i] = new FamilyDocument(id, title);
// }
// Console.WriteLine("\nВсе объекты:");

// foreach (FamilyDocument d in docs)
// {
//     d.PrintInfo();
// }

// FamilyDocument maxDoc = docs[0];
// foreach (FamilyDocument d in docs)
// {
//     if (d.Id > maxDoc.Id)
//     {
//         maxDoc = d;
//     }
// }
// Console.WriteLine("\nОбъект с максимальным Id:");
// maxDoc.PrintInfo();

//Практика 3

// using System;
// using MyLibrary;
// class Program
// {
//     static void Main()
//     {
//         int userNumber;
//         string userText;

//         InputValues(out userNumber, out userText);

//         ExtendedDocument userDoc = new ExtendedDocument(userNumber, userText, "Автор неизвестен");
//         userDoc.CapitalizeTitle();
//         userDoc.PrintInfo();

//         string surname = "Изместьева";
//         CheckSurnameInString(surname);
//     }

//     static void InputValues(out int number, out string text)
//     {
//         Console.Write("\nВведите число: ");
//         number = int.Parse(Console.ReadLine());

//         Console.Write("Введите текст: ");
//         text = Console.ReadLine();
//     }

//     static void CheckSurnameInString(string surname)
//     {
//         Console.Write("\nВведите текст для поиска фамилии: ");
//         string input = Console.ReadLine();

//         string[] words = input.Split(' ', ',', '.');

//         bool found = false;
//         foreach (string word in words)
//         {
//             if (string.Compare(word, surname) == 0)
//             {
//                 found = true;
//                 break;
//             }
//         }
//         if (found)
//             Console.WriteLine("Фамилия найдена");
//         else
//             Console.WriteLine("Фамилия не найдена");
//     }
// }

// using System;
// using System.Collections.Generic;
// using MyLibrary;

// class Program
// {
//     static List<FamilyDocument> documents = new List<FamilyDocument>();

//     static void Main()
//     {
//         AddDocument("паспорт", "Паспорт", true);
//         AddDocument("свидетельство", "Свидетельство", false);

//         ShowDocuments();
//     }

//     static void AddDocument(string title, string type, bool important)
//     {
//         int id = documents.Count + 1;

//         FamilyDocument doc;

//         if (type == "Паспорт")
//         {
//             doc = new ExtendedDocument(id, title, "Неизвестный");
//         }
//         else
//         {
//             doc = new FamilyDocument(id, title);
//         }

//         if (important)
//         {
//             doc.Title += " (Важно)";
//         }

//         documents.Add(doc);
//     }

//     static void ShowDocuments()
//     {
//         foreach (var doc in documents)
//         {
//             Console.WriteLine(doc.GetInfo());
//         }
//     }
// }

using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Задание 1
        Console.WriteLine("Задание 1");
        Console.WriteLine("Введите целые числа. Для завершения введите точку.");

        List<int> numbers = new List<int>();

        while (true)
        {
            string input = Console.ReadLine();

            if (input == ".")
                break;

            int number = int.Parse(input);
            numbers.Add(number);
        }

        int sum = 0;
        bool skip = false;

        foreach (int number in numbers)
        {
            if (number == 6)
            {
                skip = true;
                continue;
            }

            if (number == 7)
            {
                skip = false;
                continue;
            }

            if (!skip)
            {
                sum += number;
            }
        }

        Console.WriteLine("Сумма = " + sum);

        // Задание 2
        Console.WriteLine("\nЗадание 2");

        string path = "text.txt";
        string[] lines = File.ReadAllLines(path);

        Console.WriteLine("Количество строк в файле: " + lines.Length);


        // Задание 3
        Console.WriteLine("\nЗадание 3");

        string inputPath = "text.txt";
        string outputPath = "result.txt";

        string[] inputLines = File.ReadAllLines(inputPath);

        List<string> allWords = new List<string>();
        List<int> wordsPerLine = new List<int>();

        foreach (string line in inputLines)
        {
            string[] words = line.Split(' ');
            wordsPerLine.Add(words.Length);

            foreach (string word in words)
            {
                allWords.Add(word);
            }
        }

        allWords.Reverse();
        wordsPerLine.Reverse();

        List<string> outputLines = new List<string>();
        int index = 0;

        foreach (int count in wordsPerLine)
        {
            List<string> currentLineWords = new List<string>();

            for (int i = 0; i < count; i++)
            {
                currentLineWords.Add(allWords[index]);
                index++;
            }

            outputLines.Add(string.Join(" ", currentLineWords));
        }

        File.WriteAllLines(outputPath, outputLines);

        Console.WriteLine("Выполнено");
        
        // Задание 4
        Console.WriteLine("\nЗадание 4");

        try
        {
            Console.Write("Введите первый логин: ");
            string login1 = Console.ReadLine();

            Console.Write("Введите второй логин: ");
            string login2 = Console.ReadLine();

            if (login1 == login2)
                throw new Exception("Логины не могут совпадать");

            string[] logins = { login1, login2 };
            Array.Sort(logins);

            Console.WriteLine("Логины по алфавиту:");
            foreach (string login in logins)
            {
                Console.WriteLine(login);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        // задание 5
        Console.WriteLine("\nЗадание 5");

        Student student1 = new Student("Аня", "Студент", "Сдал математику");
        Student student2 = new Student("Игорь", "Студент", "Победил в олимпиаде");

        Monitor monitor1 = new Monitor("Мария", "Староста", "Организовала группу");

        Curator curator1 = new Curator("Олег", "Студент-куратор МИРЭА");

        monitor1.SetAchievement(student1, "Получил автомат");
        curator1.SetAchievement(monitor1, "Лучший староста месяца");

        IPerson[] people = { student1, student2, monitor1, curator1 };

        Console.WriteLine("Все сотрудники:");
        foreach (IPerson person in people)
        {
            Console.WriteLine("Имя: " + person.Name + ", Статус: " + person.Status);
        }
    }
}