using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxItem : MonoBehaviour
{
    Animator anim;
    Collider2D col;
    int random;




    void Start()
    {
        anim = GetComponent<Animator>();
        random = Random.Range(0,100);  
        col = GetComponent<Collider2D>();
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isTouching", true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("isTaken", true);
            anim.SetBool("isTouching", false);
            switch (random % 2 == 0)
            {
                case true:
                    collision.SendMessageUpwards("AddKits");
                    break;

                case false:
                    collision.SendMessageUpwards("AddBullet", "ShotgunBullet");
                    break;
            }
            col.enabled = false;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("isTouching", false);
    }
}
