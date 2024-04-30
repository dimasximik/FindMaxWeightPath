using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PathFinderExample : MonoBehaviour
{
    class PathResult
    {
        public List<NodeBase> Path = new List<NodeBase>();
        public int TotalWeight = 0;
    }

    private void Start()
    {
        TestPathFinding();
    }

    private void TestPathFinding()
    {
        List<long> recursionTimes = new List<long>();
        List<int> recursionWeights = new List<int>();
        List<long> BFSTimes = new List<long>();
        List<int> BFSWeights = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            var recursionResult = StartPathFinding("recursion");
            recursionTimes.Add(recursionResult.Item1);
            recursionWeights.Add(recursionResult.Item2);
            GC.Collect();

            var BFSResult = StartPathFinding("BFS");
            BFSTimes.Add(BFSResult.Item1);
            BFSWeights.Add(BFSResult.Item2);
            GC.Collect();
        }

        Debug.Log($"Recursion: Average Time = {recursionTimes.Average()}ms, Average Max Weight = {recursionWeights.Average()}");
        Debug.Log($"BFS: Average Time = {BFSTimes.Average()}ms, Average Max Weight = {BFSWeights.Average()}");
    }

    private Tuple<long, int> StartPathFinding(string method)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var nodes = NodeGenerator.GenerateNodes(100000, 3);
        var startNode = nodes.First();

        stopwatch.Restart();
        var result = method == "recursion" ? FindHeaviestPath(startNode, new List<NodeBase>(), 0) : FindHeaviestPathBFSStack(startNode);

        stopwatch.Stop();
        return new Tuple<long, int>(stopwatch.ElapsedMilliseconds, result.TotalWeight);
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

        foreach (var nextNode in node.Nodes)
        {
            var result = FindHeaviestPath(nextNode, currentPath, currentWeight);
            if (result.TotalWeight > maxResult.TotalWeight)
            {
                maxResult = result;
            }
        }

        currentPath.RemoveAt(currentPath.Count - 1);

        return maxResult;
    }

    private PathResult FindHeaviestPathBFSStack(NodeBase startNode)
    {
        if (startNode == null)
            return new PathResult { Path = new List<NodeBase>(), TotalWeight = 0 };

        Stack<(NodeBase Node, List<NodeBase> Path, int TotalWeight)> stack = new Stack<(NodeBase, List<NodeBase>, int)>();
        HashSet<NodeBase> visited = new HashSet<NodeBase>();
        PathResult maxResult = new PathResult { Path = new List<NodeBase>(), TotalWeight = 0 };

        stack.Push((startNode, new List<NodeBase> { startNode }, startNode.Weight));
        visited.Add(startNode);

        while (stack.Count > 0)
        {
            var (currentNode, currentPath, currentWeight) = stack.Pop();

            if (currentWeight > maxResult.TotalWeight)
            {
                maxResult.TotalWeight = currentWeight;
                maxResult.Path = new List<NodeBase>(currentPath);
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

        return maxResult;
    }
}
