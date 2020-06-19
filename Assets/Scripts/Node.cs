using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Node : MonoBehaviour
{

    [SerializeField] private float weight = int.MaxValue;
    [SerializeField] private Transform parentNode = null;
    [SerializeField] public string parentNodeName = null;
    [SerializeField] private List<Transform> neighbourNode;
    [SerializeField] private bool walkable = true;

    public bool isStart = false;
    public bool isEnd = false;

    public int x=0;
    public int y=0;


    // Use this for initialization
    void Start()
    {
        this.resetNode();
    }

    /// <summary>
    /// Reset all the values in the nodes.
    /// </summary>
    public void resetNode()
    {
        weight = int.MaxValue;
        parentNode = null;
    }

    // -------------------------------- Setters --------------------------------

    /// <summary>
    /// Set the parent node.
    /// </summary>
    /// <param name="node">Set the node for parent node.</param>
    public void setParentNode(Transform node)
    {
        this.parentNode = node;
        this.parentNodeName = node.name;
    }

    /// <summary>.
    /// Set the weight value
    /// </summary>
    /// <param name="value">weight value</param>
    public void setWeight(float value)
    {
        this.weight = value;
    }

    /// <summary>
    /// Set is node is walkable.
    /// </summary>
    /// <param name="value">boolean</param>
    public void setWalkable(bool value)
    {
        this.walkable = value;

        if(value)
        {
            this.GetComponent<Image>().color = Color.white;
        }
        else
        {
            this.GetComponent<Image>().color = Color.black;
        }

    }

    /// <summary>
    /// Adding neighbour node object.
    /// </summary>
    /// <param name="node">Node transform</param>
    public void addNeighbourNode(Transform node)
    {
        this.neighbourNode.Add(node);

        node.GetComponent<Node>().setParentNode(this.transform);
    }

    // -------------------------------- Getters --------------------------------

    /// <summary>
    /// Get neighbour node.
    /// </summary>
    /// <returns>All the nodes.</returns>
    public List<Transform> getNeighbourNode()
    {
        List<Transform> result = this.neighbourNode;
        return result;
    }

    /// <summary>
    /// Get weight
    /// </summary>
    /// <returns>get weight in float.</returns>
    public float getWeight()
    {
        float result = this.weight;
        return result;

    }

    /// <summary>
    /// Get the parent Node.
    /// </summary>
    /// <returns>Return the parent node.</returns>
    public Transform getParentNode()
    {
        Transform result = this.parentNode;
        return result;
    }


    // -------------------------------- Checkers --------------------------------

    /// <summary>
    /// Get if the node is walkable.
    /// </summary>
    /// <returns>boolean</returns>
    public bool isWalkable()
    {
        bool result = walkable;
        return result;
    }

    // -------------------------------- Setters --------------------------------

    /// <summary>
    /// Set start node
    /// </summary>
    /// <returns>boolean</returns>
    public void SetStart()
    {
        this.isStart = true;
    }




}
