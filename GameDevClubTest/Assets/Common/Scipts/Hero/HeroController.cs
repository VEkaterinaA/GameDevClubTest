using UnityEngine;
using UnityEngine.EventSystems;

public class HeroController : MonoBehaviour
{
    [HideInInspector]
    public Joystick joystick;

    private Rigidbody2D RigidbodyHero;

    private Vector2 move;

    private const float speed = 2;

    private void Start()
    {
        RigidbodyHero = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        move.x = joystick.Horizontal * speed ;
        move.y = joystick.Vertical * speed;

    }
    private void FixedUpdate()
    {
        //RigidbodyHero.velocity = new Vector2(dirX, dirY);
        RigidbodyHero.MovePosition(RigidbodyHero.position + move * speed * Time.deltaTime);
    }

}
