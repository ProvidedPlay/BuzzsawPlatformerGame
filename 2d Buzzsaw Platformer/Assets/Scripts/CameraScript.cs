using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector2 minBoundary;
    public Vector2 maxBoundary;
    public Vector2 cameraLeeway = new Vector2(1f, 1f);
    public Vector2 cameraSize;
    public float boundaryOrthographicSize = 9;
    public float damping = 0.5f;
    public float minimumDamp = 0.1f;
    public float velocityThreshold = 25;

    Transform playerTransform;
    Rigidbody2D playerRB;
    Player player;
    Vector2 minBoundaryOriginal;
    Vector2 maxBoundaryOriginal;
    Vector2 boundaryCameraSize;
    Vector3 targetPosition;
    Vector3 camVelocity = Vector3.zero;
    Camera cam;
    float yVelocity = 0;
    float currentDampY;

    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        targetPosition = transform.position;
        cam = GetComponent<Camera>();
    }
    private void Start()
    {
        SetupBoundaryCamera();
        minBoundaryOriginal = minBoundary;
        maxBoundaryOriginal = maxBoundary;
        UpdateCameraSize();
        
    }
    private void LateUpdate()
    {
        if (!player.playerIsDead)
        {
            CameraMovement();
        }

    }
    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - playerTransform.position.x) > cameraLeeway.x;
    }
    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - playerTransform.position.y) > cameraLeeway.y;
    }
    void CameraMovement()
    {
        Vector3 newPosition = transform.position;
        if (CheckXMargin())
        {
            newPosition.x = playerTransform.position.x;
        }
        if (CheckYMargin())
        {
            newPosition.y = playerTransform.position.y;
        }

        float yOffset = Mathf.Clamp(Mathf.Abs(transform.position.y - playerTransform.position.y), 0, cameraSize.y);
        currentDampY = Mathf.Abs(playerRB.velocity.y) < velocityThreshold? damping : Mathf.Lerp(minimumDamp, damping, (cameraSize.y - yOffset) / cameraSize.y);
        newPosition.x = CheckIfCenterCameraOnAxis("x") ? CenterCameraOnAxis("x") : Mathf.Clamp(newPosition.x, minBoundary.x, maxBoundary.x);
        newPosition.y = CheckIfCenterCameraOnAxis("y") ? CenterCameraOnAxis("y") : Mathf.Clamp(newPosition.y, minBoundary.y, maxBoundary.y);
 
        Vector3 movePosition = Vector3.SmoothDamp(transform.position, newPosition, ref camVelocity, damping);
        if (Mathf.Abs(playerRB.velocity.y) > velocityThreshold)
        {
            movePosition.y = Mathf.SmoothDamp(transform.position.y, newPosition.y, ref yVelocity, currentDampY);
        }

        transform.position = movePosition;


    }
    void UpdateCameraSize()
    {
        cameraSize = new Vector2(cam.orthographicSize* cam.aspect, cam.orthographicSize);
        cameraLeeway = cameraLeeway * (cam.orthographicSize / 9);
        SetCamBoundaries();
    }
    void SetupBoundaryCamera()
    {
        boundaryCameraSize = new Vector2(16 * (boundaryOrthographicSize * cam.aspect / 16), 9 * (boundaryOrthographicSize / 9));
    }
    void SetCamBoundaries()
    {
        minBoundary = new Vector2(minBoundaryOriginal.x - (boundaryCameraSize.x - cameraSize.x), minBoundaryOriginal.y - (boundaryCameraSize.y - cameraSize.y));
        maxBoundary = new Vector2(maxBoundaryOriginal.x + (boundaryCameraSize.x - cameraSize.x), maxBoundaryOriginal.y + (boundaryCameraSize.y - cameraSize.y));
        minBoundary = new Vector2(Mathf.Clamp(minBoundary.x, Mathf.NegativeInfinity, maxBoundary.x), Mathf.Clamp(minBoundary.y, Mathf.NegativeInfinity, maxBoundary.y));
        maxBoundary = new Vector2(Mathf.Clamp(maxBoundary.x, minBoundary.x, Mathf.Infinity), Mathf.Clamp(maxBoundary.y, minBoundary.y, Mathf.Infinity));
    }
    bool CheckIfCenterCameraOnAxis(string axis)
    {
        if ((minBoundary.x >= maxBoundary.x && axis == "x")||(minBoundary.y>=maxBoundary.y && axis == "y"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    float CenterCameraOnAxis(string axis)
    {
        Vector2 position = (maxBoundaryOriginal + minBoundaryOriginal) / 2;
        if (axis == "x")
        {
            return position.x;
        }
        if (axis == "y")
        {
            return position.y;
        }
        else
            return 0;
    }

    
}
