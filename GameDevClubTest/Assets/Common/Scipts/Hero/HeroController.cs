using Assets.Common.Scipts.Hero;
using System;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public InventoryController inventory;

    private HeroMove _heroMove;
    private HeroCollisionWithObjects _heroCollisionWithObjects;

    [HideInInspector]
    public Joystick joystick;

    private Rigidbody2D RigidbodyHero;

    #region Attack
    [HideInInspector]
    public Transform TrackedEnemyTransform;
    public event Action OnCollisionHeroFieldWithEnemy;
    #endregion

    private bool IsMoveLeft = false;
    private float dirX, dirY;
    public float speed = 3;

    private void Start()
    {
        _heroMove = new HeroMove();
        _heroCollisionWithObjects = new HeroCollisionWithObjects();

        RigidbodyHero = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _heroMove.JoystickCoordinateUpdate(joystick, speed, out dirX, out dirY);
    }
    private void FixedUpdate()
    {
        RigidbodyHero.velocity = _heroMove.MotionVector(dirX, dirY);
        if (RigidbodyHero.velocity.x < 0)
        {
            transform.rotation = GetQuaternion(180);
        }
        else
        {
            transform.rotation = GetQuaternion(0);
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
            return;
        }
    }
    private void OnTriggerEnter2D(Collider2D objectCollisionEventDetails)
    {
        var tag = objectCollisionEventDetails.transform.tag;
        if(tag=="NPC")
        {
            OnCollisionHeroFieldWithEnemy?.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D objectCollisionEventDetails)
    {
        var tag = objectCollisionEventDetails.transform.tag;
        if(tag=="NPC")
        {
            this.TrackedEnemyTransform = objectCollisionEventDetails.transform;
            OnCollisionHeroFieldWithEnemy?.Invoke();
        }
    }
    private void OnDestroy()
    {
        if (OnCollisionHeroFieldWithEnemy != null)
            foreach (var d in OnCollisionHeroFieldWithEnemy.GetInvocationList())
                OnCollisionHeroFieldWithEnemy -= (d as Action);
    }
    private Quaternion GetQuaternion(int corner)
    {
        Quaternion rot = transform.rotation;
        rot.y = corner;
        return rot;

    }

}
