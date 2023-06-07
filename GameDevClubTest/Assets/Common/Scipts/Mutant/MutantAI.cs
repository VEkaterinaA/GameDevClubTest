using Assets.Common.Scipts.HeroInventory;
using Assets.Common.Scipts.Mutant.HelperClasses;
using Assets.Common.Scipts.Mutant.MutantModes;
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
    public Transform MutantBody;

    private float StartMutantPosX;
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
        StartMutantPosX = MutantBody.position.x;

        LoadNavMesh();

        LoadMutantModes();

        LoadMutantHelperClasses();

        SettingPositionMutant();
    }
    private void Update()
    {
        Vector2 distanceToWalkPoint = transform.position - _heroController.transform.position;

        if (distanceToWalkPoint.x < sightRange & distanceToWalkPoint.y < sightRange)
        {
            StopCoroutine(nameof(_patrol.CoroutinePatroling));

            if (distanceToWalkPoint.x < attackRange & distanceToWalkPoint.y < attackRange)
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
    private void Subscribe()
    {
        _mutantHealth.OnMutantDie += MutantDie;
    }
    private void Unsubscribe()
    {
        _mutantHealth.OnMutantDie -= MutantDie;
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
        _mutantHealth = new MutantHealthManagement(_mutantCharacteristics, healthBarImage);
        _mutantTurn = new MutantTurn() { startMutantPosX = StartMutantPosX, transform = MutantBody.transform };
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
                var itemRandom = _inventoryItemDataBase.GetRandomInventoryItem();
                inventoryItem.SetItem(itemRandom, Random.Range(1, 5));
            }
        }
        Unsubscribe();
        Destroy(this.gameObject);

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
