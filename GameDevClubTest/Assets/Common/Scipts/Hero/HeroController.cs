using Assets.Common.Scipts;
using Assets.Common.Scipts.CommonForHeroAndMutantsScripts;
using Assets.Common.Scipts.Hero;
using Assets.Common.Scipts.Hero.HelperClasses;
using Assets.Common.Scipts.HeroInventory;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HeroController : MonoBehaviour
{
    //Classes
    public InventoryController inventory;
    private HeroMove _heroMove;
    public HeroCharacteristics _heroCharacteristics;
    private HealthBar _healthBar;
    private FileOperations _data;
    [HideInInspector]
    public Joystick joystick;
    //Body
    private Rigidbody2D RigidbodyHero;
    public Transform ElbowRotation;
    public Image HealthBar;
    public Transform HeroBody;
    //Attack
    [HideInInspector]
    public Transform TrackedMutantTransform;
    public event Action OnCollisionHeroFieldWithEnemy;
    //Death
    public event Action OnHeroDeath;

    private List<Transform> ListAttackMutants = new List<Transform>();
    private bool IsShoot = false;
    private bool IsAttackMode = false;
    private float dirX, dirY;
    public float timeBetweenAttacks = 0.3f;

    [Inject]
    private void Construct(FileOperations data)
    {
        _data = data;
    }
    private void Start()
    {
        LoadHelperClasses();

        RigidbodyHero = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _heroMove.JoystickCoordinateUpdate(joystick, _heroCharacteristics.speed, out dirX, out dirY);
    }
    private void FixedUpdate()
    {
        RigidbodyHero.velocity = _heroMove.MotionVector(dirX, dirY);
        if (IsAttackMode)
        {
            if (transform.position.x > TrackedMutantTransform.position.x)
            {
                HeroBody.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                HeroBody.rotation = Quaternion.identity;

            }
            return;
        }

        if (RigidbodyHero.velocity.x < 0)
        {
            HeroBody.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            HeroBody.rotation = Quaternion.identity;
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
            var item = objectCollisionEventDetails.gameObject.GetComponent<InventoryItem>();
            inventory.PickUpItem(item);
            Destroy(objectCollisionEventDetails.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D objectCollisionEventDetails)
    {
        {
            var tag = objectCollisionEventDetails.transform.tag;
            if (tag == "NPC")
            {
                if (ListAttackMutants.Any(u => u == objectCollisionEventDetails)) return;
                ListAttackMutants.Add(objectCollisionEventDetails.transform);
                if (!IsAttackMode)
                {
                    IsAttackMode = true;
                    TrackedMutantTransform = ListAttackMutants[0];
                    OnCollisionHeroFieldWithEnemy?.Invoke();
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D objectCollisionEventDetails)
    {
        var tag = objectCollisionEventDetails.transform.tag;
        if (tag == "NPC")
        {
            if (TrackedMutantTransform != null)
            {
                RotateElbowAttackMode(TrackedMutantTransform);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D objectCollisionEventDetails)
    {
        var tag = objectCollisionEventDetails.transform.tag;
        if (tag != "NPC")
        {
            return;
        }
            ListAttackMutants.Remove(objectCollisionEventDetails.transform);
        if (ListAttackMutants.Count != 0)
        {
            TrackedMutantTransform = ListAttackMutants[0];
            return;
        }
        IsAttackMode = false;
        TrackedMutantTransform = null;
        OnCollisionHeroFieldWithEnemy?.Invoke();
        ElbowRotation.localEulerAngles = new Vector3(0, 0, 90);
    }
    private void OnDestroy()
    {
        if (OnCollisionHeroFieldWithEnemy != null)
            foreach (var d in OnCollisionHeroFieldWithEnemy.GetInvocationList())
                OnCollisionHeroFieldWithEnemy -= (d as Action);
    }
    public void Shoot()
    {
        if (TrackedMutantTransform == null)
        {
            Debug.Log("No mutant found during attack!");
            return;
        }
        RotateElbowAttackMode(TrackedMutantTransform);
        Attack();
    }
    public void Attack()
    {
        if (inventory.CountBullet == 0)
        {
            return;
        }

        if (IsShoot == false)
        {
            IsShoot = true;
            var mutant = TrackedMutantTransform.GetComponent<MutantAI>();
            mutant._mutantHealth.TakeDamage(_heroCharacteristics.damage);
            inventory.SubstractItem(inventory.slots[0].item, 1);
            IsShoot = false;
        }
    }
    private void RotateElbowAttackMode(Transform target)
    {
        var HeroPosition = transform.position;
        var TargetPosition = target.position;
        var direction = TargetPosition - HeroPosition;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ElbowRotation.rotation = Quaternion.Euler(0, 0, angle);

    }
    public void TakingHeroDamage(int damage)
    {
        _heroCharacteristics.health -= damage;
        _healthBar.UpdateFillAmount(damage);

        if (_heroCharacteristics.health <= 0)
        {
            //Die
            OnHeroDeath?.Invoke();
        }
    }
    private void LoadHelperClasses()
    {
        _heroCharacteristics = new HeroCharacteristics(transform);
        _data.LoadHeroCharacteristics(_heroCharacteristics);

        _heroMove = new HeroMove();
        _healthBar = new HealthBar(_heroCharacteristics.health, _heroCharacteristics.health, HealthBar);
    }

}
