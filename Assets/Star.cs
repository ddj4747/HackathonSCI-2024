using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    [System.Serializable]
    public enum StarState
    {
        Empty,
        Half,
        Filled
    }

    [SerializeField]
    private Sprite emptyStarSprite;

    [SerializeField]
    private Sprite halfStarSprit;

    [SerializeField]
    private Sprite filledStarSprit;

    [SerializeField]
    private StarState starState;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetState(StarState state)
    {
        starState = state;

        if (starState == StarState.Empty)
        {
            spriteRenderer.sprite = emptyStarSprite;
        }
        else
        {
            spriteRenderer.sprite = filledStarSprit;
        }
    }

    public StarState GetState()
    {
        return starState;
    }
}
