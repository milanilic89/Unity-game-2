
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateMap : MonoBehaviour
{
    public int row = 10;
    public int column = 10;
    public float padding = 0f;
    public Transform nodePrefab1;
    public Transform nodePrefab2;

    public int Xstart = 0, Ystart = 4;
    public int Xend = 9, Yend = 4;

    private Transform startButton;
    private Transform endButton;

    public Transform startPrefab;
    public Transform endPrefab;
    public Transform playerPrefab;

    public int enabledAlgorithams = 3;

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

        GameObject canvas = GameObject.Find("RunCanvas");


        startButton = Instantiate(startPrefab, canvas.transform);
        startButton.name = "startRunner";
        startButton.GetComponent<Button>().onClick.AddListener(delegate { addNextRunner(); });
        endButton = Instantiate(endPrefab, canvas.transform);
        endButton.GetComponent<Button>().onClick.AddListener(delegate { PlayNextLevel(); });
        endButton.name = "playNewLevel";

        for (int i = 1; i <= enabledAlgorithams; i++)
        {
            Color randomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            Transform player = Instantiate(playerPrefab, startButton);
            player.name = "player" + i;
            player.GetComponent<Image>().color = i == enabledAlgorithams ? Color.white : randomColor;
            //player.GetComponent<Image>().color = randomColor;
        }


    }

    private void PlayNextLevel()
    { }

    private void addNextRunner()
    {
        //print("Next runner added");

        GameObject[] players = GameObject.FindGameObjectsWithTag("player");

        Player nextPlayer = null;

        for (int i = 0; i < players.Length; i++)
        {
            nextPlayer = players[i].GetComponent<Player>();

            if (players[i].transform.name.Equals("Player")) continue;

            if (!nextPlayer.playMode)
            {
                break;
            }
        }

        GameObject startNode = GameObject.Find("node ("+this.Xstart+","+this.Ystart+")");
        GameObject endNode = GameObject.Find("node (" + this.Xend + "," + this.Yend + ")");

        if (nextPlayer != null)
        {
            nextPlayer.playMode = true;
            nextPlayer.playerStep = startNode.transform.name;
            List<Transform> transformPath = nextPlayer.GetComponent<ShortestPath>().findShortestPath(startNode.transform, endNode.transform);
            List<string> path = new List<string>();

            foreach (Transform node in transformPath)
                path.Add(node.name);

            nextPlayer.playerPath = path;
        }
    }



    /// <summary>
    /// Generate the grid with the nodes.
    /// </summary>
    public void generateMapOnly()
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

    /// <summary>
    /// Set new position for Start button
    /// </summary>
    public void SetNewStart()
    {
        GameObject InputXStart = GameObject.Find("InputXStart");
        GameObject InputYStart = GameObject.Find("InputYStart");

        if (InputXStart != null && !System.String.IsNullOrEmpty(InputXStart.GetComponent<InputField>().text))
            this.Xstart = System.Int32.Parse(InputXStart.GetComponent<InputField>().text);

        if (InputYStart != null && !System.String.IsNullOrEmpty(InputYStart.GetComponent<InputField>().text))
            this.Ystart = System.Int32.Parse(InputYStart.GetComponent<InputField>().text);

        if (this.Xstart >= 0 && this.Ystart >= 0)
        {
            GameObject newNodeStart = GameObject.Find("node (" + this.Xstart + "," + this.Ystart + ")");

            this.startButton.position = newNodeStart.transform.position;
        }

    }

    /// <summary>
    /// Set new position for End button
    /// </summary>
    public void SetNewEnd()
    {
        GameObject InputXEnd = GameObject.Find("InputXEnd");
        GameObject InputYEnd = GameObject.Find("InputYEnd");

        if (InputXEnd != null && !System.String.IsNullOrEmpty(InputXEnd.GetComponent<InputField>().text))
            this.Xend = System.Int32.Parse(InputXEnd.GetComponent<InputField>().text);

        if (InputYEnd != null && !System.String.IsNullOrEmpty(InputYEnd.GetComponent<InputField>().text))
            this.Yend = System.Int32.Parse(InputYEnd.GetComponent<InputField>().text);

        if (this.Xend >= 0 && this.Yend >= 0)
        {
            GameObject newNodeEnd = GameObject.Find("node (" + this.Xend + "," + this.Yend + ")");

            this.endButton.position = newNodeEnd.transform.position;
        }
    }

    /// <summary>
    /// Generate new Map for Row and Column input
    /// </summary>
    public void resizeMap()
    {
        //this.row = 10;
        //this.column = 10;

        GameObject inputRow = GameObject.Find("InputRow");
        GameObject inputColumn = GameObject.Find("InputColumn");

        if (inputRow != null && !System.String.IsNullOrEmpty(inputRow.GetComponent<InputField>().text))
            this.row = System.Int32.Parse(inputRow.GetComponent<InputField>().text);
        if (inputColumn != null && !System.String.IsNullOrEmpty(inputColumn.GetComponent<InputField>().text))
            this.column = System.Int32.Parse(inputColumn.GetComponent<InputField>().text);

        GameObject map = GameObject.Find("Map");

        if (map != null)
        {
            foreach (Transform child in map.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }


        this.removeGrid();
        this.generateMapOnly();
        this.generateNeighbours();
    }
}
