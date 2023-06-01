using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{

    private HeroController _heroController;

    public float dumping = 1.5f;
    public Vector2 offset = new Vector2(2f,1f);
    

    [Inject]
    public void Construct(HeroController heroController)
    {
        _heroController = heroController;
    }

    private void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x),offset.y);
    }
    private void Update()
    {
        
    }
}
