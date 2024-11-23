using Unity.VisualScripting;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField]
    private Sprite pressedSprite;

    [SerializeField]
    private Sprite unpressedSprite;

    [SerializeField]
    private GameObject Doors;


    private Collider2D collider;
    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        collider = GetComponent<Collider2D>();    
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        int layerMask = ~(1 << gameObject.layer);

        RaycastHit2D hit = Physics2D.BoxCast(
            collider.bounds.center,
            collider.bounds.size,
            0f,                      
            Vector2.up,              
            0.1f,                      
            layerMask                
        );


        if (hit)
        {
            spriteRenderer.sprite = pressedSprite;
            Doors.gameObject.ConvertTo<Doors>().open = true;
        }
        else
        {
            spriteRenderer.sprite = unpressedSprite;
            Doors.gameObject.ConvertTo<Doors>().open = false;
        }


    }
}
