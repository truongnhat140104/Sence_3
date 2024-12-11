using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    Animator anim;



    void Start()
    {
        anim = GetComponent<Animator>();

    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isTouching", true);
        }

        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Item picked");
            collision.SendMessageUpwards("AddBullet", gameObject.name);
            Destroy(gameObject);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("isTouching", false);
    }
}
