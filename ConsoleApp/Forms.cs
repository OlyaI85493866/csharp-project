using System;
using System.Collections.Generic;
using MyLibrary;

public class FormLogic
{
    private List<FamilyDocument> documents = new List<FamilyDocument>();

    public void AddDocument(string title, string type, bool important)
    {
        int id = documents.Count + 1;

        FamilyDocument doc;

        if (type == "Паспорт")
        {
            doc = new ExtendedDocument(id, title, "Неизвестный");
        }
        else
        {
            doc = new FamilyDocument(id, title);
        }

        if (important)
        {
            doc.Title += " (Важно)";
        }

        documents.Add(doc);
    }

    public string GetDocumentInfo(int index)
    {
        if (index < 0 || index >= documents.Count)
            return "Ошибка";

        return documents[index].GetInfo();
    }

    public List<FamilyDocument> GetAll()
    {
        return documents;
    }

    public string GetImagePath(string type)
    {
        switch (type)
        {
            case "Паспорт":
                return "passport.png";
            case "Свидетельство":
                return "certificate.png";
            case "Договор":
                return "contract.png";
            default:
                return "default.png";
        }
    }

    public bool CanAddDocument(string title)
{
    return !string.IsNullOrWhiteSpace(title);
}

public string GetImportanceStatus(bool important)
{
    if (important)
        return "Важный документ";

    return "Обычный документ";
}
}