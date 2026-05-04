using System;

namespace MyLibrary
{
    public class FamilyDocument : Base
    {
        public string Category { get; set; }
        public string Owner { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool IsImportant { get; set; }
        public string FilePath { get; set; }
        public string Comment { get; set; }

        public FamilyDocument(int id, string title) : base(id, title)
        {
            Category = string.Empty;
            Owner = string.Empty;
            DocumentNumber = string.Empty;
            DocumentDate = DateTime.Now;
            ExpirationDate = null;
            IsImportant = false;
            FilePath = string.Empty;
            Comment = string.Empty;
        }

        public override string GetInfo()
        {
            string expirationDateText = ExpirationDate.HasValue
                ? ExpirationDate.Value.ToString("dd.MM.yyyy")
                : "не указан";

            return $"ID: {Id}\n" +
                   $"Название: {Title}\n" +
                   $"Категория: {Category}\n" +
                   $"Владелец: {Owner}\n" +
                   $"Номер документа: {DocumentNumber}\n" +
                   $"Дата документа: {DocumentDate:dd.MM.yyyy}\n" +
                   $"Срок действия: {expirationDateText}\n" +
                   $"Важный документ: {(IsImportant ? "да" : "нет")}\n" +
                   $"Файл: {FilePath}\n" +
                   $"Комментарий: {Comment}";
        }
    }
}