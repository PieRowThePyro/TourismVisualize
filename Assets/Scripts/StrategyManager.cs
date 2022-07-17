using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        DoAlgorithmEvent?.Invoke(this, new SolutionInfo(s.cal_fitness()));
    } 
    public static event EventHandler<SolutionInfo> DoAlgorithmEvent;
}
public class SolutionInfo : EventArgs
    {
        public float fitness;

        public SolutionInfo(float fitness)
        {
            this.fitness = fitness;
        }
    }
