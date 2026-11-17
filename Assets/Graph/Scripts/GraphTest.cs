using System.Collections.Generic;
using UnityEngine;

public class GraphTest : MonoBehaviour
{

    public enum Algorithm
    {
        DFS,
        BFS,
        DFSRecursive,
        PathFindingBFS,
        Dijkstra,
        AStar,
    }
    public UIGraphNode nodePrefab;
    public List<UIGraphNode> uiNodes;
    public Transform uiNodeRoot;
    private Graph graph;
    public Algorithm algorithm;
    public int startIndex;
    public int endIndex;

    private void Start()
    {
        int[,] map = new int[5, 5]
        {
        {1,-1,6,1,1},
        {2,-1,3,1,7},
        {1,-1,8,5,2},
        {1,-1,7,3,1},
        {1,1,5,3,1},
        };
        graph = new Graph();
        graph.Init(map);
        InitUiNide(graph);
    }

    private void InitUiNide(Graph graph)
    {
        foreach (var node in graph.nodes)
        {
            var uiNode = Instantiate(nodePrefab, uiNodeRoot);
            uiNode.SetNode(node);
            uiNode.Reset();

            uiNodes.Add(uiNode);
        }
    }
    private void ResetUiNodes()
    {
        foreach (var uiNode in uiNodes)
        {
            uiNode.Reset();
        }
    }
    [ContextMenu("Search")]
    public void Search()
    {
        if (graph == null)
        {
            return;
        }
        var search = new GraphSearch();
        search.Init(graph);

        switch (algorithm)
        {
            case Algorithm.DFS:
                search.DFS(graph.nodes[startIndex]);
                break;
            case Algorithm.BFS:
                search.BFS(graph.nodes[startIndex]);
                break;
            case Algorithm.DFSRecursive:
                search.DFSRecursive(graph.nodes[startIndex]);
                break;
            case Algorithm.PathFindingBFS:
                search.PathFindingBFS(graph.nodes[startIndex], graph.nodes[endIndex]);
                break;
            case Algorithm.Dijkstra:
                search.Dijkstra(graph.nodes[startIndex], graph.nodes[endIndex]);
                break;
            case Algorithm.AStar:
                search.AStar(graph.nodes[startIndex], graph.nodes[endIndex]);
                break;
        }
        ResetUiNodes();

        for (int i = 0; i < search.path.Count; ++i)
        {
            var node = search.path[i];
            float t = search.path.Count > 1 ? (float)i / (search.path.Count - 1) : 0f;
            var color = Color.Lerp(Color.red, Color.green, t);
            uiNodes[node.id].SetColor(color);
            uiNodes[node.id].SetText($"ID: {node.id}\nWeight: {node.weight}\n방문: {i}");
        }
    }
}

