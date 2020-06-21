using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public string algoName;
    public bool stop = false;
    public int level = 1;
    public bool replayMode = false;
    public bool playMode = false;
    public bool moving = false;
    public int replayLevel = 0;
    public string playerStep;
    public List<string> playerPath = new List<string>() { };
    public List<string> blockPath = new List<string>() { };
    public List<Level> levels = new List<Level>();

    public void Start()
    {
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
