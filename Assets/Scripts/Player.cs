using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Transform> paths;

    public string currentNode = null;
    private string nextNode = null;

    public bool autorun = false;

    public Text levelText;
    public Text levelCompletedText;

    public bool stop = false;
    public int level = 1;
    public int playerSpeed = 4;

    public Transform startNode = null;
    public Transform endNode = null;

    public List<Level> levelsHistory = new List<Level>();

    public void startMove()
    {
        //this.move = true;

        this.currentNode = "start";
        this.levelsHistory.Add(new Level(this.level, getBlackNodes(), this.paths));
    }

    public void Start()
    {
        this.level = 1;
    }

    public void IncreaseLevel()
    {
        this.level += 1;

        UpdateLevelInfo();

        this.levelsHistory.Add(new Level(this.level, getBlackNodes(), this.paths));
    }

    private List<Transform> getBlackNodes()
    {
        List<Transform> result = new List<Transform>();

        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");

        foreach (GameObject nodeTile in nodes)
        {
            Node node = nodeTile.GetComponent<Node>();
            if (!node.isWalkable() && !node.isStart && !node.isEnd)
                result.Add(node.transform);
        }

        return result;
    }

    private void UpdateLevelInfo()
    {
        GameObject _textLevelGameObject = GameObject.Find("TextLevel");

        _textLevelGameObject.GetComponent<Text>().text = "Level " + this.level;

        _textLevelGameObject = GameObject.Find("game_info");

        _textLevelGameObject.GetComponent<Text>().text = "LEVEL " + (this.level - 1) + " COMPLETED";
    }

    public void Update()
    {
        if (!stop)
            Move();

        if (this.level > 1 && this.stop)
        {
            GameObject _textLevelGameObject = GameObject.Find("TextLevel");

            _textLevelGameObject.GetComponent<Text>().text = "GAME OVER";
        }
    }

    public void Move()
    {
        if (this.level == 1)
            this.levelsHistory.Add(new Level(this.level, getBlackNodes(), this.paths));

        if (paths.Count == 0) return;
        if (paths.Last() == this.transform) return;

        if (paths.Count == 1)
        {
            this.GameOver();
            return;
        }

        if (this.currentNode == null)
        {
            this.currentNode = paths.First().name;
            this.nextNode = paths.ElementAt(1).name;
        }

        foreach (Transform path in paths)
        {
            movePlayer(path);
        }

        if (this.currentNode == paths.Last().name)
        {
            this.currentNode = "start";
            this.currentNode = "completed";

            if (this.levelCompletedText == null)
            {
                GameObject _textLevelGameObject = GameObject.Find("TextLevel");

                this.levelText = _textLevelGameObject.GetComponent<Text>();

                _textLevelGameObject = GameObject.Find("game_info");

                this.levelCompletedText = _textLevelGameObject.GetComponent<Text>();

                //_textLevelGameObject.GetComponent<Text>().text = "LEVEL COMPLETED";
            }

            if (this.levelCompletedText != null)
                this.levelCompletedText.text = "LEVEL " + this.level + " COMPLETED"; //this.levelText.text + " COMPLETED";


            // if autorun is enabled
            if (this.autorun)
            {
                PlayerInput input = this.GetComponent<PlayerInput>();
                if (input != null)
                {
                    //input.btnFindPath();
                    input.PlayNextLevel();

                    if (this.levelText != null)
                        this.levelText.text = "Level " + this.level;
                }
            }

        }

    }


    private void movePlayer(Transform path)
    {
        // move only for one Node
        if (this.nextNode != path.name) return;

        if (this.currentNode == this.nextNode) return;

        float speed = 0.5f;


        int diff_x = (int)System.Math.Abs((path.position.x - this.transform.position.x));
        int diff_y = (int)System.Math.Abs((path.position.y - this.transform.position.y));


        if (this.transform.position.x < path.position.x && diff_x >= 0)
        {
            int diff = (int)(path.position.x - this.transform.position.x);

            for (int i = 0; i <= diff; i++)
                this.transform.Translate(speed, 0, 0, Space.World);

            if (this.transform.position.x >= path.position.x)
            {
                this.currentNode = this.nextNode;
                //print("Imamo korak 1");
                this.nextNode = getNextElement(path);
            }
        }

        if (this.transform.position.y < path.position.y && diff_y > 0)
        {
            //print("this cordinates x= " + this.transform.position.x + " , y= " + this.transform.position.y);
            //print("path cordinates x= " + path.position.x + " , y= " + path.position.y);
            //print("diff " + (path.position.y - this.transform.position.y));

            int diff = (int)(path.position.y - this.transform.position.y);
            for (int i = 0; i <= diff; i++)
                this.transform.Translate(0, speed, 0, Space.World);

            if (this.transform.position.y >= path.position.y)
            {
                //print("this cordinates x= " + this.transform.position.x + " , y= " + this.transform.position.y);
                //print("path cordinates x= " + path.position.x + " , y= " + path.position.y);
                //print("diff= " + (path.position.y - this.transform.position.y));

                this.currentNode = this.nextNode;
                //print("Imamo korak 2");
                this.nextNode = getNextElement(path);
            }

        }

        if (this.transform.position.x > path.position.x && diff_x > 0)
        {
            int diff = (int)(this.transform.position.x - path.position.x);
            for (int i = 0; i <= diff; i++)
                this.transform.Translate(-speed, 0, 0, Space.World);

            if (this.transform.position.x <= path.position.x)
            {
                this.currentNode = this.nextNode;
                //print("Imamo korak 3");
                this.nextNode = getNextElement(path);
            }
        }

        if (this.transform.position.y > path.position.y && diff_y >= 0)
        {
            int diff = (int)(this.transform.position.y - this.transform.position.y);
            for (int i = 0; i <= diff; i++)
                this.transform.Translate(0, -speed, 0, Space.World);

            if (this.transform.position.y <= path.position.y)
            {
                this.currentNode = this.nextNode;
                //print("Imamo korak 4");
                this.nextNode = getNextElement(path);
            }
        }
    }

    string getNextElement(Transform element)
    {
        string result = null;

        if (paths.Contains(element))
        {
            if (paths.IndexOf(element) + 1 < paths.Count)
                result = paths.ElementAt(paths.IndexOf(element) + 1).name;
        }

        return result;
    }

    public void reset()
    {
        this.currentNode = null;
        this.nextNode = null;
    }

    public void GameOver()
    {
        GameObject levelText = GameObject.Find("TextLevel");

        if (levelText != null)
        {
            levelText.GetComponent<Text>().text = "GAME OVER";
            levelText.GetComponent<Text>().color = Color.red;
        }
    }

    public void replay()
    {
        this.reset();
        this.Move();
    }

    public void ListLevels(int ind)
    {
        //ind = 1; // level

        foreach (Level level in this.levelsHistory)
        {
            if (level.level == ind)
            {
                print("List Level " + level.level + "number of blocks " + level.blockPath.Count + "path algoritam " + level.movePath.Count);

                break;
            }
        }
    }

    public void PlayLevel(int ind)
    {        
        foreach (Level level in this.levelsHistory)
        {
            if (level.level == ind)
            {
                print("REPLAYING List Level " + level.level + "number of blocks " + level.blockPath.Count + "path algoritam " + level.movePath.Count);

                this.reset();
                this.level = level.level;
                this.paths = level.movePath;
                List<Transform> blocks = level.blockPath;

                this.Move();

                break;
            }
        }
    }



}
