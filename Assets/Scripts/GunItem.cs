
using UnityEngine;

public class GunItem : MonoBehaviour
{
    public GameObject gunholder;
    public Transform pos;
    Animator anim;
    
    

    void Start()
    {
        (gameObject.GetComponent("GunRotate") as MonoBehaviour).enabled = false;
        anim = GetComponent<Animator>();


        // Change animation not as default animation
        anim.SetBool("isTouching", true);
        anim.SetBool("isPicked", true);

    }

    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("isPicked", true);
            
            // Add rotated script into gun object
            (gameObject.GetComponent("GunRotate") as MonoBehaviour).enabled = true;

            // Change position when picked
            gameObject.transform.position = new Vector3(pos.position.x, pos.position.y, 0);

            // Transform to be child of gun holder
            gameObject.transform.parent = gunholder.transform;


            (gameObject.GetComponent("GunItem") as MonoBehaviour).enabled = false;


        }
        
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isTouching", true);
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("isTouching", false);
        anim.SetBool("isPicked", false);

    }
}
