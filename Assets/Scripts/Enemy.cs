using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class HuntTarget {
    public Transform transform { get; set; }
    public float distance { get; set; }
}

public class Enemy : MonoBehaviour {
    [SerializeField]
    private bool isHunting = false;
    [SerializeField]
    private EnemyState currentState = EnemyState.Patrol;
    private NavMeshAgent mob;

    [Header ("Relations")]
    [SerializeField]
    private MazeManager mazeManager;
    public BasicCharacter player;
    public float mobDistanceRun = 4.0f;

    [SerializeField]
    private Animator animator;

    [Header ("Stats")]
    [SerializeField]
    private float chaseScareFactor = 10.0f;
    [SerializeField]
    private float huntScareFactor = 3.0f;
    [SerializeField]
    private float walkSpeed = 5.0f;
    [SerializeField]
    private float runSpeed = 10.0f;
    [SerializeField]
    private float huntTime = 10.0f;

    [Header ("Navigation")]
    [SerializeField]
    private float lineOfSightDistance = 5.0f;
    [SerializeField]
    private Vector3 personalLastSighting;
    [SerializeField]
    private SphereCollider senseCollider;
    [SerializeField]
    private float fieldOfViewAngle = 110.0f;

    [SerializeField]
    private bool playerInSight = false;

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

        player = GameObject.FindWithTag("Player").GetComponent<BasicCharacter>();

        SetAllPaths();

        currentPatrolPath = GetPatrolPath ();
        senseCollider = GetComponent<SphereCollider> ();

        GotoNextPoint ();
    }

    public void SetAllPaths(){
        Transform mainHall = GameObject.Find("EnemyPatrolPoints/MainHall").GetComponent<Transform>();

        Transform bedroom1 = GameObject.Find("EnemyPatrolPoints/Bedroom1").GetComponent<Transform>();
        Transform bedroom2 = GameObject.Find("EnemyPatrolPoints/Bedroom2").GetComponent<Transform>();
        Transform bedroom3 = GameObject.Find("EnemyPatrolPoints/Bedroom3").GetComponent<Transform>();

        patrolPoints = new List<Transform>();
        patrolPoints.Add(mainHall);
        patrolPoints.Add(bedroom1);
        patrolPoints.Add(bedroom2);
        patrolPoints.Add(bedroom3);

        Transform maze1 = GameObject.Find("EnemyPatrolPoints/Maze1").GetComponent<Transform>();
        maze1PatrolPoints = new List<Transform>();
        maze1PatrolPoints.Add(maze1);

        Transform maze2 = GameObject.Find("EnemyPatrolPoints/Maze2").GetComponent<Transform>();
        maze2PatrolPoints = new List<Transform>();
        maze2PatrolPoints.Add(maze1);

        Transform maze3 = GameObject.Find("EnemyPatrolPoints/Maze3").GetComponent<Transform>();
        maze3PatrolPoints = new List<Transform>();
        maze3PatrolPoints.Add(maze1);
    }

    void Update () {
        switch (currentState) {
            case EnemyState.Hunt:
                Hunt ();
                break;
            case EnemyState.Patrol: 
                PatrolUpdate ();
                break;
            case EnemyState.Chase:
                Chase ();
                break;

        }
    }

    void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player") {
            playerInSight = false;
            Vector3 playerDirection = other.transform.position - transform.position;
            float angle = Vector3.Angle (playerDirection, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f) {
                if (Physics.Raycast (transform.position + transform.up, playerDirection.normalized, out RaycastHit raycastHit, lineOfSightDistance)) {
                    Debug.DrawRay (transform.position + transform.up, playerDirection.normalized * raycastHit.distance, Color.yellow);
                    if (raycastHit.collider.gameObject.tag == "Player") {
                        playerInSight = true;
                        personalLastSighting = other.transform.position;
                        ChangeState (currentState, EnemyState.Chase);
                    }
                }
            }
        }
    }

    private void Chase () {
        player.Scare(chaseScareFactor);
        float distance = Vector3.Distance (transform.position, player.transform.position);

        if (distance < mobDistanceRun) {
            animator.SetBool ("Chase", true);
            Vector3 directionToPlayer = transform.position - player.transform.position;
            Vector3 newPos = transform.position - directionToPlayer;

            mob.SetDestination (newPos);
        } else {
            ChangeState(currentState, EnemyState.Hunt);
        }
    }

    private List<Transform> GetHuntPath(){
        List<HuntTarget> huntLocations = new List<HuntTarget> ();

        foreach (Transform location in currentPatrolPath) {
            float distance = (location.position - personalLastSighting).sqrMagnitude;
            huntLocations.Add (new HuntTarget { transform = location, distance = distance });
        }

        return huntLocations.OrderBy(x => x.distance).Select(x => x.transform).ToList();
    }

    public void ChangeState (EnemyState prevState, EnemyState newState) {

        switch (prevState) {
            case EnemyState.Patrol:
                animator.SetBool ("Patrol", false);
                break;
            case EnemyState.Chase:
                animator.SetBool ("Chase", false);
                break;
            case EnemyState.Hunt:
                animator.SetBool ("Chase", false);
                isHunting = false;
                break;

        }

        switch (newState) {
            case EnemyState.Patrol:
                mob.speed = walkSpeed;
                animator.SetBool ("Patrol", true);
                break;
            case EnemyState.Chase:
                mob.speed = runSpeed;
                animator.SetBool ("Chase", true);
                break;
            case EnemyState.Hunt:
                mob.speed = runSpeed;
                animator.SetBool ("Chase", true);
                currentPatrolPath = GetHuntPath();
                break;
        }

        currentState = newState;
    }

    private void Hunt () {
        player.Scare(huntScareFactor);
        if(!isHunting){
            Invoke("EndHunt", huntTime);
            isHunting = true;
        }

        PatrolUpdate();
    }

    private void EndHunt(){
        if(isHunting)
            ChangeState(currentState, EnemyState.Patrol);
    }

    private void PatrolUpdate () {
        if (lastMaze != mazeManager.currentMaze) {
            lastMaze = mazeManager.currentMaze;
            currentPatrolPath = GetPatrolPath ();
        }
        if (!mob.pathPending && mob.remainingDistance < 0.5f)
            GotoNextPoint ();
    }

    private void GotoNextPoint () {
        if (currentPatrolPath.Count == 0)
            return;

        mob.SetDestination (currentPatrolPath[destPoint].position);

        destPoint = (destPoint + 1) % currentPatrolPath.Count;
    }

    private List<Transform> GetPatrolPath () {
        var newPath = new List<Transform> ();
        newPath.AddRange (patrolPoints);

        switch (mazeManager.currentMaze) {
            case 1:
                newPath.AddRange (maze1PatrolPoints);
                break;
            case 2:
                newPath.AddRange (maze2PatrolPoints);
                break;
            case 3:
                newPath.AddRange (maze3PatrolPoints);
                break;
        }

        return newPath;
    }
}