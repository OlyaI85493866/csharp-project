public class Student : IPerson, IAchievement
{
    public string Name { get; set; }
    public string Status { get; set; }
    public string Achievement { get; set; }

    public Student(string name, string status, string achievement)
    {
        Name = name;
        Status = status;
        Achievement = achievement;
    }
}