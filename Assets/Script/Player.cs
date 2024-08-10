using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidad;
    public float jumpForce;
    public int vidas;
    public SpriteRenderer spritePlayer;
    public Animator animatorPlayer;
    public Rigidbody2D rigidbodyPlayer;
    public GameObject smoke, sparks;
    [Header("Salto")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask layerGround;
    public GameObject bulletPlayer;
    public bool alive;
    private float direccion;

    public void Start()
    {
        UiManager.Instance.UpdateHearts(vidas);
        alive = true;
    }
    public void Update()
    {
        if (alive == false) return;
        Movimiento();
        Jump();
        Shoot();
    }

    public void Movimiento()
    {
        direccion = Input.GetAxisRaw("Horizontal");
        transform.position += transform.right * (direccion * velocidad * Time.deltaTime);
        if (direccion != 0)
        {
            spritePlayer.flipX = direccion < 0;
        }
        animatorPlayer.SetBool("Run", direccion != 0);
        if (Input.GetKeyDown(KeyCode.F))
        {
            Dash(spritePlayer.flipX);
        }
    }

    public void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckGround())
            {
                rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpForce);
            }
        }
        animatorPlayer.SetFloat("Falling", rigidbodyPlayer.velocity.y);
    }

    public bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, layerGround);
    }

    public void Dash(bool flipX)
    {
        float direccionDash = 0;
        if (flipX == true)
        {
            direccionDash = -1;
            GameObject spark = Instantiate(sparks, transform.position, transform.rotation);
            spark.transform.rotation = Quaternion.Euler(0, 0, -90);
            Destroy(spark, 0.4f);
        }
        else
        {
            direccionDash = 1;
            GameObject spark = Instantiate(sparks, transform.position, transform.rotation);
            spark.transform.rotation = Quaternion.Euler(0, 0, 90);
            Destroy(spark, 0.4f);
        }

        transform.position += transform.right * direccionDash * 1;
        GameObject smokeGO = Instantiate(smoke, transform.position, transform.rotation);
        Destroy(smokeGO, 0.4f);
    }

    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animatorPlayer.SetBool("Shooting", true);
            Vector3 positionBullet = Vector3.zero;
            if (spritePlayer.flipX == true)
            {
                positionBullet = transform.position + new Vector3(-0.3f, 0.03f, 0);
            }
            else
            {
                positionBullet = transform.position + new Vector3(0.3f, 0.03f, 0);
            }
            GameObject bala = Instantiate(bulletPlayer, positionBullet, transform.rotation);
            bala.GetComponent<SpriteRenderer>().flipX = spritePlayer.flipX;
            Invoke("CancelShoot", 0.3f);
        }
    }

    public void CancelShoot()
    {
        animatorPlayer.SetBool("Shooting", false);
    }


    public void TakeDamage(int damage)
    {
        vidas -= damage;
        if (vidas <= 0)
        {
            alive = false;
            vidas = 0;
        }
        UiManager.Instance.UpdateHearts(vidas);
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }


}
