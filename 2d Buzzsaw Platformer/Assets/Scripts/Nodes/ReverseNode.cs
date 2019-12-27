using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseNode : Node
{
    public override int GetNodeInterval(GameObject[] nodeList, GameObject currentNode, int currentInterval)
    {
        if (currentNode != null)
        {
            nodeInterval = currentInterval * -1;
        }
        for (int i = 0; i < nodeList.Length; i++)
        {
            if (nodeList[i] == currentNode)
            {
                if (i + nodeInterval < 0 || i + nodeInterval >= nodeList.Length)
                {
                    nodeInterval = nodeInterval * -1;

                }
            }
        }
        return nodeInterval;
    }
    public override int GetNodeInterval(GameObject[] nodeList, GameObject currentNode)
    {
        if (currentNode != null)
        {
            nodeInterval = 1;
        }
        for (int i = 0; i < nodeList.Length; i++)
        {
            if (nodeList[i] == currentNode)
            {
                if (i + nodeInterval < 0 || i + nodeInterval >= nodeList.Length)
                {
                    nodeInterval = nodeInterval * -1;

                }
            }
        }
        return nodeInterval;
    }
    public override GameObject GetNextNode(GameObject[] nodeList, GameObject currentNode)
    {
        GameObject nextNode = null;
        for (int i = 0; i < nodeList.Length; i++)
        {
            if (nodeList[i] == currentNode)
            {
                if (i + nodeInterval >= 0 && i + nodeInterval < nodeList.Length)
                {
                    nextNode = nodeList[i + nodeInterval];

                }
            }
        }
        return nextNode;
    }
}
