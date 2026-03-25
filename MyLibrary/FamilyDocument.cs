using System;

namespace MyLibrary
{
    public class FamilyDocument : Base
    {
        public FamilyDocument(int id, string title)
            : base(id, title)
        {
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"Id: {Id}, Title: {Title}");
        }
    }
}