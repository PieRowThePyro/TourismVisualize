using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution
{
    public List<List<int>> gene; //{[1,2,3],[4,5,6]}
    private Data data;

    public Solution(Data data)
    {
        this.data = data;
        gene = new List<List<int>>();
    }

    public float cal_hapiness_obj()
    {
        float happiness = 0;
        foreach (List<int> arrayList in gene)
        {
            foreach (int index in arrayList)
            {
                happiness += data.tourist[index,data.C];
            }
        }
        return happiness;
    }

    public float cal_distance_obj()
    {
        float distance = 0;
        for (int i = 0; i < data.K; i++)
        {
            for (int j = 0; j < gene[i].Count - 1; j++)
            {
                distance += data.D[gene[i][j],gene[i][j+1]];
            }
        }
        return distance;
    }

    public float cal_number_of_destination_obj()
    {
        float number = 0;
        foreach (List<int> arrayList in gene)
        {
            number += arrayList.Count;
        }
        return number;
    }

    public float cal_waiting_time_obj()
    {
        float waiting_time = 0;
        for (int i = 0; i < data.K; i++)
        {
            float current_time = data.t_s[i] + data.POI[gene[i][0]].Duration;
            for (int j = 1; j < gene[i].Count; j++)
            {
                if (current_time + data.D[gene[i][j - 1],gene[i][j]] * 90 < data.POI[gene[i][j]].Start)
                {
                    waiting_time += data.POI[gene[i][j]].Start - current_time + data.D[gene[i][j - 1], gene[i][j]] * 90;

                }
                current_time = Mathf.Max(current_time + data.D[gene[i][j - 1], gene[i][j]] * 90, data.POI[gene[i][j]].Start) + data.POI[gene[i][j]].Duration;
            }
        }
        return waiting_time;
    }
    public float cal_cost_obj()
    {
        float cost = 0;
        for (int i = 0; i < data.K; i++)
        {
            for (int j = 0; j < gene[i].Count - 1; j++)
            {
                cost += data.POI[gene[i][j]].Cost
                        + data.S * data.D[gene[i][j], gene[i][j + 1]];
            }
            cost += data.POI[gene[i].Count - 1].Cost;
        }
        return cost;
    }

    public float cal_fitness()
    {
        float fitness = 0;
        fitness += Mathf.Pow((cal_distance_obj() - Data.MIN_DISTANCE) / (Data.MAX_DISTANCE - Data.MIN_DISTANCE), 2) * data.w1;
       /* fitness += Mathf.Pow((Data.MIN_WATING_TIME - cal_waiting_time_obj()) / (Data.MAX_WATING_TIME - Data.MIN_WATING_TIME), 2) * data.w2;
        fitness += Mathf.Pow((cal_hapiness_obj() - Data.MAX_HAPPINESS) / (Data.MAX_HAPPINESS - Data.MIN_HAPPINESS), 2) * data.w3;
        fitness += Mathf.Pow((cal_number_of_destination_obj() - Data.MAX_NUMBER_OF_DESTINATION) / (Data.MAX_NUMBER_OF_DESTINATION - Data.MIN_NUMBER_OF_DESTINATION), 2) * data.w4;
        fitness += Mathf.Pow((cal_cost_obj() - Data.MIN_BUDGET) / (Data.MAX_BUDGET - Data.MIN_BUDGET), 2) * data.w5;*/
        fitness = Mathf.Sqrt(fitness);
        return fitness;
    }

    public bool Equals(object obj,Data data)
    {
        if (obj is Solution) {
            var s = obj as Solution;
            for (int i = 0; i < data.K; i++) {
                if (CompareList.SetwiseEquivalentTo<int>(s.gene[i], this.gene[i]) == false) {
                    return false;
                } ;
            }
            return true;
        }
        return false;
    }
}
