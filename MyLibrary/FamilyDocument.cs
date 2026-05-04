using System;

namespace MyLibrary
{
    public class FamilyDocument : Base
    {
        public FamilyDocument(int id, string title)
            : base(id, title)
        {
        }

        public override string GetInfo()
        {
            return $"Id: {Id}, Название: {Title}";
        }
    }
}