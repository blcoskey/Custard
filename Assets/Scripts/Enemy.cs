using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private EnemyState currentState = EnemyState.Patrol;
    private NavMeshAgent mob;

    [Header ("Relations")]
    [SerializeField]
    private MazeManager mazeManager;
    public GameObject Player;
    public float mobDistanceRun = 4.0f;

    [SerializeField]
    private Animator animator;

    [Header ("Stats")]
    [SerializeField]
    private float walkSpeed = 5.0f;
    [SerializeField]
    private float runSpeed = 10.0f;
    [SerializeField]
    private float lineOfSight = 20.0f;
    
    [Header("Navigation")]

    [SerializeField]
    private int lastMaze = 0;
    public List<Transform> currentPatrolPath;
    public List<Transform> patrolPoints;
    public List<Transform> maze1PatrolPoints;
    public List<Transform> maze2PatrolPoints;
    public List<Transform> maze3PatrolPoints;
    private int destPoint = 0;

    void Start () {
        mazeManager = GameObject.Find ("Maze").GetComponent<MazeManager> ();
        mob = GetComponent<NavMeshAgent> ();
        mob.speed = walkSpeed;
        animator.SetBool ("Patrol", true);
        lastMaze = mazeManager.currentMaze;
        currentPatrolPath = GetPatrolPath();

        GotoNextPoint ();
    }

    // Update is called once per frame
    void Update () {
        switch (currentState) {
            case EnemyState.Patrol:
                PatrolUpdate ();
                break;

        }
        // float distance = Vector3.Distance (transform.position, Player.transform.position);

        // if (distance < mobDistanceRun) {
        //     animator.SetBool ("Chase", true);
        //     Vector3 directionToPlayer = transform.position - Player.transform.position;
        //     Vector3 newPos = transform.position - directionToPlayer;

        //     mob.SetDestination (newPos);
        // }
    }

    private void PatrolUpdate () {
        if(lastMaze != mazeManager.currentMaze){
            lastMaze = mazeManager.currentMaze;
            currentPatrolPath = GetPatrolPath();
        }
        if (!mob.pathPending && mob.remainingDistance < 0.5f)
            GotoNextPoint ();
    }

    void GotoNextPoint () {
        // Returns if no points have been set up
        if (currentPatrolPath.Count == 0)
            return;

        // Set the agent to go to the currently selected destination.
        mob.SetDestination (currentPatrolPath[destPoint].position);

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % currentPatrolPath.Count;
    }

    private List<Transform> GetPatrolPath(){
        var newPath = new List<Transform>();
        newPath.AddRange(patrolPoints);

        switch(mazeManager.currentMaze){
            case 1:
                newPath.AddRange(maze1PatrolPoints);
                break;
            case 2:
                newPath.AddRange(maze2PatrolPoints);
                break;
            case 3:
                newPath.AddRange(maze3PatrolPoints);
                break;
        }

        return newPath;
    }
}