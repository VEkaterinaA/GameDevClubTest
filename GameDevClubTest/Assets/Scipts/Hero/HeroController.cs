using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [HideInInspector]
    public Joystick joystick;
    
    private Rigidbody2D RigidbodyPlayer;

    public float dirX, dirY;
    public float speed;

    private void Start()
    {
        RigidbodyPlayer = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        dirX = joystick.Horizontal * speed;
        dirY = joystick.Vertical * speed;
    }

    void FixedUpdate()
    {
        RigidbodyPlayer.velocity = new Vector2(dirX,dirY);
    }
}
