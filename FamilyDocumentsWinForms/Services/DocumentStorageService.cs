using System.Text.Json;
using MyLibrary;

namespace FamilyDocumentsWinForms.Services
{
    public class DocumentStorageService
    {
        private readonly string _filePath;

        public DocumentStorageService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "documents.json");
        }

        public List<FamilyDocument> LoadDocuments()
        {
            if (!File.Exists(_filePath))
            {
                return new List<FamilyDocument>();
            }

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<FamilyDocument>();
            }

            List<FamilyDocument>? documents = JsonSerializer.Deserialize<List<FamilyDocument>>(json);

            return documents ?? new List<FamilyDocument>();
        }

        public void SaveDocuments(List<FamilyDocument> documents)
        {
            string json = JsonSerializer.Serialize(documents, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }
    }
}