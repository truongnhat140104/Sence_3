using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float dirX;
    float dirY, moveInput, moveSpeed = 3f, jumpforce = 6.2f, groundCheckRadius = 0.2f;
    bool isGrounded, canDoubleJump, onDamaged;
    public bool isDead = false, isOnPlatform;
    public bool ClimbingAllowed { get; set; }

    Vector2 checkpoint;
    Animator anim;
    public Rigidbody2D rb;
    public Rigidbody2D platformRb;
    new Collider2D collider;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public TextMeshProUGUI numberOfFistKits;

    public HP HP_Bar;

    public float recentHP, maxHP;
    int numberOfFistAid = 0;


    public float dashBoost;
    public float dashTime;
    bool isDashing = false, canDash = true;




    void Start()
    {
        numberOfFistKits.text = "0";
        checkpoint = transform.position;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();

        recentHP = maxHP;
        HP_Bar.updateHPBar(recentHP, maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }


        moveInput = Input.GetAxisRaw("Horizontal");
        dirX = moveInput * moveSpeed;

        /*** Check if player is grounded ***/
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        /*** Player double jump ***/
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
            canDoubleJump = true;
            rb.gravityScale = 1.5f;
        }
        else if (canDoubleJump && Input.GetButtonDown("Jump"))
        {
            Jump();
            canDoubleJump = false;
            rb.gravityScale = 1.5f;
        }


        /*** Player Climb ***/
        if (ClimbingAllowed)
        {
            dirY = Input.GetAxisRaw("Vertical") * moveSpeed * 1.5f;
        }

        /*** Player Dash ***/

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }




        SetAnimationState();


        HP_Bar.updateHPBar(recentHP, maxHP);

    }



    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }


        if (isOnPlatform)
        {
            rb.velocity = new Vector2(dirX+platformRb.velocity.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(dirX, rb.velocity.y);
        }


        /*** Player climb ***/
        if (ClimbingAllowed)
        {
            rb.isKinematic = true;
            rb.velocity = new Vector2(dirX, dirY);
        }
        else
        {
            rb.isKinematic = false;
            rb.velocity = new Vector2(dirX, rb.velocity.y);
        }


    }


    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce);
    }


    void SetAnimationState()
    {
        if (dirX == 0)
        {
            anim.SetBool("isMoving", false);
        }

        if (Mathf.Abs(dirX) == 3 && rb.velocity.y == 0)
        {
            anim.SetBool("isMoving", true);
        }

        if (dirY == 0 || isGrounded)
        {
            anim.SetBool("isClimbing", false);
        }

        if (Mathf.Abs(dirY) == 4.5 && rb.velocity.x == 0 )
        {
            anim.SetBool("isClimbing", true);
        }

        if (onDamaged)
        {
            anim.SetBool("isHurt", true);
        }

        if (!onDamaged)
        {
            anim.SetBool("isHurt", false);
        }


        if (isDead)
        {
            StartCoroutine(DeadAnimation(0.2f));
            StartCoroutine(Respawn(1.4f));
        }
    }


    public void OnDamaged(float Damage)
    {
        
        recentHP -= Damage;

        if (recentHP <= 0)
        {
            isDead = true;
        }
        onDamaged = true;
    }


    public void AddKits()
    {
        numberOfFistAid += 1;
        numberOfFistKits.text = numberOfFistAid.ToString(); 

    }

    public void Health()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            recentHP = maxHP;
            numberOfFistAid -= 1;
        }
    }

    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpoint = pos;
    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("EnemyBullet"))
        {
            onDamaged = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Traps"))
        {
            isDead = true;
        }
    }

    IEnumerator DeadAnimation(float seconds)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpforce + 0.8f);

        yield return new WaitForSeconds(seconds);
        collider.enabled = false;
        yield return new WaitForSeconds(seconds);
        rb.gravityScale = 50;

    }

    IEnumerator Respawn(float seconds)
    {
        recentHP = 300;
        isDead = false;

        yield return new WaitForSeconds(seconds);

        HP_Bar.updateHPBar(recentHP, maxHP);
        rb.velocity = new Vector2(0, 0);
        transform.position = checkpoint;
        collider.enabled = true;
    }



    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(dashBoost * moveInput, dirY);

        yield return new WaitForSeconds(dashTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(1);
        canDash = true;
    }






}
