using Assets.Common.Scipts.CommonForHeroAndMutantsScripts;
using Assets.Common.Scipts.Hero;
using Assets.Common.Scipts.Hero.HelperClasses;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HeroController : MonoBehaviour
{
    public InventoryController inventory;
    private HeroMove _heroMove;
    private HeroCollisionWithObjects _heroCollisionWithObjects;
    private HeroCharacteristics _heroCharacteristics;
    private HealthBar _healthBar; 
    [HideInInspector]
    public Joystick joystick;

    private Rigidbody2D RigidbodyHero;
    public Transform elbowRotation;
    public Image healthBar;
    #region Attack
    [HideInInspector]
    public Transform TrackedMutantTransform;

    public event Action OnCollisionHeroFieldWithEnemy;
    #endregion

    public event Action OnHeroDeath;

    private bool IsAttacked = false;
    private float dirX, dirY;
    public float speed = 3f;
    public float timeBetweenAttacks = 1f;
    private void Start()
    {
        LoadHelperClasses();

        RigidbodyHero = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _heroMove.JoystickCoordinateUpdate(joystick, speed, out dirX, out dirY);
    }
    private void FixedUpdate()
    {
        RigidbodyHero.velocity = _heroMove.MotionVector(dirX, dirY);
        if (IsAttacked)
        {
            if (transform.position.x > TrackedMutantTransform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
            return;
        }
        if (RigidbodyHero.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.identity;

        }
    }
    private void OnCollisionEnter2D(Collision2D objectCollisionEventDetails)
    {
        var tag = objectCollisionEventDetails.transform.tag;
        if (tag == null)
        {
            return;
        }
        if (tag == "Inventory")
        {
            _heroCollisionWithObjects.MoveItemToInventory(objectCollisionEventDetails, inventory.slots);
            Destroy(objectCollisionEventDetails.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D objectCollisionEventDetails)
    {
        var tag = objectCollisionEventDetails.transform.tag;
        if (tag == "NPC")
        {
            IsAttacked = true;
            TrackedMutantTransform = objectCollisionEventDetails.transform;
            OnCollisionHeroFieldWithEnemy?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D objectCollisionEventDetails)
    {
        var tag = objectCollisionEventDetails.transform.tag;
        if (tag == "NPC")
        {
            IsAttacked = false;
            TrackedMutantTransform = null;
            OnCollisionHeroFieldWithEnemy?.Invoke();
        }
    }
    private void OnDestroy()
    {
        if (OnCollisionHeroFieldWithEnemy != null)
            foreach (var d in OnCollisionHeroFieldWithEnemy.GetInvocationList())
                OnCollisionHeroFieldWithEnemy -= (d as Action);
    }
    public void ClickShootButton()
    {
        if (TrackedMutantTransform == null)
        {
            Debug.Log("No mutant found during attack!");
            return;
        }
        StartCoroutine(Attack());
        ElbowRotationWhenAttacking(TrackedMutantTransform);
    }
    public IEnumerator Attack()
    {
        if (inventory.CountBullet != 0)
        {
            var mutant = TrackedMutantTransform.GetComponent<MutantAI>();
            yield return new WaitForSeconds(timeBetweenAttacks);
            mutant._mutantTakingDamage.TakeDamage(_heroCharacteristics.damage);
            inventory.SpendItem(inventory.slots[0].item, 1);
        }
    }
    private void ElbowRotationWhenAttacking(Transform trackedMutantTransform)
    {
        var HeroPosition = transform.position;
        var MutantPosition = trackedMutantTransform.position;
        var direction = MutantPosition - HeroPosition;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        elbowRotation.rotation = Quaternion.Euler(0, 0, angle);

    }
    public void TakingHeroDamage(int damage)
    {
        _heroCharacteristics.health -= damage;
        _healthBar.UpdateFillAmount(damage);

        if (_heroCharacteristics.health<=0)
        {
            //Die
            OnHeroDeath?.Invoke();
        }
    }
    private void LoadHelperClasses()
    {
        _heroCharacteristics = new HeroCharacteristics();
        _heroMove = new HeroMove();
        _heroCollisionWithObjects = new HeroCollisionWithObjects();
        _healthBar = new HealthBar(_heroCharacteristics.health, _heroCharacteristics.health, healthBar);
    }

}
