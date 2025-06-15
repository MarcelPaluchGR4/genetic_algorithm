// w metodzie Rate klasy Object zakladam ze beda tylko 2 parametry

//strategia gdzie blizej tam biore do dekodowania

using System.Text;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;


class Object
{
    static Random random = new Random();

    public string Parameter { get; set; }
    public double Rating { get; set; }


    public Object(int LBnP)
    {
        StringBuilder sb = new(LBnP);
        for (int i = 0; i < LBnP; i++)
        {
            sb.Append(random.Next(2));
        }
        Parameter = sb.ToString();
    }

    public Object(string parameter)
    {
        Parameter = new string(parameter.ToCharArray());
    }

    public Object(Object item)
    {
        Parameter = new string(item.Parameter.ToCharArray());
        Rating = item.Rating;
    }

    public void Rate(Dictionary<string, double> bytesStringsValues, int LBnP)
    {
        // ocenianie, tutaj zakladam ze beda tylko 2 parametry
        string part1 = Parameter.Substring(0, LBnP);
        string part2 = Parameter.Substring(LBnP, LBnP);
        double x1 = bytesStringsValues[part1];
        double x2 = bytesStringsValues[part2];
        Rating = Math.Sin(x1 * 0.05) + Math.Sin(x2 * 0.05) + (0.4 * Math.Sin(x1 * 0.15) * Math.Sin(x2 * 0.15));
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

class Program
{
    static Random random = new Random();
    static int LBnP = 3;
    static int numberOfParameters = 2;
    static int zdmin = 0;
    static int zdmax = 100;
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
            mapping[bytesStrings[i]] = (double)zdmin + step*i;
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

    public static Object TournamentSelection(List<Object> population)
    {
        int tournamentSizeCount = Math.Max(2, (int)(population.Count * tournamentSize));
        Object best = null;
        double bestRating = double.MinValue;

        for (int i = 0; i < tournamentSizeCount; i++)
        {
            Object candidate = population[random.Next(population.Count)];
            if (candidate.Rating > bestRating)
            {
                bestRating = candidate.Rating;
                best = candidate;
            }
        }
        return new Object(best); // zwracamy kopię najlepszego osobnika
    }

    public static Object HotDeckSelection(List<Object> objects)
    {
        double bestRating = objects[0].Rating;
        Object bestObject = objects[0];
        foreach (Object item in objects)
        {
            if (item.Rating > bestRating)
            {
                bestRating = item.Rating;
                bestObject = item;
            }
        } 

        return new Object(bestObject);
    }

    public static double GetMeanObjectValue(List<Object> objects)
    {
        double total = 0;
        foreach (Object item in objects)
        {
            total += item.Rating;
        }
        return total/objects.Count;
    }

    static void Main()
    {

        List<string> bytesStrings = GenerateBytesStrings(LBnP);
        var bytesStringsValues = GenerateValues(bytesStrings, zdmax, zdmin);

        List<Object> objects = CreateObjects();
        List<Object> selectedObjects = [];
        foreach (Object item in objects)
            {
                item.Rate(bytesStringsValues, LBnP);
            }
        for (int j=0; j<2000;j++)
        {   
            for (int i = 0; i < numberOfObjects - 1; i++)
            {
                selectedObjects.Add(TournamentSelection(objects));
            }
            foreach (Object item in selectedObjects)
            {
                item.Mutate();
            }
            selectedObjects.Add(HotDeckSelection(objects));
            foreach (Object item in selectedObjects)
            {
                item.Rate(bytesStringsValues, LBnP);
            }
            Console.WriteLine($"Iteracja {j}, najlepszy {HotDeckSelection(selectedObjects).Rating}, srednia {GetMeanObjectValue(selectedObjects)}");
            objects.Clear();
            selectedObjects.ForEach((item)=>
            {
                objects.Add(new Object(item));
            });
            selectedObjects.Clear();
        }    
    }
}