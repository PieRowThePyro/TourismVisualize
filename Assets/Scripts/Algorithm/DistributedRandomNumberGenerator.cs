
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributedRandomNumberGenerator {

    private Dictionary<int, double> distribution;
    private double distSum;

    public static int getRandomNumber(int min, int max)
    {
        return Random.Range(min,max);
    }

    public DistributedRandomNumberGenerator()
    {
        distribution = new Dictionary<int, double>();
    }

    public void addNumber(int value, double distribution)
    {
        if (this.distribution.ContainsKey(value)==true)
        {
            distSum -= this.distribution[value];
        }
        this.distribution.Add(value, distribution);
        distSum += distribution;
    }

    public int getDistributedRandomNumber()
    {
        System.Random random = new System.Random();
        double Rand = random.NextDouble();
        double ratio = 1.0f / distSum;
        double tempDist = 0;
        foreach (int i in distribution.Keys)
        {
            tempDist += distribution[i];
            if (Rand / ratio <= tempDist)
            {
                return i;
            }
        }
        return 0;
    }
}
