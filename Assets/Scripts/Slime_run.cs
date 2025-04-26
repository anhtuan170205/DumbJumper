using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_run : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 3f;
    
    private Vector3 startPos;
    private bool movingRight = true;
    
    private bool isDead = false;
    
    private Animator anim; 
    
    void Awake()
    {
        anim = GetComponent<Animator>(); 
    }
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        if (isDead) return;
        Move();
    }
    
    private void Move()
    {
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;
        
        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
    }
    
    private void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") && !isDead)
        {
            Die();
        }
        else if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            Debug.Log("Slime hit player");
        }
    }
    
    private void Die()
    {
        isDead = true;
        anim.SetBool("Die", true); // Fixed variable name from anime to anim
        
        // Start coroutine to delay destruction
        StartCoroutine(DestroyAfterAnimation());
    }
    
    private IEnumerator DestroyAfterAnimation()
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(1f);
        
        // Destroy the game object
        Destroy(gameObject);
    }
}