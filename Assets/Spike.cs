using Unity.VisualScripting;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObject = collision.gameObject;

        if (gameObject.tag == "Player")
        {
            gameObject.ConvertTo<Player>().Die();
        }
        else if (gameObject.tag == "destuctable_object")
        {

        }
    }
}
