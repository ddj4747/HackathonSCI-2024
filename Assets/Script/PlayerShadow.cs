using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    public BoxCollider2D BodyCollider;

    public float TimeToDie;
    public bool GoDie = false;

    private void Awake()
    { 
        BodyCollider = GetComponent<BoxCollider2D>();
    }
    void Start()
    {
    }

    private void Update()
    {
        Do();
    }

    private void Do()
    {
        TimeToDie -= Time.deltaTime;

        if (TimeToDie <= 0)
        {
            Die();
        }
        else
        {
            GoDark();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void GoDark()
    {
        // Implementacja efektu ciemnoœci
    }
}
