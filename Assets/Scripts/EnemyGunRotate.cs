using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunRotate : MonoBehaviour
{
    public GameObject player;
    public Transform gunRotate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
        playerPos.x = Mathf.Abs(playerPos.x - gunPos.x);                        // If player in the left or right the x position can be positive or negative
        playerPos.y = playerPos.y - gunPos.y;
        float gunangle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        if (Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(player.transform.position)).x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -gunangle));
            gunRotate.rotation = Quaternion.Euler(new Vector3(0f, 180f, gunangle));
        }
        else
        {
            
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunangle));
            gunRotate.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunangle));

        }
    }
}
