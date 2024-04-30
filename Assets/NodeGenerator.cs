using System.Collections.Generic;

public class NodeBase
{
    public List<NodeBase> Nodes = new List<NodeBase>();
    public int Weight;
}


public static class NodeGenerator
{
    public static List<NodeBase> GenerateNodes(int maxNodes, int childrenPerNode)
    {
        var startNode = new NodeBase() { Weight = 1 };
        var nodes = new List<NodeBase> { startNode };
        int currentWeight = 2;

        var currentNodes = new List<NodeBase> { startNode };

        while (nodes.Count < maxNodes)
        {
            var newNodes = new List<NodeBase>();
            foreach (var node in currentNodes)
            {
                for (int i = 0; i < childrenPerNode && nodes.Count < maxNodes; i++)
                {
                    var newNode = new NodeBase() { Weight = currentWeight++ };
                    node.Nodes.Add(newNode);
                    newNodes.Add(newNode);
                    nodes.Add(newNode);
                }
            }
            currentNodes = newNodes;
        }

        return nodes;
    }
}