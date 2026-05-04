public class Curator : IPerson, ISetAchievement<Monitor>
{
    public string Name { get; set; }
    public string Status { get; set; }

    public Curator(string name, string status)
    {
        Name = name;
        Status = status;
    }

    public void SetAchievement(Monitor person, string achievement)
    {
        person.Achievement = achievement;
    }
}