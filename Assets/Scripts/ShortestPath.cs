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
    public List<Transform> findShortestPath(Transform start, Transform end)
    {
        nodes = GameObject.FindGameObjectsWithTag("node");

        List<Transform> result = new List<Transform>();

        // algoritam  
        Transform node = DijkstrasAlgo(start, end);

        //Transform node = Search(start, end);


        // While there's still previous node, we will continue.
        while (node != null)
        {
            result.Add(node);
            Node currentNode = node.GetComponent<Node>();
            node = currentNode.getParentNode();
        }

        // Reverse the list so that it will be from start to end.
        result.Reverse();
        return result;
    }

    //public List<Transform> findShortestPath(Transform start, Transform end)
    //{
    //    nodes = GameObject.FindGameObjectsWithTag("node");

    //    List<Transform> result = new List<Transform>();

    //    //foreach(GameObject nodeGO in nodes)
    //    //{
    //    //    Node node = nodeGO.GetComponent<Node>();

    //    //    print("node name: " + node.name + " parent node: " + node.parentNodeName);
    //    //}


    //    Transform node = Search(start, end.name);


    //    // While there's still previous node, we will continue.
    //    while (node != null)
    //    {
    //        result.Add(node);
    //        Node currentNode = node.GetComponent<Node>();
    //        print("nasao " + currentNode.name);
    //        print("parent node  " + currentNode.parentNodeName);
    //        node = Search(start, currentNode.parentNodeName); // currentNode.getParentNode();
    //        print("parent node 2 " + currentNode.parentNodeName);
    //    }

    //    // Reverse the list so that it will be from start to end.
    //    result.Reverse();
    //    return result;
    //}

    /// <summary>
    /// Dijkstra Algorithm to find the shortest path
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>The end node</returns>
    private Transform DijkstrasAlgo(Transform start, Transform end)
    {
        double startTime = Time.realtimeSinceStartup;

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

        // Set the starting node weight to 0;
        Node startNode = start.GetComponent<Node>();
        startNode.setWeight(0);

        while (unexplored.Count > 0)
        {
            // Sort the explored by their weight in ascending order.
            unexplored.Sort((x, y) => x.GetComponent<Node>().getWeight().CompareTo(y.GetComponent<Node>().getWeight()));

            // Get the lowest weight in unexplored.
            Transform current = unexplored[0];

            // Note: This is used for games, as we just want to reduce compuation, better way will be implementing A*
            /*
            // If we reach the end node, we will stop.
            if(current == end)
            {   
                return end;
            }*/

            //Remove the node, since we are exploring it now.
            unexplored.Remove(current);

            Node currentNode = current.GetComponent<Node>();
            List<Transform> neighbours = currentNode.getNeighbourNode();
            foreach (Transform neighNode in neighbours)
            {
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

        double endTime = (Time.realtimeSinceStartup - startTime);
        //print("Compute time: " + endTime);

        //print("Path completed!");

        return end;
    }



    public Transform Search(Transform root, string nameToSearchFor)
    {
        //string nameToSearchFor = nameToSearchForT.GetComponent<Node>().name;

        Queue<Transform> Q = new Queue<Transform>();
        HashSet<Transform> S = new HashSet<Transform>();
        Q.Enqueue(root);
        S.Add(root);

        while (Q.Count > 0)
        {
            Transform e = Q.Dequeue();
            if (e.GetComponent<Node>().name == nameToSearchFor)
                return e;
            foreach (Transform friend in e.GetComponent<Node>().getNeighbourNode())
            {
                print("prolazi");
                if (!S.Contains(friend))
                {
                    Q.Enqueue(friend);
                    S.Add(friend);
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Breadth-first search BFS traversal Algorithm to find the shortest path
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>The end node</returns>
    public List<Transform> BFS_traversalAlgo(Transform start, Transform end)
    {
        Queue<Transform> q = new Queue<Transform>();
        q.Enqueue(start);

        while (q.Count > 0)
        {
            start = q.Dequeue();

            foreach (Transform child in start.GetComponent<Node>().getNeighbourNode())
                if (child != null)
                    q.Enqueue(child);
        }

        return q.ToList<Transform>();
    }

    /// <summary>
    /// DFS traversal Algorithm to find the shortest path
    /// </summary>
    /// <param name="start">The start point</param>
    /// <param name="end">The end point</param>
    /// <returns>The end node</returns>
    private Transform DFS_traversalAlgo(Transform start, Transform end)
    {
        return null;
    }

    //DFS find path
    public List<Transform> FindPath_DFS_Algo(Transform start, Transform target)
    {
        List<Transform> path = new List<Transform>();

        path.Add(start);

        if (target == start)
        {
            return path;
        }

        foreach (Transform tn in start.GetComponent<Node>().getNeighbourNode())
        {
            List<Transform> childPath = FindPath_DFS_Algo(tn, target);
            if (childPath != null)
            {
                path.AddRange(childPath);
                return path;
            }
        }

        return null;
    }

}
