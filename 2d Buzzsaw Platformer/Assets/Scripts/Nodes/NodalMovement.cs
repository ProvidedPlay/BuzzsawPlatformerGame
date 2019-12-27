using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodalMovement : MonoBehaviour
{
    public GameObject[] nodeList;
    public float moveDelay;
    public float speed;

    Rigidbody2D rb;
    GameObject currentNode;
    GameObject targetNode;
    float delayTimer= 0;
    int nodeInterval;

    bool isInitializing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        InitializeNodes();
    }
    private void FixedUpdate()
    {
        Move();
    }
    void SnapToStart()
    {
        if(currentNode != null)
        {
            transform.position = currentNode.transform.position;
        }

    }
    void UpdateNodes()
    {
        currentNode = GetClosestNode(nodeList);
        UpdateNodeInterval();
        targetNode = GetNextNode(nodeList);
    }
    void InitializeNodes()
    {
        isInitializing = true;
        currentNode = GetClosestNode(nodeList);
        SnapToStart();
        UpdateNodeInterval();
        targetNode = GetNextNode(nodeList);
        isInitializing = false;
    }
    void UpdateNodeInterval()
    {
        Node current = currentNode.GetComponent<Node>();
        if (current != null)
        {
            if (isInitializing)
            {
                current.GetNodeInterval(nodeList, currentNode);
            }
            else
            {
                current.GetNodeInterval(nodeList, currentNode, nodeInterval);
            }
            nodeInterval = current.nodeInterval;
        }
    }
    void MoveToNextNode()
    {
        if (targetNode != null)
        {
            Vector3 targetPosition = targetNode.transform.position;
            float step = speed * Time.fixedDeltaTime;
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, step));
            
            
        }
    }
    void ResetMoveTimer()
    {
        delayTimer = Time.time + moveDelay;
    }
    bool CheckCanMove()
    {
        if (Time.time > delayTimer && targetNode != null)
        {
            return true;
        }
        return false;
    }
    void Move()
    {
        if (targetNode)
        {
            if (transform.position == targetNode.transform.position)
            {
                UpdateNodes();
                ResetMoveTimer();
            }
            if (CheckCanMove())
            {
                MoveToNextNode();
            }
        }

    }
    GameObject GetClosestNode(GameObject[] inputNodes)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestNode = null;
        foreach (GameObject node in inputNodes)
        {

            float distance = Vector3.Distance(transform.position, node.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = node;
            }
            
        }
        return closestNode;
    }
    GameObject GetNextNode(GameObject[] inputNodes)
    {
        GameObject nextNode = null;
        Node current = currentNode.GetComponent<Node>();

        if (currentNode != null)
        {
            nextNode = current.GetNextNode(inputNodes, currentNode);
        }
        return nextNode;
    }
}
