using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public Transform playerPrefab;

    private Transform node;
    private Transform startNode;
    private Transform endNode;
    public List<Transform> blockPath = new List<Transform>();

    public InputField x_start, y_start, x_end, y_end;
    public Text levelText;
    public Text levelCompletedText;


    public InputField map_size;
    public InputField InputFieldLevel;

    public Button startBtn, endBtn;

    private Player player;
    private bool colorPath = false;

    public bool ReplayMode = false;

    private Dictionary<int, string> sceneLevel = new Dictionary<int, string>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetDefaultNodes();
    }


    ///// <summary>
    ///// Button for Set Starting node.
    ///// </summary>
    public void btnStartNode()
    {
        if (this.startNode == null)
        {
            // set defult start node
            GameObject _startNode = GameObject.Find("node (1,4)");

            if (_startNode != null)
            {
                //_startNode.GetComponent<Image>().color = Color.blue;

                this.startNode = _startNode.transform;
                startNode.GetComponent<Node>().isStart = true;
            }
        }
    }

    /// <summary>
    /// Button for Set End node.
    /// </summary>
    public void btnEndNode()
    {
        if (this.endNode == null)
        {
            // set defult start node
            GameObject _endNode = GameObject.Find("node (8,7)");

            if (_endNode != null)
            {
                _endNode.GetComponent<Node>().isEnd = true;

                this.endNode = _endNode.transform;
            }
        }
    }

    /// <summary>
    /// Button for find path.
    /// </summary>
    public void btnFindPath()
    {
        //this.player.

        // Only find if there are start and end node.
        if (startNode != null && endNode != null)
        {
            //SetLatestBlockPath();
            SetGame();
            // Execute Shortest Path.
            ShortestPath finder = gameObject.GetComponent<ShortestPath>();


            if (finder == null)
            {
                finder = new ShortestPath();
            }

            List<Transform> paths = finder.findShortestPath(startNode, endNode);

            //print("paths count: " + paths.Count);

            //List<Transform> paths = finder.FindPath_DFS_Algo(startNode, endNode);

            //List<Transform> paths = finder.BFS_traversalAlgo(startNode, endNode);


            if (paths.Count == 0)
            {
                //print("GAME OVER");
                //this.levelText.text = "GAME OVER" + "score: " + this.levelText.text;
            }

            float r = Random.Range(1f, 10f);

            foreach (Transform path in paths)
            {
                if (path == startNode) continue;
                if (path == endNode) continue;

                // color path

                //if (this.colorPath)
                //path.GetComponent<Image>().color = new Color(0.5f, 0.1f, 0.1f, 1.0f);
            }

            //set path to player one
            GameObject[] players = GameObject.FindGameObjectsWithTag("player");

            (players[0].GetComponent<Player>()).paths = paths;

            (players[0].GetComponent<Player>()).reset();

            this.player = players[0].GetComponent<Player>();

            this.player.paths = paths;

            // add level after play
     //       Level l = new Level(this.player.level, blockPath, this.player.paths);
       //     this.player.levels.Add(l);


        }
    }


    void ClearBlockPathsAndSetNewOne(List<Transform> blocks)
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");

        // For each blocked path in the list
        foreach (GameObject gamenode in nodes)
        {
            Node n = gamenode.GetComponent<Node>();
            n.setWalkable(true);

            //gamenode.GetComponent<Image>().color = Color.white;
        }

        if (blocks != null)
        {
            foreach (Transform t in blocks)
            {
                Node n = t.GetComponent<Node>();
                n.setWalkable(false);
                //t.GetComponent<Image>().color = Color.black;
            }
        }
    }

    public void LoadPlayScene()
    {
        GameObject map = GameObject.Find("Map");

        if (map == null)
            SceneManager.LoadScene("RunAndPlay", LoadSceneMode.Additive);
        else
        {
            // run game
            btnFindPath();
        }
    }

    public void SetDefaultNodes()
    {
        if (this.startNode == null && this.endNode == null)
        {
            //print("start and end set again");
            btnStartNode();
            btnEndNode();
        }
    }

    public void SetGame()
    {
        GameObject[] _startButton = GameObject.FindGameObjectsWithTag("startButton");
        GameObject[] _endButton = GameObject.FindGameObjectsWithTag("endButton");

        GameObject[] player1 = GameObject.FindGameObjectsWithTag("player");

        if (startNode != null && endNode != null)
        {

            Vector2 newpos = startNode.position;

            player1[0].transform.position = newpos;
            _startButton[0].transform.position = newpos;


            newpos = endNode.position;
            _endButton[0].transform.position = newpos;
        }


    }

    public void applicationExit()
    {
        Application.Quit();
    }


    public void btnBlockPath()
    {
        //random node
        Node _node = GetRandomWalkableNode(this.blockPath);

        if (_node == null)
        {
            this.player.stop = true;
            return;
        }

        if (CheckRouteForBlockNode(_node, this.blockPath))
        {
            // route exists, add black node
            _node.setWalkable(false);
            this.blockPath.Add(_node.transform);
        }
        else
        {
            if (!AddAnyPossibleBlock(this.blockPath))
            {
                this.player.stop = true;
                this.player.GameOver();
            }
        }

        if (this.player != null)
            this.player.IncreaseLevel();

    }



    /// <summary>
    /// Check if there is at least one path for the random block Node.
    /// </summary>
    private bool CheckRouteForBlockNode(Node node, List<Transform> blockPath)
    {
        bool result = false;

        node.setWalkable(false);
        blockPath.Add(node.transform);

        ShortestPath finder = gameObject.GetComponent<ShortestPath>();


        if (finder == null) finder = new ShortestPath();

        List<Transform> paths = finder.findShortestPath(startNode, endNode);

        if (paths.Count <= 1)
        {
            result = false;
        }
        else
        {
            result = true;
        }


        node.setWalkable(true);
        blockPath.Remove(node.transform);
        return result;
    }

    /// <summary>
    /// Check if there is at least one path for any walkable node.
    /// return "else" - gameover
    /// retrun "true" - added new random block
    /// </summary>
    private bool AddAnyPossibleBlock(List<Transform> blockPath)
    {
        bool result = false;
        GameObject[] mapNodes = GameObject.FindGameObjectsWithTag("node");
        List<Node> walkableNodes = new List<Node>();

        foreach (GameObject mapNode in mapNodes)
        {
            Node node = mapNode.GetComponent<Node>();
            if (node.isWalkable())
                walkableNodes.Add(node);
        }

        Node walkableNode = null;

        foreach (Node walkable in walkableNodes)
        {
            if (CheckRouteForBlockNode(walkable, blockPath))
            {
                result = true;
                walkableNode = walkable;
                break;
            }
        }

        if (walkableNode != null)
        {
            walkableNode.setWalkable(false);
            blockPath.Add(walkableNode.transform);
        }

        return result;
    }

    /// <summary>
    /// Get random walkable node
    /// </summary>
    private Node GetRandomWalkableNode(List<Transform> blockPath)
    {
        Node result = null;

        GameObject[] mapNodes = GameObject.FindGameObjectsWithTag("node");

        List<Node> walkableNodes = new List<Node>();

        foreach (GameObject mapNode in mapNodes)
        {
            Node node = mapNode.GetComponent<Node>();
            if (node.isWalkable() && !node.isStart && !node.isEnd)
                walkableNodes.Add(node);
        }

        int i = Random.Range(0, walkableNodes.Count);

        if (walkableNodes.Count != 0)
            result = walkableNodes.ElementAt(i);

        return result;
    }

    /// <summary>
    /// Check if there is at least one path for any walkable node.
    /// </summary>
    private bool CheckAnyPossibleBlock(List<Transform> blockPath)
    {
        GameObject[] mapNodes = GameObject.FindGameObjectsWithTag("node");
        List<Node> walkableNodes = new List<Node>();


        foreach (GameObject mapNode in mapNodes)
        {
            Node node = mapNode.GetComponent<Node>();
            if (node.isWalkable())
                walkableNodes.Add(node);
        }

        foreach (Node n in walkableNodes)
        {
            if (CheckRouteForBlockNode(n, blockPath))
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Button to unblock a path.
    /// </summary>
    public void btnUnblockPath()
    {
        foreach (Transform t in blockPath)
        {
            Node node = t.GetComponent<Node>();
            node.setWalkable(true);
            t.GetComponent<Image>().color = Color.white;
        }

        if (this.levelText != null)
            this.levelText.text = "Level 1";

        blockPath.Clear();
    }

    /// <summary>
    /// Clear all the block path.
    /// </summary>
    public void btnClearBlock()
    {
        // For each blocked path in the list
        foreach (Transform path in blockPath)
        {
            // Set walkable to true.
            Node n = path.GetComponent<Node>();
            n.setWalkable(true);

            // Set their color to white.
            Renderer rend = path.GetComponent<Renderer>();
            rend.material.color = Color.white;

        }
        // Clear the block path list and 
        blockPath.Clear();
    }

    /// <summary>
    /// Clear all the player paths.
    /// </summary>
    public void btnClearPaths()
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");

        // For each blocked path in the list
        foreach (GameObject gamenode in nodes)
        {
            Node n = gamenode.GetComponent<Node>();

            if (n.isWalkable())
                gamenode.GetComponent<Image>().color = Color.white;
        }
    }

    /// <summary>
    /// Button to restart level.
    /// </summary>
    public void btnRestart()
    {
        Scene loadedLevel = SceneManager.GetActiveScene();
        SceneManager.LoadScene(loadedLevel.buildIndex);
    }

    /// <summary>
    /// Coloured unwalkable path to black.
    /// </summary>
    private void colorBlockPath()
    {
        foreach (Transform block in blockPath)
        {
            Renderer rend = block.GetComponent<Renderer>();
            rend.material.color = Color.black;
        }
    }

    public void btnSizeMap()
    {
        GameObject map = GameObject.Find("Panel");

        foreach (Transform child in map.transform)
        {
            if (child.tag == "node")
                GameObject.Destroy(child.gameObject);
        }

        if (map != null)
        {
            GenerateMap mapGrid = map.GetComponent<GenerateMap>();

            mapGrid.row = int.Parse(map_size.text);

            mapGrid.removeGrid();
            mapGrid.generateGrid();
        }

    }

    /// <summary>
    /// Refresh Update Nodes Color.
    /// </summary>
    private void updateNodeColor()
    {
        if (startNode != null)
        {
            Renderer rend = startNode.GetComponent<Renderer>();
            rend.material.color = Color.blue;
        }

        if (endNode != null)
        {
            Renderer rend = endNode.GetComponent<Renderer>();
            rend.material.color = Color.cyan;
        }
    }

    /// <summary>
    /// Set X, Y cordinates for Start Node
    /// </summary>
    public void setStartCordinates()
    {
        if (x_start != null && y_start != null)
        {
            string _newStartNode = System.String.Format("node ({0},{1})", x_start.text, y_start.text);

            GameObject newStart = GameObject.Find(_newStartNode);

            if (newStart != null)
            {
                this.startNode = newStart.transform;
                this.startNode.position = newStart.transform.position;
            }
        }
    }

    /// <summary>
    /// Set X,Y cordinates for End Node
    /// </summary>
    public void setEndCordinates()
    {
        if (x_end != null && y_end != null)
        {
            string _newEndNode = System.String.Format("node ({0},{1})", x_end.text, y_end.text);

            GameObject newEnd = GameObject.Find(_newEndNode);

            if (newEnd != null)
            {
                this.endNode = newEnd.transform;
                this.endNode.position = newEnd.transform.position;
            }
        }
    }

    public void btnReplay()
    {

        this.player.replay();
        this.player.replayMode = true;
        this.ReplayMode = true;
        btnFindPath();

        PlayerPrefs.SetString("ReplayRun", "true");
    }

    public void PlayNextLevel()
    {
        btnFindPath();
    }

    public void AutoRun()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("player");

        //foreach(GameObject go in players)
        this.player = players[0].GetComponent<Player>();
        this.player.autorun = !this.player.autorun;
    }
}
