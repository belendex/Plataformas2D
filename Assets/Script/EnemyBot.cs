using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBot : MonoBehaviour
{
    public float speed;
    public Vector3[] positions;
    public SpriteRenderer spriteBot;
    public float waitTime;
    public float waitShoot;
    public bool waiting;
    public Animator animatorBot;
    public float visionRange;
    public Player player;
    public GameObject bulletEnemy;
    public int lifes;
    public bool alive;
    public GameObject explosion;

    private int index;
    private bool canShot;
    private bool reloading;
    private int amountShoot;

    void Start()
    {
        canShot = true;
        alive = true;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.alive == false || alive == false) return;

        if (CanSeePlayer() == false)
        {
            amountShoot = 0;
            Patrol();
        }
        else
        {
            LookAtPlayer();
            Shoot();
        }
    }


    public void Shoot()
    {
        if (amountShoot < 3)
        {
            if (canShot == true)
            {
                amountShoot++;

                Vector3 positionBullet = Vector3.zero;
                if (spriteBot.flipX == true)
                {
                    positionBullet = transform.position + new Vector3(0.17f, 0.015f);
                }
                else
                {
                    positionBullet = transform.position + new Vector3(-0.17f, 0.015f);
                }


                GameObject bala = Instantiate(bulletEnemy, positionBullet, transform.rotation);
                bala.GetComponent<SpriteRenderer>().flipX = !spriteBot.flipX;
                StartCoroutine(WaitShoot());
            }
        }
        else
        {
            if (reloading == false)
            {
                StartCoroutine(WaitReload());
            }
        }




    }


    public IEnumerator WaitReload()
    {

        reloading = true;
        yield return new WaitForSeconds(2);
        reloading = false;
        amountShoot = 0;
    }


    public IEnumerator WaitShoot()
    {
        canShot = false;

        yield return new WaitForSeconds(waitShoot);

        canShot = true;
    }


    public bool CanSeePlayer()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= visionRange;
    }


    public void LookAtPlayer()
    {
        animatorBot.SetBool("Walk", false);
        spriteBot.flipX = transform.position.x < player.transform.position.x;
    }


    public void Patrol()
    {
        float distance = Vector2.Distance(transform.position, positions[index]);
        // normalized : conseguir la direccion SIN la magnitud del vector, es decir 
        // consigo si tengo que ir al sur, norte, sin la distancia

        Vector3 direction = (positions[index] - transform.position).normalized;

        if (distance <= 0.01f)
        {
            if (!waiting)
            {
                StartCoroutine(Wait());
            }
        }
        else
        {
            transform.position += direction * (speed * Time.deltaTime);
            animatorBot.SetBool("Walk", true);

            spriteBot.flipX = transform.position.x < positions[index].x;
        }
    }


    public void SetNewTarget()
    {
        index++;
        if (index >= positions.Length)
        {
            index = 0;
        }
    }


    public IEnumerator Wait()
    {
        waiting = true;
        animatorBot.SetBool("Walk", false);
        yield return new WaitForSeconds(waitTime);
        SetNewTarget();
        waiting = false;
    }


    public void TakeLife(int damage)
    {
        lifes -= damage;
        if (lifes <= 0)
        {
            alive = false;
            GameObject explo = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(explo, 0.65f);
            Destroy(gameObject);
        }
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var item in positions)
        {
            Gizmos.DrawSphere(item, 0.1f);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

    }
}
