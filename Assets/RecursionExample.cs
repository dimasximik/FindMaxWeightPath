using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class RecursionExample : MonoBehaviour
{
    class PathResult
    {
        public List<NodeBase> Path = new List<NodeBase>();
        public int TotalWeight = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.LogError("Standard recursion started");
            StartNodeBase();
        }
    }

    private PathResult FindHeaviestPath(NodeBase node, List<NodeBase> currentPath, int currentWeight)
    {
        currentPath.Add(node);
        currentWeight += node.Weight;

        PathResult maxResult = new PathResult
        {
            Path = new List<NodeBase>(currentPath),
            TotalWeight = currentWeight
        };

        foreach (var nextNodeBase in node.Nodes)
        {
            var result = FindHeaviestPath(nextNodeBase, currentPath, currentWeight);
            if (result.TotalWeight > maxResult.TotalWeight)
            {
                maxResult = result;
            }
        }

        currentPath.RemoveAt(currentPath.Count - 1);

        return maxResult;
    }

    void StartNodeBase()
    {
        GC.Collect();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var startNodeBase = NodeGenerator.GenerateNodes(100000, 3).First();
        Debug.Log("NodeBases generated in " + stopwatch.ElapsedMilliseconds + "ms");

        stopwatch.Restart();

        var result = FindHeaviestPath(startNodeBase, new List<NodeBase>(), 0);

        Debug.Log($"Standard Path found in {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Stop();
        Debug.Log($"Max Weight: {result.TotalWeight}");

        var pathWeights = string.Join(" ", result.Path.Select(node => node.Weight));
        Debug.Log("Max Path: " + pathWeights);
    }
}