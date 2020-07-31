using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent Mob;
    public GameObject Player;
    public float MobDistanceRun = 4.0f;

    [SerializeField]
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        Mob = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if(distance < MobDistanceRun){
            animator.SetBool("Chase", true);
            Vector3 directionToPlayer = transform.position - Player.transform.position;
            Vector3 newPos = transform.position - directionToPlayer;

            Mob.SetDestination(newPos);
        }
    }
}
