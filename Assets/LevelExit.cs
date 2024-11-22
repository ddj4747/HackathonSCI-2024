using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private string nextLevel;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == "Player")
        {
            
        }
    }
}
