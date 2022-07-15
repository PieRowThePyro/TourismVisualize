using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntColonyAgorithm
{
    Data data;
    double[,] CostMatrix;
    double[,] PheromoneMatrix;
    double[,] TemporaryMatrix;
    double beta;
    int numberOfAnts;
    
    double alpha;
    List<Solution> results;

    public AntColonyAgorithm(Data data, double alpha, double beta, int numberOfAnts)
    {
        this.data = data;
        this.results = new List<Solution>();
        this.beta = beta;
        this.numberOfAnts = numberOfAnts;       
        this.alpha = alpha;
        CostMatrix = new double[data.P, data.P];
        TemporaryMatrix = new double[data.P, data.P];
        PheromoneMatrix = new double[data.P, data.P];

        for (int i = 0; i < data.P; i++)
        {
            for (int j = 0; j < data.P; j++)
            {
                if (i != j)
                {
                    CostMatrix[i, j] = data.D[i, j];
                }
                else
                {
                    CostMatrix[i, j] = 0;
                }
                if (CostMatrix[i, j] == 0)
                {
                    PheromoneMatrix[i, j] = 0;
                    TemporaryMatrix[i, j] = 0;
                }
                else
                {
                    PheromoneMatrix[i, j] = 1;
                    TemporaryMatrix[i, j] = 1;

                    /// costMatrix[i][j];
                }

            }
        }


    }

    public void Evolve()
    {
        List<Solution> ants = new List<Solution>();
        for (int i = 0; i < numberOfAnts; i++)
        {
            Solution ant = new Solution(data);
            for (int j = 0; j < data.K; j++)
            {

                List<int> chosen = new List<int>();
                int startLocation = Random.Range(0, data.P);
                double budget = data.POI[startLocation].Cost;
                double currentTime = 27000;
                int currentLocation = startLocation;
                List<int> oneTrip = new List<int>();
                while (budget < data.C_max[j] || currentTime < data.T_max[j])
                {

                    List<int> canVisited = new List<int>();
                    for (int k = 0; k < data.P; k++)
                    {
                        double timePrediction;

                        timePrediction = currentTime + data.POI[k].Duration + data.D[k, currentLocation] * 90;

                        if (timePrediction <= data.T_max[j] && chosen.IndexOf(k) < 0)
                        {

                            if (budget + data.S * data.D[currentLocation, k] + data.POI[k].Cost < data.C_max[j])
                            {
                                canVisited.Add(k);
                            }


                        }
                    }

                    if (canVisited.Count == 0)
                    {
                        break;
                    }
                    DistributedRandomNumberGenerator drng = new DistributedRandomNumberGenerator();
                    foreach (int integer in canVisited)
                    {
                        drng.addNumber(integer, PheromoneMatrix[currentLocation, integer] * 1 / CostMatrix[currentLocation, integer]);
                    }
                    int random = drng.getDistributedRandomNumber();
                    oneTrip.Add(random);
                    budget += data.S * data.D[currentLocation, random] + data.POI[random].Cost;
                    currentTime += data.D[currentLocation, random] * 90 + data.POI[random].Duration;
                    currentLocation = random;
                    chosen.Add(random);
                }

                ant.gene.Add(oneTrip);
            }
            ants.Add(ant);
        }
        ants.Sort((x, y) => x.cal_fitness().CompareTo(y.cal_fitness()));
        foreach (Solution ant in ants)
        {
            
            double cost = 0;
            for (int j = 0; j < data.K; j++)
            {
                for (int k = 0; k < ant.gene[j].Count - 1; k++)
                {
                    cost += CostMatrix[ant.gene[j][k],ant.gene[j][k + 1]];

                }
            }

            for (int j = 0; j < data.K; j++)
            {
                for (int k = 0; k < ant.gene[j].Count - 1; k++)
                {

                    TemporaryMatrix[ant.gene[j][k],ant.gene[j][k + 1]]
                            += 1 / cost;//+ 0.4*Algorithm.pheromoneMatrix[ant.gene.[j].get(k)][ant.gene.[j].get(k + 1)];

                }
            }
        }
        for (int i = 0; i < data.P; i++)
        {
            for (int j = 0; j < data.P; j++)
            {
                PheromoneMatrix[i,j] = alpha * PheromoneMatrix[i,j] + beta*TemporaryMatrix[i,j];
                TemporaryMatrix[i,j] = 0;
            }
        }
        if (results.Count == 0)
        {
            results.Add(ants[0]);
        }
        else {
            if (results[results.Count - 1].cal_fitness() < ants[0].cal_fitness())
            {
                results.Add(results[results.Count - 1]);
            }
            else {
                results.Add(ants[0]);
            }
        }

        Debug.Log(results[results.Count - 1].cal_fitness());

    }
}
