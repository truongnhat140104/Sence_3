using UnityEngine;

public class FinishPoint : MonoBehaviour
{

    [SerializeField] string levelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneController.instance.NextLevel(levelName);
        }
    }
}
