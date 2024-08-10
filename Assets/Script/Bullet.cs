using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public SpriteRenderer bulletSprite;
    public GameObject explosion;

    public void Start()
    {
        GameObject explo = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(explo, 0.1f);
    }
    void Update()
    {
        if (bulletSprite.flipX == true)
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }
}
