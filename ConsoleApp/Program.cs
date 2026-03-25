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

using System;
using MyLibrary;
class Program
{
    static void Main()
    {
        int userNumber;
        string userText;

        InputValues(out userNumber, out userText);

        ExtendedDocument userDoc = new ExtendedDocument(userNumber, userText, "Автор неизвестен");
        userDoc.CapitalizeTitle();
        userDoc.PrintInfo();

        string surname = "Изместьева";
        CheckSurnameInString(surname);
    }

    static void InputValues(out int number, out string text)
    {
        Console.Write("\nВведите число: ");
        number = int.Parse(Console.ReadLine());

        Console.Write("Введите текст: ");
        text = Console.ReadLine();
    }

    static void CheckSurnameInString(string surname)
    {
        Console.Write("\nВведите текст для поиска фамилии: ");
        string input = Console.ReadLine();

        string[] words = input.Split(' ', ',', '.');

        bool found = false;
        foreach (string word in words)
        {
            if (string.Compare(word, surname) == 0)
            {
                found = true;
                break;
            }
        }
        if (found)
            Console.WriteLine("Фамилия найдена");
        else
            Console.WriteLine("Фамилия не найдена");
    }
}