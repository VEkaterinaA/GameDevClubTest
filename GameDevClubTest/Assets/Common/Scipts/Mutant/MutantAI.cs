using Assets.Common.Scipts.HeroInventory;
using Assets.Common.Scipts.Mutant.HelperClasses;
using Assets.Common.Scipts.Mutant.MutantModes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class MutantAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    //Classes
    public MutantHealthManagement _mutantHealth;
    private HeroController _heroController;
    private Attack _attack;
    private MutantGenerationService _mutantPositionGeneration;
    private Chase _chase;
    private Patrol _patrol;
    private MutantControl _mutantTurn;
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
    public Transform MutantBody;

    [Inject]
    void Construct(HeroController heroController, MutantGenerationService mutantPositionGeneration, InventoryItemDataBase inventoryItemDataBase)
    {
        _heroController = heroController;
        _mutantPositionGeneration = mutantPositionGeneration;
        _inventoryItemDataBase = inventoryItemDataBase;
    }
    private void Start()
    {
        LoadNavMesh();

        InitMutantModes();

        InitMutantHelperClasses();

        SetMutantPosition();
    }
    private void Update()
    {
        Vector2 distanceToHero = transform.position - _heroController.transform.position;

        if (Mathf.Abs(distanceToHero.x) < sightRange & Mathf.Abs(distanceToHero.y) < sightRange)
        {
            StopCoroutine(nameof(_patrol.CoroutinePatroling));

            if (Mathf.Abs(distanceToHero.x) < attackRange & Mathf.Abs(distanceToHero.y) < attackRange)
            {
                StartCoroutine((_attack.CoroutineAttackHero(_heroController, navMeshAgent, transform, timeBetweenAttacks, _mutantCharacteristics.damage)));
            }
            else
            {
                _chase.ChaseHero(_mutantTurn, navMeshAgent, _heroController.transform.position);
            }
        }
        else
        {
            StopCoroutine(nameof(_attack.CoroutineAttackHero));
            StartCoroutine((_patrol.CoroutinePatroling(_mutantTurn, navMeshAgent, transform.position, timeBetweenPatrols, walkPointRange, _mutantPositionGeneration)));
        }
    }
    private void OnCollisionEnter2D(Collision2D objectCollisionEventDetails)
    {
        if (objectCollisionEventDetails.transform.tag != "Bullet")
        {
            return;
        }
        _mutantHealth.TakeDamage(_heroController._heroCharacteristics.damage);
        Destroy(objectCollisionEventDetails.gameObject);

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
    private void Subscribe()
    {
        _mutantHealth.OnMutantDie += DestroyMutantObject;
    }
    private void Unsubscribe()
    {
        _mutantHealth.OnMutantDie -= DestroyMutantObject;
    }
    private void SetMutantPosition()
    {
        transform.position = _mutantPositionGeneration.GetRandomStartPosition();
    }
    private void LoadNavMesh()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.updateRotation = false;
    }
    private void InitMutantModes()
    {
        _attack = new Attack();
        _chase = new Chase();
        _patrol = new Patrol();
    }
    private void InitMutantHelperClasses()
    {
        _mutantCharacteristics = new MutantCharacteristics();
        _mutantHealth = new MutantHealthManagement(_mutantCharacteristics, healthBarImage);
        _mutantTurn = new MutantControl() { startMutantPosX = MutantBody.position.x, transform = MutantBody.transform };
        Subscribe();
    }
    private void DestroyMutantObject()
    {
        try
        {
            Object prefabObj = Resources.Load("Prefabs/InventoryItem");
            var prefab = Instantiate(prefabObj, transform.position, Quaternion.identity);
            var inventoryItem = prefab.GetComponent<InventoryItem>();
            var itemRandom = _inventoryItemDataBase.GetRandomInventoryItem();
            inventoryItem.SetItem(itemRandom, Random.Range(1, 5));
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error occured during mutant destruction process, error: {ex.Message}");
        }
        finally
        {
            Unsubscribe();
            Destroy(this.gameObject);
        }
    }
}
