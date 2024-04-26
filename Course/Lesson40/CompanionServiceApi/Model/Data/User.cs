using System.ComponentModel.DataAnnotations;


public class User 
{
    public int ID { get; set; }


    [StringLength(255, MinimumLength = 1)]
    public string? Name { get; set; }

    public DateOnly? Date { get; set; }
    public int? Reward { get; set; }
    public string? Origin { get; set; }
    public string? Destination { get; set; }

    public User(string name, DateOnly date, int reward)
    {
        Name = name;
        Date = date;
        Reward = reward;
    }

    public User(string name)
    {
        Name = name;
    }

    public User() { }

}