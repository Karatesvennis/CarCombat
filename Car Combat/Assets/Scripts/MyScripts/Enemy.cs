using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    [SerializeField] WayPoint[] waypoints;
    [SerializeField] float speed = 1f;
    [SerializeField] float maxPatrolSpeed = 10f;
    [SerializeField] float maxAttackSpeed = 10f;
    [SerializeField] float turnspeed = 5f;
    [SerializeField] GameObject deathVFX = null;

    Rigidbody rb;
    GameObject player;

    public enum EnemyStates { patrolling, attacking };
    public EnemyStates myState;

    Transform currentTarget;

    int currentWaypoint = 0;
    float selectTargetRate = 2f;
    float nextSelectTarget = 0f;



    // Start is called before the first frame update
    void Start()
    {
        myState = EnemyStates.patrolling;
        FindObjectOfType<GameManager>().nrOfEnemiesAlive++;
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerController>().gameObject;
        StartCoroutine(GetComponent<EnemyShoot>().Firing());
        waypoints = FindObjectsOfType<WayPoint>();
        SelectTarget();
    }

    private void OnDestroy()
    {
        if (FindObjectOfType<GameManager>())
        {
            FindObjectOfType<GameManager>().nrOfEnemiesAlive--;
        }
            
        GameObject myDeathVFX = Instantiate(deathVFX, this.transform.position, Quaternion.identity);
        Destroy(myDeathVFX, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        FollowTargetWithRotation(currentTarget.position, speed);
    }

    void FollowTargetWithRotation(Vector3 target, float speed)
    {
        Vector3 direction = target - rb.position;
        direction.Normalize();
        rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(direction), turnspeed * Time.deltaTime);
        
        rb.velocity = transform.forward * speed * Time.deltaTime;
        Vector3 proximity = target - transform.position;

        switch (myState)
        {
            case EnemyStates.patrolling:

                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxPatrolSpeed);

                if (proximity.magnitude < 1)
                {
                    SelectTarget();
                }
                break;

            case EnemyStates.attacking:

                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxAttackSpeed);
                break;

            default:
                break;
        }

    }

    void SelectTarget()
    {
        currentWaypoint = Random.Range(0, waypoints.Length);
        currentTarget = waypoints[currentWaypoint].transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            currentTarget = player.transform;
            myState = EnemyStates.attacking;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            myState = EnemyStates.patrolling;
            SelectTarget();
        }
    }
}
