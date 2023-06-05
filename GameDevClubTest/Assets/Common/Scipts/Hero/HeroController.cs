using Assets.Common.Scipts.Hero;
using Assets.Common.Scipts.HeroInventory;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class HeroController : MonoBehaviour
{
    public InventoryController inventory;

    private HeroMove _heroMove;
    private HeroCollisionWithObjects _heroCollisionWithObjects;

    [HideInInspector]
    public Joystick joystick;

    private Rigidbody2D RigidbodyHero;

    #region Attack
    public Transform TrackedEnemyTransform;
    public event Action OnCollisionHeroFieldWithEnemy;
    #endregion

    private float dirX,dirY;
    public float speed = 3;

    private void Start()
    {
        _heroMove = new HeroMove();
        _heroCollisionWithObjects = new HeroCollisionWithObjects();

        RigidbodyHero = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _heroMove.JoystickCoordinateUpdate(joystick,speed,out dirX,out dirY);
    }
    private void FixedUpdate()
    {
        RigidbodyHero.velocity = _heroMove.MotionVector(dirX,dirY);
    }

    public void CollisionHeroFieldWithEnemy(Transform TrackedEnemyTransform)
    {
        OnCollisionHeroFieldWithEnemy?.Invoke();
    }
    public void CollisionHeroFieldWithEnemyExit()
    {
        OnCollisionHeroFieldWithEnemy?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D objectCollisionEventDetails)
    {
        _heroCollisionWithObjects.MoveItemToInventory(objectCollisionEventDetails, inventory.slots);
        Destroy(objectCollisionEventDetails.gameObject);

    }
    private void OnDestroy()
    {
        if (OnCollisionHeroFieldWithEnemy != null)
            foreach (var d in OnCollisionHeroFieldWithEnemy.GetInvocationList())
                OnCollisionHeroFieldWithEnemy -= (d as Action);
    }

}
