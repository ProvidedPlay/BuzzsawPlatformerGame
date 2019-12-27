using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TronTail : MonoBehaviour
{
    public float minimumVertexUpdateDistance;
    public int maxLineRendererPoints;
    public bool soundEffectActive = true;
    public string soundEffectName;

    List<Vector2> tailVertices = new List<Vector2>();
    List<Vector2> activeTailVertices = new List<Vector2>();
    Player player;
    ParticleSystem particleCone;
    ParticleSystem particleCircle;
    ParticleSystem[] particleGroup;
    Vector2 playerTransform;
    Vector2 lastVertex;
    Vector2 secondLastVertex;
    public GameObject currentTronTailObject;
    public LineRenderer currentTronTailLine;
    public EdgeCollider2D currentTronTailCollider;
    GameController controller;


    private void Awake()
    {
        player = GetComponent<Player>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        UnpackParticleSystems();
    }
    private void OnEnable()
    {
        //InitiateTronTail(player.transform.position); // debug
    }
    private void FixedUpdate()
    {
        if (player.tronTailActive)
        {
            if (!controller.gameOver)
            {
                CreateVertexPoint();
                AimTailParticles();
            }
            else if (controller.gameOver)
            {
                EndTronTail();
            }
        }
    }
    private void Update()
    {
        if (controller.gamePaused && soundEffectActive)
        {
            PauseSound(soundEffectName);
            soundEffectActive = false;
        }
        if(!controller.gamePaused && !soundEffectActive)
        {
            soundEffectActive = true;
            if (player.tronTailActive && !controller.gameOver)
            {
                ToggleSoundEffectOn(soundEffectName);
            }
        }
    }
    void UnpackParticleSystems()
    {
        particleGroup = GetComponentsInChildren<ParticleSystem>();
        particleCone = particleGroup[0];
        particleCircle = particleGroup[1];
    }
    bool CheckToCreateVertex()
    {
        //after player moves x magnitude from last vertex point, this boolean is true
        playerTransform = player.transform.position;

        if (Vector2.Distance(lastVertex, playerTransform) > minimumVertexUpdateDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void CreateVertexPoint()
    {
        //if CheckToCreateVertex is true, instantiate a vertex at this point, add the new vertex to the bottom of list list 'trailVertices', input this vertex to UpdateVertices
        if (CheckToCreateVertex())
        {
            Vector2 newVertexPoint = playerTransform;
            tailVertices.Add(newVertexPoint);
            lastVertex = newVertexPoint;
            UpdateTronLine(newVertexPoint);
            //AimTailParticles();
        }
    }
    void UpdateTronLine(Vector2 vertex)
    {
        //after a new vertex is added to the tailVertices list, render a 'tron' line between the second last and third last vertex in the list (not this one)
        int count = tailVertices.Count;
        if (count >= 3)
        {
            secondLastVertex = tailVertices[count - 2];
            Vector2 thirdLastVertex = tailVertices[count - 3];
            if (activeTailVertices.Count == 0)
            {
                activeTailVertices.Add(thirdLastVertex);
            }
            activeTailVertices.Add(secondLastVertex);
            
            if (currentTronTailLine != null)
            {
                currentTronTailLine.positionCount += 1;
                if (currentTronTailLine.positionCount == 2)
                {
                    currentTronTailLine.SetPosition(currentTronTailLine.positionCount - 2, thirdLastVertex);
                }
                currentTronTailLine.SetPosition(currentTronTailLine.positionCount - 1, secondLastVertex);
                currentTronTailCollider.points = activeTailVertices.ToArray();
                if (currentTronTailLine.positionCount > maxLineRendererPoints)
                {
                    InitiateTronTail(lastVertex);
                }
            }
        }
        
    }
    public void InitiateTronTail(Vector2 vertex)
    {
        player.tronTailActive = true; // debug
        GameObject newTronTail = Instantiate(Resources.Load("TronTail")) as GameObject;

        currentTronTailObject = newTronTail;
        currentTronTailLine = newTronTail.GetComponent<LineRenderer>();
        currentTronTailCollider = newTronTail.GetComponent<EdgeCollider2D>();

        currentTronTailLine.positionCount += 1;
        if (lastVertex !=vertex)
        {
            tailVertices.Add(vertex);
        }
        lastVertex = vertex;
        activeTailVertices.Clear();
        StartTailParticles();
        ToggleSoundEffectOn(soundEffectName);
    }
    public void EndTronTail()
    {
        if (player.tronTailActive)
        {
            player.tronTailActive = false; // debug
            currentTronTailObject = null;
            currentTronTailLine = null;
            currentTronTailCollider = null;
            secondLastVertex = Vector2.zero;
            tailVertices.Clear();
            activeTailVertices.Clear();
            EndTailParticles();
            ToggleSoundEffectOff(soundEffectName);
        }
    }
    void StartTailParticles()
    {
        foreach (ParticleSystem particles in particleGroup)
        {
            particles.Play();
        }
    }
    void EndTailParticles()
    {
        foreach (ParticleSystem particles in particleGroup)
        {
            particles.Stop();
        }
    }
    void AimTailParticles()
    {
        // orient x axis rotation of particle transform to face the most recent vertex (last vertex)
        Vector3 relativePosition = secondLastVertex - new Vector2(transform.position.x, transform.position.y);
        relativePosition.Normalize();

        float rotX = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg;

        particleCone.transform.rotation = secondLastVertex == Vector2.zero ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(-rotX, 90, 0);
        particleCircle.transform.rotation = secondLastVertex == Vector2.zero ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(-rotX, 11, 0);
    }
    public void ToggleSoundEffectOn(string soundEffect)
    {
        if (controller != null)
        {
            if (!controller.gameOver)
            {
                player.audioManager.LoopSoundEffect(soundEffect);
            }

        }
    }
    public void ToggleSoundEffectOff(string soundEffect)
    {
        if(controller != null)
        {
            player.audioManager.StopSoundEffect(soundEffect);
        }
    }
    public void PauseSound(string soundEffect)
    {
        if (controller != null)
        {
            player.audioManager.PauseSoundEffect(soundEffect);
        }
    }

}
