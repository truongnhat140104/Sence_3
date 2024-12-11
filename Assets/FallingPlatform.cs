using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallDelay = 1f;
    private float destroyDelay = 2f;
    private Vector3 pos;

    [SerializeField] private Rigidbody2D rb;

    private void Start()
    {
        pos = gameObject.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StartCoroutine(Rebuild());
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private IEnumerator Rebuild()
    {
        yield return new WaitForSeconds(1f);
        gameObject.transform.position = pos;

        rb.bodyType = RigidbodyType2D.Static;
    }
}
