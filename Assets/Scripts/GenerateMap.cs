using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public int row = 10;
    public int column = 10;
    public float padding = 0f;
    public Transform nodePrefab1;
    public Transform nodePrefab2;

    public int Xstart = 0, Ystart = 4;
    public int Xend = 9, Yend = 4;


    public List<Transform> grid = new List<Transform>();

    // Map initialization
    void Start()
    {
        this.generateGrid();
        this.generateNeighbours();
    }

    /// <summary>
    /// Generate the grid with the nodes.
    /// </summary>
    public void generateGrid()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                int randomPrefab = Random.Range(0, 2);
                Transform node = randomPrefab == 0 ? Instantiate(nodePrefab1, transform) : Instantiate(nodePrefab2, transform);
                
                node.name = "node (" + i + "," + j + ")";

                node.GetComponent<Node>().x = i;
                node.GetComponent<Node>().y = j;
                
                node.tag = "node";

                grid.Add(node);

            }
        }
    }

    /// <summary>
    /// Generate the neighbours for each node.
    /// </summary>
    private void generateNeighbours()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            Node currentNode = grid[i].GetComponent<Node>();
            int index = i + 1;

            // For those on the left, with no left neighbours
            if (index % column == 1)
            {
                // We want the node at the top as long as there is a node.
                if (i + column < column * row)
                {
                    currentNode.addNeighbourNode(grid[i + column]);   // North node
                }

                if (i - column >= 0)
                {
                    currentNode.addNeighbourNode(grid[i - column]);   // South node
                }
                currentNode.addNeighbourNode(grid[i + 1]);     // East node
            }

            // For those on the right, with no right neighbours
            else if (index % column == 0)
            {
                // We want the node at the top as long as there is a node.
                if (i + column < column * row)
                {
                    currentNode.addNeighbourNode(grid[i + column]);   // North node
                }

                if (i - column >= 0)
                {
                    currentNode.addNeighbourNode(grid[i - column]);   // South node
                }
                currentNode.addNeighbourNode(grid[i - 1]);     // West node
            }

            else
            {
                // We want the node at the top as long as there is a node.
                if (i + column < column * row)
                {
                    currentNode.addNeighbourNode(grid[i + column]);   // North node
                }

                if (i - column >= 0)
                {
                    currentNode.addNeighbourNode(grid[i - column]);   // South node
                }
                currentNode.addNeighbourNode(grid[i + 1]);     // East node
                currentNode.addNeighbourNode(grid[i - 1]);     // West node
            }

        }
    }

    /// <summary>
    /// Remove all nodes from grid.
    /// </summary>
    public void removeGrid()
    {
        this.grid.Clear();
    }
}
