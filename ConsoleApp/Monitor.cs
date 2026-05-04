public class Monitor : IPerson, IAchievement, ISetAchievement<Student>
{
    public string Name { get; set; }
    public string Status { get; set; }
    public string Achievement { get; set; }

    public Monitor(string name, string status, string achievement)
    {
        Name = name;
        Status = status;
        Achievement = achievement;
    }

    public void SetAchievement(Student person, string achievement)
    {
        person.Achievement = achievement;
    }
}