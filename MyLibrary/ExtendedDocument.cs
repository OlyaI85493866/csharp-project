using System;

namespace MyLibrary
{
    public class ExtendedDocument : FamilyDocument
    {
        public string Author { get; set; }

        public ExtendedDocument(int id, string title, string author)
            : base(id, title)
        {
            Author = author;
        }

        public void CapitalizeTitle()
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                Title = char.ToUpper(Title[0]) + Title.Substring(1).ToLower();
            }
        }

        public override string GetInfo()
        {
            return $"{base.GetInfo()}, Автор: {Author}";
        }
    }
}