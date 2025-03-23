//strategia gdzie blizej tam biore do dekodowania

using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

class Object
{

    public string Parameter { get; set; }
    private static readonly Random random = new Random();

    public Object()
    {
        StringBuilder sb = new(6);
        for (int i = 0; i < 6; i++)
        {
            sb.Append(random.Next(2));
        }
        Parameter = sb.ToString();
    }

    public double GetRating(Dictionary<string, double>bitStringValues)
    {
        // ocenianie
        string part1 = Parameter.Substring(0, 3);
        string part2 = Parameter.Substring(3, 3);
        double value = bitStringValues[part1] + bitStringValues[part2];
        return value;

    }

    public void Mutate()
    {
        int randomIndex = random.Next(Parameter.Length);
        char[] charList = Parameter.ToCharArray();
        if (charList[randomIndex] == '0')
            charList[randomIndex] = '1';
        else 
            charList[randomIndex] = '0';
        Parameter = new string(charList);
    }
}


static class Program
{
    static List<string> GenerateBytesStrings(int n)
    {
        int total = (int)Math.Pow(2, n);
        List<string> bytesStrings = new List<string>();

        for (int i = 0; i < total; i++)
        {
            bytesStrings.Add(Convert.ToString(i, 2).PadLeft(n, '0'));
        }

        return bytesStrings;
    }

    static Dictionary<string, double> GenerateValues(List<string> bytesStrings, int zdmax, int zdmin)
    {
        int range = zdmax - zdmin;
        double step = (double)range / (bytesStrings.Count-1);
        Dictionary<string, double> mapping = [];
        for (int i = 0; i < bytesStrings.Count ; i++)
        {
            mapping[bytesStrings[i]] = zdmin + step*i;
        }
        mapping[bytesStrings[bytesStrings.Count-1]] = zdmax; 
        return mapping;
    }

    static void Main()
    {
        // Console.Write("Podaj liczbe bitow: ");
        // int n = int.Parse(Console.ReadLine());

        // Console.Write("Podaj ZDmin: ");
        // int zdmin = int.Parse(Console.ReadLine());

        // Console.Write("Podaj ZDmax: ");
        // int zdmax = int.Parse(Console.ReadLine());

        int n = 3;
        int zdmin = -1;
        int zdmax = 1;

        List<string> bytesStrings = GenerateBytesStrings(n);

        var bitStringValues = GenerateValues(bytesStrings, zdmax, zdmin);
        
        Object specimen = new();
        specimen.GetRating(bitStringValues);
        Console.WriteLine(specimen.Parameter);
        specimen.Mutate();
        Console.WriteLine(specimen.Parameter);

        // Console.WriteLine("\nGenerated mappings:");
        // foreach (var pair in bitStringValues)
        // {
        //     Console.WriteLine($"{pair.Key} -> {pair.Value}");
        // }
    }
}