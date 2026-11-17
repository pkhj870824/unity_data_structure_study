using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class GraphSearch
{
    private Graph graph;
    public List<GraphNode> path = new List<GraphNode>();

    public void Init(Graph graph)
    {
        this.graph = graph;
    }

    public void DFS(GraphNode node)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var stack = new Stack<GraphNode>();
        var visited = new HashSet<GraphNode>();

        stack.Push(node);
        visited.Add(node);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            path.Add(current);

            foreach (var adjacent in current.adjacents)
            {
                if (!adjacent.Canvisit || visited.Contains(adjacent))
                {
                    continue;
                }
                stack.Push(adjacent);
                visited.Add(adjacent);
            }
        }
    }
    public void BFS(GraphNode node)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var queue = new Queue<GraphNode>();
        var visited = new HashSet<GraphNode>();
        queue.Enqueue(node);
        visited.Add(node);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            path.Add(current);

            foreach (var adjacent in current.adjacents)
            {
                if (!adjacent.Canvisit || visited.Contains(adjacent))
                {
                    continue;
                }
                queue.Enqueue(adjacent);
                visited.Add(adjacent);
            }
        }
    }

    public void DFSRecursive(GraphNode node)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var visited = new HashSet<GraphNode>();
        DFSRecursive(node, visited);
    }
    private void DFSRecursive(GraphNode node, HashSet<GraphNode> visited)
    {
        visited.Add(node);
        path.Add(node);
        foreach (var adjacent in node.adjacents)
        {
            if (!adjacent.Canvisit || visited.Contains(adjacent))
            {
                continue;
            }
            DFSRecursive(adjacent, visited);
        }
    }

    public bool PathFindingBFS(GraphNode startNode, GraphNode endNode)
    {
        path.Clear();
        graph.ResetNodePrevious();

        var queue = new Queue<GraphNode>();
        var visited = new HashSet<GraphNode>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        bool success = false;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current == endNode)
            {
                success = true;
                break;
            }
            foreach (var adjacent in current.adjacents)
            {
                if (!adjacent.Canvisit || visited.Contains(adjacent))
                {
                    continue;
                }

                adjacent.previous = current;

                visited.Add(adjacent);
                queue.Enqueue(adjacent);
            }
        }
        if (!success)
        {
            return false;
        }

        GraphNode step = endNode;
        while (step != null)
        {
            path.Add(step);
            step = step.previous;
        }
        path.Reverse();
        return true;
    }
}
