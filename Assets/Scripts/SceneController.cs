using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }



    IEnumerator LoadLevel(string levelName)
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(levelName);
        transitionAnim.SetTrigger("Start");
    }
}
