using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class BasicEnemyAI : MonoBehaviour
{

    [Header("General Stuff")]
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;
    public float health;
    public float damage;

    [Header("Patroling")]
    public Vector3 patrolPoint;
    bool patrolPointSet;
    public float patrolPointRange;

    [Header("Attacking")]
    public float attackSpeed;
    bool alreadyAttacked;

    [Header("Which states")]
    public float sightRange,attackRange;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Gun")]
    public GameObject projectile;
    public float bulletSpeed = 30f;
    public Transform barrelPosition;
    


    [SerializeField]
    private GameObject playerChase;

    public EnemyCount enemynumber;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = Random.Range(70, 200);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemynumber.AddEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }

    }
    private void Patroling()
    {
        Gizmos.color = Color.magenta;
        
        if (!patrolPointSet)
        {
            SearchPatrolPoint();
        }
        if (patrolPointSet)
        {
            agent.SetDestination(patrolPoint);
        }

        Vector3 distanceBeforePatrolPoint = transform.position - patrolPoint;

        if(distanceBeforePatrolPoint.magnitude < 4)
        {
            patrolPointSet = false;
        }
    }
    private void ChasePlayer()
    {
        
        agent.SetDestination(player.position);
        
    }
    private void AttackPlayer()
    {
        
        //needs to stop or will jsut keep running into players face while shoots
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        ;

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, barrelPosition.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
            
            alreadyAttacked = true;
            Invoke(nameof(StartAttackAgain), attackSpeed);
        }
    }
    private void SearchPatrolPoint()
    {
        float randomZval = Random.Range(-patrolPointRange, patrolPointRange);
        float randomXval = Random.Range(-patrolPointRange, patrolPointRange);

        patrolPoint = new Vector3(transform.position.x + randomXval, transform.position.y, transform.position.z + randomZval);

        if(Physics.Raycast(patrolPoint, -transform.up, 2f, isGround))
        {
            patrolPointSet = true;
        }

    }
    private void StartAttackAgain()
    {
        alreadyAttacked = false;

    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            DestroyEnemy();
        }
    }
    private void DestroyEnemy()
    {
        enemynumber.RemoveEnemy();
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
