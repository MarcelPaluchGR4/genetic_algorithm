//strategia gdzie blizej tam biore do dekodowania

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

    static Dictionary<string, double> GenerateValues(List<string> population, int zdmax, int zdmin)
    {
        int range = zdmax - zdmin;
        double step = (double)range / (population.Count-1);
        Dictionary<string, double> mapping = new Dictionary<string, double>();
        for (int i = 0; i < population.Count ; i++)
        {
            mapping[population[i]] = zdmin + step*i;
        }
        mapping[population[population.Count-1]] = zdmax; 
        return mapping;
    }

    static void Main()
    {
        Console.Write("Podaj liczbe bitow: ");
        int n = int.Parse(Console.ReadLine());

        Console.Write("Podaj ZDmin ");
        int zdmin = int.Parse(Console.ReadLine());

        Console.Write("Podaj ZDmax");
        int zdmax = int.Parse(Console.ReadLine());

        List<string> population = GeneratePopulation(n);

        var populationMapping = GenerateValues(population, zdmax, zdmin);

        Console.WriteLine("\nGenerated mappings:");
        foreach (var pair in populationMapping)
        {
            Console.WriteLine($"{pair.Key} -> {pair.Value}");
        }
    }
}