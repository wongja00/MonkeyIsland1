using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeightedProbability
{
    public string Item { get; set; }
    public int Weight { get; set; }
    public double Probability { get; set; }

    public WeightedProbability(string item, int weight)
    {
        Item = item;
        Weight = weight;
    }
}

public class WeightedProbabilityCalculator
{
    public List<WeightedProbability> CalculateProbabilities(List<WeightedProbability> items)
    {
        int totalWeight = items.Sum(item => item.Weight);

        foreach (var item in items)
        {
            item.Probability = (double)item.Weight / totalWeight;
        }

        return items;
    }
}
