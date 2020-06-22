using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShortestPath : MonoBehaviour
{
    private GameObject[] nodes;

    // ScreenData
    public string algorithmName;
    public int checkedNodesCount;
    public double timeSpent;

    /// <summary>
    /// Finding the shortest path and return in a List
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>A List of transform for the shortest path</returns>
    public List<Transform> findShortestPath(Transform start, Transform end, string algorithmName)
    {
        this.checkedNodesCount = 0;

        double startTime = Time.realtimeSinceStartup;

        nodes = GameObject.FindGameObjectsWithTag("node");

        List<Transform> result = new List<Transform>();

        Transform node = null;

        if (algorithmName.Equals("BFS"))
            node = BFS_Search(start, end.name);

        if (algorithmName.Equals("Dijkstra"))
            node = DijkstrasAlgo(start, end);

        if (algorithmName.Equals("DFS"))
        {
            node = DepthFirstTraversal(start, end);
        }

        while (node != null)
        {
            result.Add(node);
            Node currentNode = node.GetComponent<Node>();
            node = currentNode.getParentNode();
        }

        // Reverse the list so that it will be from start to end.
        result.Reverse();

        double endTime = (Time.realtimeSinceStartup - startTime);

        this.timeSpent = endTime;

        //print(result);

        return result;
    }

    private void print(List<Transform> result)
    {
        foreach (Transform t in result)
            print(t.name);
    }

    /// <summary>
    /// Dijkstra Algorithm to find the shortest path
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>The end node</returns>
    private Transform DijkstrasAlgo(Transform start, Transform end)
    {
        // Nodes that are unexplored
        List<Transform> unexplored = new List<Transform>();

        // We add all the nodes we found into unexplored.
        foreach (GameObject obj in nodes)
        {
            Node n = obj.GetComponent<Node>();
            if (n.isWalkable())
            {
                n.resetNode();
                unexplored.Add(obj.transform);
            }
        }

        this.checkedNodesCount += 1;
        // Set the starting node weight to 0;
        Node startNode = start.GetComponent<Node>();
        startNode.setWeight(0);

        while (unexplored.Count > 0)
        {
            this.checkedNodesCount += 1;
            // Sort the explored by their weight in ascending order.
            unexplored.Sort((x, y) => x.GetComponent<Node>().getWeight().CompareTo(y.GetComponent<Node>().getWeight()));

            // Get the lowest weight in unexplored.
            Transform current = unexplored[0];

            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.getNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
                this.checkedNodesCount += 1;
                Node node = neighNode.GetComponent<Node>();

                // We want to avoid those that had been explored and is not walkable.
                if (unexplored.Contains(neighNode) && node.isWalkable())
                {
                    // Get the distance of the object.
                    float distance = Vector3.Distance(neighNode.position, current.position);
                    distance = currentNode.getWeight() + distance;

                    // If the added distance is less than the current weight.
                    if (distance < node.getWeight())
                    {
                        // We update the new distance as weight and update the new path now.
                        node.setWeight(distance);
                        node.setParentNode(current);
                    }
                }
            }

        }

        return end;
    }

    /// <summary>
    /// Breadth-first search algorithm to find the shortest path
    /// </summary>
    /// <param name="root">The start point</param>
    /// <param name="nameToSearchFor">The end point</param>
    /// <returns>The end node</returns>
    public Transform BFS_Search(Transform root, string nameToSearchFor)
    {
        Queue<Transform> Q = new Queue<Transform>();
        HashSet<Transform> S = new HashSet<Transform>();
        Q.Enqueue(root);
        S.Add(root);

        this.checkedNodesCount += 1;

        while (Q.Count > 0)
        {
            Transform e = Q.Dequeue();
            if (e.GetComponent<Node>().name == nameToSearchFor)
                return e;
            foreach (Transform neighbour in e.GetComponent<Node>().getNeighbourNode())
            {
                this.checkedNodesCount += 1;
                if (!S.Contains(neighbour) && neighbour.GetComponent<Node>().isWalkable() && root != neighbour)
                {
                    Q.Enqueue(neighbour);
                    neighbour.GetComponent<Node>().setParentNode(e);
                    S.Add(neighbour);
                }
            }
        }

        return null;
    }


    public Transform DepthFirstTraversal(Transform start, Transform end)
    {
        HashSet<Transform> visited = new HashSet<Transform>();
        Stack<Transform> stack = new Stack<Transform>();



        stack.Push(start);

        while (stack.Count != 0)
        {
            Transform current = stack.Pop();

            if (current == end)
            {
                this.checkedNodesCount += visited.Count();
                return current;
            }

            if (!visited.Add(current))
                continue;

            var neighbours = current.GetComponent<Node>().getNeighbourNode().Where(n => !visited.Contains(n) && n.GetComponent<Node>().isWalkable() && n != start);

            //foreach (var neighbour in neighbours)
            //{
            //    stack.Push(neighbour);
            //    neighbour.GetComponent<Node>().setParentNode(current);
            //}

            // left-to-right order
            foreach (var neighbour in neighbours.Reverse())
            {
                stack.Push(neighbour);
                neighbour.GetComponent<Node>().setParentNode(current);
            }

        }

        return null;
    }

    /// <summary>
    /// Depth-first search algorithm to find the shortest path
    /// </summary>
    /// <param name="root">The start point</param>
    /// <param name="nameToSearchFor">The end point</param>
    /// <returns>The end node</returns>
    public Transform DFS_Search(Transform root, string nameToSearchFor)
    {
        if (nameToSearchFor == root.name)
            return root;

        Transform personFound = null;

        foreach (Transform child in root.GetComponent<Node>().getNeighbourNode())
        {
            if (child.GetComponent<Node>().isWalkable())// && root != child)
            {
                personFound = DFS_Search(child, nameToSearchFor);
                if (personFound != null)
                {
                    child.GetComponent<Node>().setParentNode(root);
                    break;
                }
            }
        }

        return personFound;
    }

}

