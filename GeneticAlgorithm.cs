using System;
using System.Collections.Generic;

class Program
{
    static List<string> GeneratePopulation(int n)
    {
        int total = (int)Math.Pow(2, n);
        List<string> population = new List<string>();

        for (int i = 0; i < total; i++)
        {
            population.Add(Convert.ToString(i, 2).PadLeft(n, '0'));
        }

        return population;
    }

    static void Main()
    {
        Console.Write("Podaj liczbe bitow: ");
        int n = int.Parse(Console.ReadLine());

        List<string> population = GeneratePopulation(n);

        Console.Write("Podaj ZDmin ");
        int zdmin = int.Parse(Console.ReadLine());

        Console.Write("Podaj ZDmax");
        int zdmax = int.Parse(Console.ReadLine());

        Console.WriteLine("Generated bit strings:");
        foreach (var item in population)
        {
            Console.WriteLine(item);
        }
    }
}