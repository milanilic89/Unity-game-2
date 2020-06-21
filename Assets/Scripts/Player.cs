using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Security.Cryptography;
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
    public int playerSpeed = 1;
    public bool replayMode = false;
    public bool moveToStart = false;
    public bool playMode = false;
    public bool moving = false;

    public string algoName;
    public List<Level> levels = new List<Level>();

    public InputField levelField;

    public string playerStep;

    public List<string> playerPath = new List<string>() { }; 
    private List<string> blockPath = new List<string>() { }; 

    public int replayLevel = 0;

    public void startMove()
    {
        this.currentNode = "start";
    }

    public void Start()
    {
    }

    public void ListLevels()
    {
        print("levels saved: " + this.levels.Count);
        foreach (Level l in this.levels)
        {
            print("::level:: " + l.level + " :: blocks :: " + l.blockPath.Count + " :: moves :: " + l.movePath.Count + " :: algo name ::" + l.algoName);
        }
    }

    public List<string> getPlayerPathForLevel(int ind)
    {
        foreach (Level level in this.levels)
        {
            if (level.level == ind)
            {
                return level.movePath;
                break;
            }
        }

        return null;
    }

    public List<Transform> getPlayerBlockPathForLevel(int ind)
    {
        foreach (Level level in this.levels)
        {
            if (level.level == ind)
            {
                return level.blockPath;
                break;
            }
        }
        return null;
    }

    public void ListLevels(int ind)
    {
        foreach (Level level in this.levels)
        {
            if (level.level == ind)
            {
                print("List Level " + level.level + "number of blocks " + level.blockPath.Count + "path algoritam " + level.movePath.Count + " alogritam " + level.algoName);

                break;
            }
        }
    }

    public Level GetLevel(int ind)
    {
        foreach (Level level in this.levels)
        {
            if (level.level == ind)
            {
                return level;
                break;
            }
        }

        return null;
    }

    public void IncreaseLevel()
    {
        this.level += 1;
        UpdateLevelInfo();
    }

    private bool anyPlayerMoving()
    {
        bool result = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag("player");

        for (int i = 0; i < players.Length; i++)
        {
            Player player = players[i].GetComponent<Player>();

            if (player.moving) result = true;
        }

        return result;
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

    public void Update()
    {
        if (this.playMode)
        {
            RunPlayer();
        }
        if (this.replayMode)
        {
            TriggerReplay(this.replayLevel, this.playerPath);

            if (this.playerStep.Equals(this.playerPath.Last()))
                replayMode = false;
        }
    }

    private void RunPlayer()
    {
        this.RunPath();

        if (this.playerStep.Equals(this.playerPath.Last()))
        {
            if (this.moving) this.IncreaseLevel();

            this.moving = false;
        }
    }

    private void RunPath()
    {
        TriggerPlay(this.playerPath);
    }

    public void printPath()
    {
        foreach (string node in this.playerPath)
            print(node);
    }

    public void colorPath()
    {
        foreach (string node in this.playerPath)
            GameObject.Find(node).GetComponent<Image>().color = Color.red;
    }

    private void transformToStart()
    {
        GameObject start = GameObject.FindGameObjectsWithTag("start").FirstOrDefault();

        if (start == null)
            start = GameObject.Find("node (1,4)");

        if (this.transform.position.x < start.transform.position.x)
        {
            float x_diff = start.transform.position.x - this.transform.position.x;
            this.transform.Translate(x_diff, 0, 0, Space.World);
        }
        else
        {
            float x_diff = this.transform.position.x - start.transform.position.x;
            this.transform.Translate(-x_diff, 0, 0, Space.World);
        }

        if (this.transform.position.y < start.transform.position.y)
        {
            float y_diff = start.transform.position.y - this.transform.position.y;
            this.transform.Translate(0, y_diff, 0, Space.World);
        }
        else
        {
            float y_diff = this.transform.position.y - start.transform.position.y;
            this.transform.Translate(0, -y_diff, 0, Space.World);
        }
    }

    public void TriggerReplay(int level, List<string> _path)
    {
        for (int i = 0; i < _path.Count - 1; i++)
        {
            if (this.playerStep.Equals(_path[i]))
            {
                MovePlayerVisual(this.playerStep, _path[i + 1]);
            }
        }
    }

    public void TriggerPlay(List<string> _path)
    {
        for (int i = 0; i < _path.Count - 1; i++)
        {
            if (this.playerStep.Equals(_path[i]))
            {
                MovePlayerVisual(this.playerStep, _path[i + 1]);
            }
        }
    }

    private void movePlayer(Transform path)
    {
        if (this.nextNode != path.name) return;

        if (this.currentNode == this.nextNode) return;

        float speed = 0.5f;

        int diff_x = (int)System.Math.Abs((path.position.x - this.transform.position.x));
        int diff_y = (int)System.Math.Abs((path.position.y - this.transform.position.y));

        if (diff_x == 0 && diff_y == 0)
        {
        }

        if (this.transform.position.x < path.position.x && diff_x >= 0)
        {
            int diff = (int)(path.position.x - this.transform.position.x);

            for (int i = 0; i <= diff; i++)
                this.transform.Translate(speed, 0, 0, Space.World);

            if (this.transform.position.x >= path.position.x)
            {
                this.currentNode = this.nextNode;
                this.nextNode = getNextElement(path);
            }
        }

        if (this.transform.position.y < path.position.y && diff_y > 0)
        {
            int diff = (int)(path.position.y - this.transform.position.y);
            for (int i = 0; i <= diff; i++)
                this.transform.Translate(0, speed, 0, Space.World);

            if (this.transform.position.y >= path.position.y)
            {
                this.currentNode = this.nextNode;
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

    private void MovePlayerToStart()
    {
        this.moveToStart = true;
    }

    private void SetBlocksForreplay(List<string> blockPath)
    {
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");

        foreach (GameObject gamenode in nodes)
        {
            gamenode.GetComponent<Image>().color = Color.white;
        }

        foreach (string block in blockPath)
        {
            GameObject blockGO = GameObject.Find(block);
            blockGO.GetComponent<Image>().color = Color.black;
        }
    }

    public void MovePlayerVisual(string startNode, string endNode)
    {
        GameObject start = GameObject.Find(startNode);
        GameObject end = GameObject.Find(endNode);

        if (startNode.Equals(endNode)) return;

        if (!this.playerStep.Equals(startNode))
        {
            this.playerStep = endNode;
            return;
        }

        if (start != null && end != null)
        {
            movePlayerToNode(getDirection(start, end), end.transform);
        }

    }

    Direction getDirection(GameObject start, GameObject end)
    {
        Direction direction = Direction.Defult;

        Node _start = start.GetComponent<Node>();
        Node _end = end.GetComponent<Node>();

        if (_start.y == _end.y)
        {
            if (_start.x < _end.x)
                direction = Direction.Bottom;
            else
                direction = Direction.Top;
        }

        if (_start.x == _end.x)
        {
            if (_start.y < _end.y)
                direction = Direction.Right;
            else
                direction = Direction.Left; ;
        }

        return direction;
    }

    public enum Direction
    {
        Defult = 0,
        Right = 1,
        Left = 2,
        Top = 3,
        Bottom = 4
    }

    private void movePlayerToNode(Direction direction, Transform target)
    {
        float speed = 1.5f;

        if (this.transform.name == "player1") speed = 1f;
        if (this.transform.name == "player3") speed = 2f;

        int diff_x = (int)System.Math.Abs((this.transform.position.x - target.position.x));
        int diff_y = (int)System.Math.Abs((this.transform.position.y - target.position.y));

        // right/left
        if (diff_x > 0)
        {
            if (this.transform.position.x < target.position.x && direction == Direction.Right)
                this.transform.Translate(speed, 0, 0, Space.World);

            if (this.transform.position.x > target.position.x && direction == Direction.Left)
                this.transform.Translate(-speed, 0, 0, Space.World);
        }

        // top/bottom
        if (diff_y > 0)
        {
            if (this.transform.position.y < target.position.y && direction == Direction.Top)
                this.transform.Translate(0, speed, 0, Space.World);

            if (this.transform.position.y > target.position.y && direction == Direction.Bottom)
                this.transform.Translate(0, -speed, 0, Space.World);
        }

        if (diff_x == 0 && diff_y == 0 || (diff_x == 1 && diff_y == 0) || (diff_x == 0 && diff_y == 1))
        {
            this.playerStep = target.name;
        }
    }

    private void UpdateLevelInfo()
    {
        GameObject _textLevelGameObject = GameObject.Find("TextLevel");

        _textLevelGameObject.GetComponent<Text>().text = "Level " + this.level;

        _textLevelGameObject = GameObject.Find("game_info");

        _textLevelGameObject.GetComponent<Text>().text = "LEVEL " + (this.level - 1) + " COMPLETED";
    }

}
