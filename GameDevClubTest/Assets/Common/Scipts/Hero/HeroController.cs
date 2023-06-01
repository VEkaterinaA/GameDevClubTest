using Assets.Common.Scipts.Hero;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class HeroController : MonoBehaviour
{
    private HeroMove _heroMove;

    [HideInInspector]
    public Joystick joystick;

    private Rigidbody2D RigidbodyHero;

    private float dirX,dirY;

    private const float speed = 3;

    [Inject]
    public void Construct(HeroMove heroMove)
    {
        _heroMove = heroMove;
    }
    private void Start()
    {
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

}
