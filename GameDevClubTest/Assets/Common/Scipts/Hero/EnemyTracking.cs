using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyTracking : MonoBehaviour
{
    private HeroController _heroController;

    [Inject]
    private void Construct(HeroController heroController)
    {
        _heroController = heroController;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        _heroController.CollisionHeroFieldWithEnemy(collision.transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        _heroController.CollisionHeroFieldWithEnemyExit();
    }
}
