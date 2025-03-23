// w metodzie Rate klasy Object zakladam ze beda tylko 2 parametry

//strategia gdzie blizej tam biore do dekodowania

using System.Text;
using System.Collections.Generic;
using Microsoft.VisualBasic;

class Object
{

    public string Parameter { get; set; }
    public double Rating { get; set; }
    private static readonly Random random = new();

    public Object(int LBnP)
    {
        StringBuilder sb = new(LBnP);
        for (int i = 0; i < LBnP; i++)
        {
            sb.Append(random.Next(2));
        }
        Parameter = sb.ToString();
    }

    public void Rate(Dictionary<string, double>bytesStringsValues, int LBnP)
    {
        // ocenianie, tutaj zakladam ze beda tylko 2 parametry
        string part1 = Parameter.Substring(0, LBnP);
        string part2 = Parameter.Substring(LBnP, LBnP);
        double value = bytesStringsValues[part1] + bytesStringsValues[part2];
        Rating = value;
    }

    public void Mutate()
    {
        //mutacja
        int randomIndex = random.Next(Parameter.Length);
        char[] charList = Parameter.ToCharArray();
        if (charList[randomIndex] == '0')
            charList[randomIndex] = '1';
        else 
            charList[randomIndex] = '0';
        Parameter = new string(charList);
    }
}


class Program
{
    static int LBnP = 3;
    static int numberOfParameters = 2;
    static int zdmin = -1;
    static int zdmax = 1;
    static int numberOfObjects = 11;
    static float tournamentSize = 0.2f;

    static List<string> GenerateBytesStrings(int n)
    {
        int total = (int)Math.Pow(2, n);
        List<string> bytesStrings = [];

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

    public static List<Object> CreateObjects()
    {
        List<Object> objects = [];
        for (int i=0; i<numberOfObjects; i++)
        {
            Object specimen = new(LBnP * numberOfParameters);
            objects.Add(specimen);
        }
        return objects;
    }

    public static Object TournamentSelection(List<Object> objects)
    {
        int numberOfSelectedObjects = (int)(numberOfObjects * tournamentSize);
        List<Object> selectedObjects = [];
        Random random = new();
        for (int i=0; i<numberOfSelectedObjects; i++)
        {
            selectedObjects.Add(objects[random.Next(objects.Count)]);
        }
        double bestRating = selectedObjects[0].Rating;
        Object bestObject = selectedObjects[0];
        foreach (Object item in selectedObjects)
        {
            if (item.Rating > bestRating)
            {
                bestObject = item;
            }
        } 
        return bestObject;
    }

    static void Main()
    {
        // Console.Write("Podaj liczbe bitow na parametr: ");
        // Program.LBnP = int.Parse(Console.ReadLine());

        // Console.Write("Podaj ZDmin: ");
        // Program.zdmin = int.Parse(Console.ReadLine());

        // Console.Write("Podaj ZDmax: ");
        // Program.zdmax = int.Parse(Console.ReadLine());

        List<string> bytesStrings = GenerateBytesStrings(LBnP);
        var bytesStringsValues = GenerateValues(bytesStrings, zdmax, zdmin);

        List<Object> objects = CreateObjects();
        List<Object> selectedObjects = [];
        foreach (Object item in objects)
        {
            item.Rate(bytesStringsValues, LBnP);
        }
        for (int i=0; i < numberOfObjects-1;i++)
        {
            selectedObjects.Add(TournamentSelection(objects));
        }
        foreach (Object item in selectedObjects)
        {
            Console.WriteLine(item.Parameter);
            Console.WriteLine(item.Rating);
        }


        
        

        // Console.WriteLine("\nGenerated mappings:");
        // foreach (var pair in bytesStringsValues)
        // {
        //     Console.WriteLine($"{pair.Key} -> {pair.Value}");
        // }
    }
}