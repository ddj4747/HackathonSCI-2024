using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private string nextLevel;

    [SerializeField]
    private GameObject player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == "Player")
        {
            SceneManager.LoadSceneAsync(nextLevel);
        }
    }
}
