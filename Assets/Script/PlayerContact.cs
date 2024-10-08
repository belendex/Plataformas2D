using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContact : MonoBehaviour
{
    public Player player;

    public void Start()
    {
        player = GetComponent<Player>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            player.TakeDamage(1);
         
            Destroy(collision.gameObject);//destruime la bala!
        }
    }
}
