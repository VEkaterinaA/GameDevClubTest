using Assets.Common.Scipts.Mutant.MutantModes;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;
using Assets.Common.Scipts.Mutant.HelperClasses;
using Unity.VisualScripting;
using Assets.Common.Scipts.HeroInventory;
using System;
using Object = UnityEngine.Object;
using UnityEngine.UI;

public class MutantAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    //Classes
    public MutantTakingDamage _mutantTakingDamage;
    private HeroController _heroController;
    private Attack _attack;
    private MutantPositionGeneration _mutantPositionGeneration;
    private Chase _chase;
    private Patrol _patrol;
    private MutantTurn _mutantTurn;
    private MutantCharacteristics _mutantCharacteristics;
    private InventoryItemDataBase _inventoryItemDataBase;
    //Patroling
    public float walkPointRange;
    public float timeBetweenPatrols;
    //Attacking
    public float timeBetweenAttacks;
    //States
    public float sightRange, attackRange;

    public Image healthBarImage;

    private Vector2 randomPoint;

    [Inject]
    void Construct(HeroController heroController, MutantPositionGeneration mutantPositionGeneration, InventoryItemDataBase inventoryItemDataBase)
    {
        _heroController = heroController;
        _mutantPositionGeneration = mutantPositionGeneration;
        _inventoryItemDataBase = inventoryItemDataBase;
    }
    private void Start()
    {

        LoadNavMesh();

        LoadMutantModes();

        LoadMutantHelperClasses();

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
                StartCoroutine((_attack.CoroutineAttackHero(_heroController,navMeshAgent,transform,timeBetweenAttacks, _mutantCharacteristics.damage)));
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
    private void Subscribe()
    {
        _mutantTakingDamage.OnMutantDie += MutantDie;
    }
    private void Unsubscribe()
    {
        _mutantTakingDamage.OnMutantDie -= MutantDie;
    }
    private void SettingPositionMutant()
    {
        transform.position = _mutantPositionGeneration.GetRandomStartPointMutantPositionGeneration();
    }
    private void LoadNavMesh()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updateRotation = false;
    }
    private void LoadMutantModes()
    {
        _attack = new Attack();
        _chase = new Chase();
        _patrol = new Patrol();
    }
    private void LoadMutantHelperClasses()
    {
        _mutantCharacteristics = new MutantCharacteristics();
        _mutantTakingDamage = new MutantTakingDamage(_mutantCharacteristics, healthBarImage);
        _mutantTurn = new MutantTurn();
        Subscribe();
    }
    private void MutantDie()
    {
        Object prefabObj = Resources.Load("Prefabs/InventoryItem");
        if (prefabObj != null)
        {
            var prefab = Instantiate(prefabObj, transform.position, Quaternion.identity);
            if (prefab != null)
            {
                var inventoryItem = prefab.GetComponent<InventoryItem>();

                inventoryItem.SetItem(_inventoryItemDataBase.GetRandomInventoryItem(), Random.Range(1, 5));
            }
        }
        Unsubscribe();
        Destroy(this);

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
