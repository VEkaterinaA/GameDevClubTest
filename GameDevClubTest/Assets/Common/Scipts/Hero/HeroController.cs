using Assets.Common.Scipts;
using Assets.Common.Scipts.CommonForHeroAndMutantsScripts;
using Assets.Common.Scipts.Hero;
using Assets.Common.Scipts.Hero.HelperClasses;
using Assets.Common.Scipts.HeroInventory;
using Assets.Common.Scipts.Weapon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HeroController : MonoBehaviour
{
    //Classes
    private HeroMove _heroMove;
    private HealthBar _healthBar;
    private FileOperations _data;
    public HeroWeapon _heroWeapon;
    public InventoryController inventory;
    public CreateBullet _createBullet;
    [HideInInspector]
    public HeroCharacteristics _heroCharacteristics;

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

    [Inject]
    private void Construct(FileOperations data, HeroWeapon heroWeapon)
    {
        _data = data;
        _heroWeapon = heroWeapon;
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
        if (_heroWeapon.CountBullet == 0)
        {
            return;
        }

        if (IsShoot == false)
        {
            IsShoot = true;
            _heroWeapon.SubstractBullet();
            _createBullet.LoadBulletPrefab();
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
        _heroCharacteristics = new HeroCharacteristics(transform, HealthBar);
        _data.LoadHeroCharacteristics(_heroCharacteristics);
        _heroCharacteristics.InitHealth();
        _heroMove = new HeroMove();
    }

}
