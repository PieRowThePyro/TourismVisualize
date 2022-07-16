using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyManager : MonoBehaviour
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
    }

    public void SetStrategy(IStrategy strategy)
    {
        this._strategy = strategy;
    }

    public void DoAlgorithm()
    {
        _strategy.Evolve();
    } 
}
