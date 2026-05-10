using System.Text.Json;

namespace FamilyDocumentsWinForms.Services
{
    public class OwnerStorageService
    {
        private readonly string _filePath;

        public OwnerStorageService()
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "owners.json");
        }

        public List<string> LoadOwners()
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

            List<string>? owners = JsonSerializer.Deserialize<List<string>>(json);

            return owners ?? new List<string>();
        }

        public void SaveOwners(List<string> owners)
        {
            string json = JsonSerializer.Serialize(owners, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_filePath, json);
        }
    }
}