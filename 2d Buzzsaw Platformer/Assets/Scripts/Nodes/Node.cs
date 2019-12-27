using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    public int nodeInterval;

    public abstract int GetNodeInterval(GameObject[] nodeList, GameObject currentNode);
    public abstract int GetNodeInterval(GameObject[] nodeList, GameObject currentNode, int currentInterval);
    public abstract GameObject GetNextNode(GameObject[] nodeList, GameObject currentNode);
}
