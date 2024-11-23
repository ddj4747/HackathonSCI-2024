using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private GameObject scorePopOut;

    [SerializeField]
    private GameObject player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == "Player")
        {
            scorePopOut.SetActive(true);
            player.SetActive(false);
        }
    }
}
