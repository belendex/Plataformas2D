using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotContact : MonoBehaviour
{
    public EnemyBot enemy;

    public void Start()
    {
        enemy = GetComponent<EnemyBot>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            enemy.TakeLife(1);

            Destroy(collision.gameObject);//destruime la bala!
        }
    }
}
