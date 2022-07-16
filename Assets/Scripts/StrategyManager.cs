using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyManager
{
    private IStrategy _strategy;
    public static List<Solution> BestSolutions; 

    public StrategyManager()
    {
        BestSolutions = new List<Solution>();

    }
    
    public StrategyManager(IStrategy strategy)
    {
        this._strategy = strategy;
        BestSolutions.Clear();

    }

    public void SetStrategy(IStrategy strategy)
    {
        this._strategy = strategy;
        BestSolutions.Clear();
    }

    public void DoAlgorithm()
    {
        Solution s = _strategy.Evolve();
        BestSolutions.Add(s);
        Debug.Log(s.cal_fitness());
    } 
}
