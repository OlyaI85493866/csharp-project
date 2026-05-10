using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace FamilyDocumentsWinForms.Services
{
    public class DocumentTypeStorageService
    {
        private readonly string _filePath;

        public DocumentTypeStorageService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "types.json");
        }

        public List<string> LoadTypes()
        {
            if (!File.Exists(_filePath))
            {
                return new List<string>();
            }

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<string>();
            }

            List<string>? types = JsonSerializer.Deserialize<List<string>>(json);

            return types ?? new List<string>();
        }

        public void SaveTypes(List<string> types)
        {
            string json = JsonSerializer.Serialize(types, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }
    }
}