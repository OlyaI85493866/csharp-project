using System;

namespace MyLibrary
{
    public abstract class Base
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Base(int id, string title)
        {
            Id = id;
            Title = title;
        }

        public abstract string GetInfo();
    }
}