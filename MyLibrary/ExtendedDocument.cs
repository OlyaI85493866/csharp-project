using System;

namespace MyLibrary
{
    public class ExtendedDocument : FamilyDocument
    {
        private string _author;

        public ExtendedDocument(int id, string title, string author)
            : base(id, title)
        {
            _author = author;
        }

        public void CapitalizeTitle()
        {
            Title = char.ToUpper(Title[0]) + Title.Substring(1).ToLower();
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Author: {_author}");
        }
    }
}