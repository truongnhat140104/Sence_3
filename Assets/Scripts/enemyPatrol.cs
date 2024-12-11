using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    public Transform player; // Reference to the player object
    private bool onDamaged, isPlayerDetected = false, isDead = false; // Flag to check if the player is detected

    GameObject dustShootEffect;
    public GameObject pointA, pointB, gun, bullet, dustShoot;

    private Rigidbody2D rb;
    private Animator anim;

    public Transform startBullet;
    private Transform currentPoint;
    Vector3 localScale;

    float hp = 100;
    public float speed;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isMoving", true);
    }


    void Update()
    {
        timer += Time.deltaTime;

        // Automatically move
        if (!isPlayerDetected && !onDamaged)
        {
            autoMoving();
        }



        if (isPlayerDetected)
        {
            detectedAnimation();
        }



        if (!isDead && !isPlayerDetected)
        {
            anim.SetBool("isMoving", true);
            gun.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        // Animation when get hurt
        if (onDamaged)
        {
            detectedAnimation();
            anim.SetBool("isHurt", true);
        }

        if (!onDamaged)
        {
            anim.SetBool("isHurt", false);
        }

        // Animation when dead
        if (isDead)
        {
            StartCoroutine(DeadAnimation(1.5f));
        }

    }




    public void OnDamaged(float Damage)
    {
        onDamaged = true;
        isPlayerDetected = true;

        hp -= Damage;
        Debug.Log("enemy" + hp);
        if (hp <= 0)
        {
            isDead = true;
        }
    }

    // Detecting Player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            isPlayerDetected = true;
            (gameObject.transform.GetChild(1).GetComponent("EnemyGunRotate") as MonoBehaviour).enabled = true;
        }

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            onDamaged = false;
        }

        if (collision.CompareTag("Player"))
        {
            rb.isKinematic = true;
            isPlayerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            isPlayerDetected = false;
            (gameObject.transform.GetChild(1).GetComponent("EnemyGunRotate") as MonoBehaviour).enabled = false;
        }
    }

    private void flip()
    {
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            if (localScale.x < 0)
            {
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }

        if (player.transform.position.x < gameObject.transform.position.x)
        {
            if (localScale.x > 0)
            {
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }
    }


    private void detectedAnimation()
    {
        flip();

        if (timer > 1f)
        {
            timer = 0;
            shooting();
            dustShootEffect = Instantiate(dustShoot, gun.transform.GetChild(2).transform.position, gun.transform.GetChild(2).transform.rotation);
            Destroy(dustShootEffect, 1f);
        }

        Destroy(dustShootEffect, 1f);
        rb.velocity = new Vector2(0, 0);
        anim.SetBool("isMoving", false);
    }

    private void autoMoving()
    {
        Vector3 localScale = transform.localScale;

        // Moving from A -> B & B -> A
        if (currentPoint == pointB.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }


        // Rotate when reach edge point
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            localScale.x *= -1;
            transform.localScale = localScale;

            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            localScale.x *= -1;
            transform.localScale = localScale;

            currentPoint = pointB.transform;
        }



        // Back to movement line after being damaged
        if (currentPoint == pointA.transform && localScale.x > 0)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        if (currentPoint == pointB.transform && localScale.x < 0)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }


    void shooting()
    {
        GameObject shoot = Instantiate(bullet, gun.transform.GetChild(0).position, gun.transform.GetChild(0).rotation);
        Destroy(shoot, 3f);
    }


    IEnumerator DeadAnimation(float seconds)
    {
        anim.SetBool("isDead", true);
        gun.SetActive(false);
        isPlayerDetected = false;

        yield return new WaitForSeconds(seconds);

        gameObject.SetActive(false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }


}



