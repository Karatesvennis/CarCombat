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
    new BoxCollider collider;
    public bool isGrounded;

    public enum EnemyStates { patrolling, attacking };
    public EnemyStates myState;

    Transform currentTarget;

    int currentWaypoint = 0;
    float selectTargetRate = 2f;
    float nextSelectTarget = 0f;
    float distanceToGround;

    public Vector3 extraForce;
    public Vector3 extraRotation;


    // Start is called before the first frame update
    void Start()
    {
        myState = EnemyStates.patrolling;
        FindObjectOfType<GameManager>().nrOfEnemiesAlive++;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        player = FindObjectOfType<PlayerController>().gameObject;
        //StartCoroutine(GetComponent<EnemyShoot>().Firing());
        waypoints = FindObjectsOfType<WayPoint>();
        distanceToGround = collider.bounds.extents.y;
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
    void FixedUpdate()
    {
        FollowTargetWithRotation(currentTarget.position, speed);
    }

    void FollowTargetWithRotation(Vector3 target, float speed)
    {

        Vector3 direction = target - rb.position;
        direction.Normalize();
        rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(direction), turnspeed * Time.fixedDeltaTime);

        if (IsGrounded())
        {

            float yVelocity = rb.velocity.y;
            rb.velocity = transform.forward * speed * Time.fixedDeltaTime; // own
            rb.velocity = new Vector3(rb.velocity.x, Physics.gravity.y, rb.velocity.z);


            Vector3 proximity = target - transform.position;

            switch (myState)
            {
                case EnemyStates.patrolling:

                    rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxPatrolSpeed);

                    if (proximity.magnitude < 0.5)
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
        

        extraForce = Vector3.Lerp(extraForce, Vector3.zero, Time.fixedDeltaTime * 5);
        extraRotation = Vector3.Lerp(extraRotation, Vector3.zero, Time.fixedDeltaTime * 5);
        rb.velocity += extraForce; // external
        rb.rotation *= Quaternion.Euler(extraRotation);


    }

    void SelectTarget()
    {
        currentWaypoint = Random.Range(0, waypoints.Length);
        currentTarget = waypoints[currentWaypoint].transform;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            if (other.gameObject.GetComponent<PlayerController>().IsGrounded())
            {
                player = other.gameObject;
                currentTarget = player.transform;
                myState = EnemyStates.attacking;
            }
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
