using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BFSExample : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.LogError("BFS started");
            StartBFS();
        }
    }

    void StartBFS()
    {
        GC.Collect();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var nodes = NodeGenerator.GenerateNodes(100000, 3);
        var startNode = nodes.First();
        Debug.Log($"BFS Nodes generated: {nodes.Count} nodes in {stopwatch.ElapsedMilliseconds}ms");

        stopwatch.Restart();
        var (resultPath, resultWeight) = FindHeaviestPathBFSStack(startNode);

        Debug.Log($"BFS Path found in {stopwatch.ElapsedMilliseconds}ms");
        
        stopwatch.Stop();
        Debug.Log($"Max Weight: {resultWeight}");
        
        var pathWeights = string.Join(" ", resultPath.Select(node => node.Weight));
        Debug.Log("Max Path: " + pathWeights);
    }

    private (List<NodeBase> Path, int TotalWeight) FindHeaviestPathBFSStack(NodeBase startNode)
    {
        if (startNode == null)
            return (new List<NodeBase>(), 0);

        Stack<(NodeBase Node, List<NodeBase> Path, int TotalWeight)> stack = new Stack<(NodeBase, List<NodeBase>, int)>();
        HashSet<NodeBase> visited = new HashSet<NodeBase>();
        (List<NodeBase> maxPath, int maxWeight) = (new List<NodeBase>(), 0);

        stack.Push((startNode, new List<NodeBase> { startNode }, startNode.Weight));
        visited.Add(startNode);

        while (stack.Count > 0)
        {
            var (currentNode, currentPath, currentWeight) = stack.Pop();

            if (currentWeight > maxWeight)
            {
                maxWeight = currentWeight;
                maxPath = new List<NodeBase>(currentPath);
            }

            foreach (var nextNode in currentNode.Nodes)
            {
                if (visited.Add(nextNode))
                {
                    var newPath = new List<NodeBase>(currentPath) { nextNode };
                    var newWeight = currentWeight + nextNode.Weight;
                    stack.Push((nextNode, newPath, newWeight));
                }
            }
        }

        return (maxPath, maxWeight);
    }
}
