using Assets.Common.Scipts.Mutant.MutantModes;
using Assets.Common.Scipts.Mutant;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

public class MutantAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    //Classes
    private HeroController _heroController;
    private Attack _attack;
    private MutantPositionGeneration _mutantPositionGeneration;
    private Chase _chase;
    private Patrol _patrol;
    private MutantTurn _mutantTurn;
    //Patroling
    public float walkPointRange;
    public float timeBetweenPatrols;
    //Attacking
    public float timeBetweenAttacks;
    //States
    public float sightRange, attackRange;

    private Vector2 randomPoint;

    [Inject]
    void Construct(HeroController heroController, MutantPositionGeneration mutantPositionGeneration)
    {
        _heroController = heroController;
        _mutantPositionGeneration = mutantPositionGeneration;
    }
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updateRotation = false;

        _attack = new Attack();
        _chase = new Chase();
        _patrol = new Patrol();
        _mutantTurn = new MutantTurn();


        SettingPositionMutant();
    }
    private void Update()
    {
        Vector2 distanceToWalkPoint = transform.position - _heroController.transform.position;
        
        if(distanceToWalkPoint.x < sightRange && distanceToWalkPoint.y < sightRange)
        {
            StopCoroutine(nameof(_patrol.CoroutinePatroling));

            if(distanceToWalkPoint.x < attackRange && distanceToWalkPoint.y < attackRange)
            {
                StartCoroutine((_attack.CoroutineAttackHero(navMeshAgent,transform,timeBetweenAttacks)));
            }
            else
            {
                _chase.ChaseHero(_mutantTurn,navMeshAgent,_heroController.transform.position);
            }
        }
        else
        {
            StopCoroutine(nameof(_attack.CoroutineAttackHero));
            StartCoroutine((_patrol.CoroutinePatroling(_mutantTurn, navMeshAgent,transform.position,timeBetweenPatrols,walkPointRange,_mutantPositionGeneration)));
        }
    }
    private void SettingPositionMutant()
    {
        transform.position = _mutantPositionGeneration.GetRandomStartPointMutantPositionGeneration();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
