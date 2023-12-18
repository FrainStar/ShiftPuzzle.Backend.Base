﻿namespace practicea;

class Program
{
    static void Main(string[] args)
    {
        Person people1 = new Person("Ilya", 17);
        people1.Name = "Danil";
        people1.Age = 18;
        people1.Introduce();
        Person[] mas = {people1, new Person("Egor", 17), new Person("Valera", 17)};
        foreach (var item in mas)
        {
            item.Introduce();
        }
    }
}

public class Person
{
    public string Name;
    public int Age;

    public Person(string name, int age)
    {
        this.Name = name;
        this.Age = age;
    }

    public virtual void Introduce()
    {
        Console.WriteLine($"Привет! Моё имя {this.Name}");
    }


}
