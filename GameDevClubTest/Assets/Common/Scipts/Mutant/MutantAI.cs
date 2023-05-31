using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

public class MutantAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private HeroController _heroController;

    //Patroling
    public Vector2 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    //States
    public float sightRange, attackRange;

    [Inject]
    void Construct(HeroController heroController)
    {
        _heroController = heroController;
    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updateRotation = false;
    }
    private void Update()
    {
        Vector2 distanceToWalkPoint = transform.position - _heroController.transform.position;
        
        if(distanceToWalkPoint.x<sightRange && distanceToWalkPoint.y < sightRange)
        {
            if(distanceToWalkPoint.x < attackRange && distanceToWalkPoint.y < attackRange)
            {
                AttackHero();
            }
            else
            {
                ChaseHero();
            }
        }
        else
        {
            Patroling();
        }
    }
    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            navMeshAgent.SetDestination(walkPoint);
        }
        Vector2 distanceToWalkPoint = new Vector2(transform.position.x,transform.position.y) - walkPoint;

        if(distanceToWalkPoint.magnitude<1f)
        {
            walkPointSet = false;
        }
    }
    private void ChaseHero()
    {
        navMeshAgent.SetDestination(_heroController.transform.position);
    }
    private void AttackHero()
    {
        navMeshAgent.SetDestination(transform.position);
        
        if(!alreadyAttacked)
        {
            //Attack

            //
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack),timeBetweenAttacks);
        }
    }
    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomY = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector2(transform.position.x + randomX, transform.position.y + randomY);

        walkPointSet = true;
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
