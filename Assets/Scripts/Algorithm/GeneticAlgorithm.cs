using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm : IStrategy
{
    int populationCount;
    float selectionRate;
    float crossoverRate;
    float mutationRate;
    Data data;
    List<Solution> population;
    public static List<Solution> bestSolutions;
    public GeneticAlgorithm(Data data, int populationCount, float selectionRate , float crossoverRate, float mutationRate ) {
        this.data = data;
        this.populationCount = populationCount;
        population = new List<Solution>();
        for (int i = 0; i <populationCount; i++) {
            population.Add(GenerateSolution(data));
;        }
        this.selectionRate = selectionRate;
        this.crossoverRate = crossoverRate;
        this.mutationRate = mutationRate;
        bestSolutions = new List<Solution>();
    }

    public Solution Evolve() {
        List<Solution> next_population = new List<Solution>();
        int selectionSize =Mathf.FloorToInt(selectionRate * populationCount);
        
        for (int i = 0; i < selectionSize; i++) {
            next_population.Add(population[i]);
        }
        int crossoverSize = populationCount - selectionSize;
        for (int i = 0;i< crossoverSize; i++) {
            int mom=Random.Range(0, populationCount);
            next_population.Add(Crossover(population[mom], data));
        }
        population.Sort((x, y) => x.cal_fitness().CompareTo(y.cal_fitness()));
        int MutationSize = Mathf.FloorToInt(mutationRate * populationCount);
        for (int i = 0; i < MutationSize; i++) {
            int ran = Random.Range(Mathf.FloorToInt(selectionSize / 2), populationCount);
            next_population[ran]=Mutation(next_population[ran],data);
        }
        
        population = new List<Solution>(next_population);
        population.Sort((x, y) => x.cal_fitness().CompareTo(y.cal_fitness()));
        Debug.Log(population[0].cal_fitness());
        return population[0];
    }
    public Solution GenerateSolution(Data data)
    {
        List<int> fullTrip = Enumerable.Range(0, data.P).ToList();
        fullTrip.Shuffle();
        Solution s = new Solution(data);
        for (int i = 0; i < data.K; i++)
        {
            List<int> dayTrip = new List<int>();
            float time = Mathf.Max(data.t_s[i], data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration;
            float cost = data.POI[fullTrip[0]].Cost;
            dayTrip.Add(fullTrip[0]);
            int current = fullTrip[0];
            fullTrip.RemoveAt(0);

            while (true)
            {
                if (fullTrip.Count <= 0)
                {
                    break;
                }
                float predict = Mathf.Max(time + data.D[current, fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration;
                if (Mathf.Max(time + data.D[current, fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration < data.t_e[i]
                        && cost + data.POI[fullTrip[0]].Cost < data.C_max[i])
                {
                    time = Mathf.Max(time + data.D[current, fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration;
                    cost += data.POI[fullTrip[0]].Cost;
                    current = fullTrip[0];
                    dayTrip.Add(fullTrip[0]);
                    fullTrip.RemoveAt(0);
                    continue;
                }
                else
                {
                    break;
                }
            }
            s.gene.Add(dayTrip);
        }
        return s;

    }

    public Solution Crossover(Solution s, Data data)
    {
        List<int> fullTrip = new List<int>();
        for (int i = 1; i < data.P; i++)
        {
            fullTrip.Add(i);
        }

        // random trip number and cutoff point
        int tripNumber = Random.Range(0, data.K);
        Debug.Log(tripNumber);
        int cutoffPoint = Random.Range(0, s.gene[tripNumber].Count);
        Solution newS = new Solution(data);
        for (int i = 0; i < tripNumber; i++)
        {
            if (tripNumber != 0)
            {
                foreach (int poi in s.gene[i])
                {
                    fullTrip.Remove(poi);
                }
                newS.gene.Add(s.gene[i]);
            }
        }
        List<int> newTrip = new List<int>();
        float time = Mathf.Max(data.t_s[tripNumber], data.POI[s.gene[tripNumber][0]].Start) + data.POI[s.gene[tripNumber][0]].Duration; 
        float cost = data.POI[s.gene[tripNumber][0]].Cost;
        for (int i = 0; i < cutoffPoint; i++)
        {
            int currentPOI = s.gene[tripNumber][i];
            newTrip.Add(currentPOI);
            fullTrip.Remove(currentPOI);
            time = Mathf.Max(time + data.D[currentPOI,s.gene[tripNumber][i + 1]] * 90, data.POI[s.gene[tripNumber][i + 1]].Start) + data.POI[s.gene[tripNumber][i + 1]].Duration;
            cost += data.POI[s.gene[tripNumber][i]].Cost;
        }

        int current = s.gene[tripNumber][cutoffPoint];
        newTrip.Add(current);
        fullTrip.Remove(current);

        fullTrip.Shuffle();
        
        while (fullTrip.Count > 0)
        {
            if (Mathf.Max(time + data.D[current,fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration < data.t_e[tripNumber]
                    && cost + data.POI[fullTrip[0]].Cost < data.C_max[tripNumber])
            {
                time = Mathf.Max(time + data.D[current,fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration;
                cost += data.POI[fullTrip[0]].Cost;
                current = fullTrip[0];
                newTrip.Add(fullTrip[0]);
                fullTrip.RemoveAt(0);
                continue;
            }
            else
            {
                break;
            }
        }

        newS.gene.Add(newTrip);

        for (int i = tripNumber + 1; i < data.K; i++)
        {
            List<int> dayTrip = new List<int>();
            float newTime = Mathf.Max(data.t_s[i], data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration;
            float newCost = data.POI[fullTrip[0]].Cost;
            dayTrip.Add(fullTrip[0]);
            int currentPOI = fullTrip[0];
            fullTrip.RemoveAt(0);
            while (fullTrip.Count > 0)
            {
                //float predict = Double.max(newTime + data.D[currentPOI][fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration;
                if (Mathf.Max(newTime + data.D[currentPOI,fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration < data.t_e[i]
                        && newCost + data.POI[fullTrip[0]].Cost < data.C_max[i])
                {
                    newTime = Mathf.Max(newTime + data.D[currentPOI,fullTrip[0]] * 90, data.POI[fullTrip[0]].Start) + data.POI[fullTrip[0]].Duration;
                    newCost += data.POI[fullTrip[0]].Cost;
                    currentPOI = fullTrip[0];
                    dayTrip.Add(fullTrip[0]);
                    fullTrip.RemoveAt(0);
                    continue;
                }
                else
                {
                    break;
                }
            }
            newS.gene.Add(dayTrip);
        }
        return newS;
    }

    public Solution Mutation (Solution s, Data data)
    {
        return GenerateSolution(data);
    }

}

public static class Extensions
{
    public static void Shuffle<T>(this IList<T> values)
    {
        for (int i = values.Count - 1; i > 0; i--)
        {
            int k = Random.Range(0, i + 1);
            T value = values[k];
            values[k] = values[i];
            values[i] = value;
        }
    }
}
