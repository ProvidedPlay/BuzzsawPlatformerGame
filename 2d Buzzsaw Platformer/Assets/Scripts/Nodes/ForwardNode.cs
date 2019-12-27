using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardNode : Node
{
    public override int GetNodeInterval(GameObject[] nodeList, GameObject currentNode)
    {
        if (currentNode != null)
        {
            nodeInterval = 1;
        }
        return nodeInterval;
    }
    public override int GetNodeInterval(GameObject[] nodeList, GameObject currentNode, int currentInterval)
    {
        if (currentNode != null)
        {
            nodeInterval = currentInterval;
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
                if(i+nodeInterval < 0)
                {
                    nextNode = nodeList[nodeList.Length - 1];
                }
                if(i+nodeInterval >= nodeList.Length)
                {
                    nextNode = nodeList[0];
                }
            }
        }
        return nextNode;
    }

}
