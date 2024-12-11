using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public float speed;
    Vector3 targetPos;

    PlayerMove playerMove;
    Rigidbody2D rb;
    Vector3 moveDirection;

    Rigidbody2D playerRb;

    public GameObject ways;
    public Transform[] wayPoints;
    int pointIndex;
    int pointCount;
    int direction = 1;
    public float waitDuration;
    public float distance;


    private void Awake()
    {
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        wayPoints = new Transform[ways.transform.childCount];
        for (int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pointIndex = 1;
        pointCount = wayPoints.Length;
        targetPos = wayPoints[1].transform.position;
        DirectionCalculate();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector2.Distance(transform.position, targetPos) + "hii");
        if (Vector2.Distance(transform.position, targetPos) < distance)
        {
            NextPoint();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;

    }

    void NextPoint()
    {
        transform.position = targetPos;
        moveDirection = Vector3.zero;

        if (pointIndex == pointCount - 1)
        {
            direction = -1;
        }
        if (pointIndex == 0)
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPos = wayPoints[pointIndex].transform.position;

        StartCoroutine(WaitNextPoint());
        //DirectionCalculate();
    }

    IEnumerator WaitNextPoint()
    {
        yield return new WaitForSeconds(waitDuration);
        DirectionCalculate();
    }

    void DirectionCalculate()
    {
        moveDirection = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMove.isOnPlatform = true;
            playerMove.platformRb = rb;

            //playerRb.gravityScale = playerRb.gravityScale * 50;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerMove.isOnPlatform = false;
 
        }
    }
}
