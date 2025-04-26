using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mace_random : MonoBehaviour
{
    [SerializeField] private float moveSpeed_slow = 2f;
    [SerializeField] private float moveSpeed_fast = 4f;
    [SerializeField] private float distance = 2f;
    
    private Vector3 startPos; 
    private bool movingDown = true;
    private float currentSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        PickRandomSpeed(); 
    }
    
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= startPos.y - distance)
        {
            movingDown = false;
            PickRandomSpeed(); 
        }
        else if (transform.position.y >= startPos.y + distance)
        {
            movingDown = true;
            PickRandomSpeed(); 
        }
        
        Move(currentSpeed);
    }
    
    void PickRandomSpeed()
    {
        int randomNumber = Random.Range(0, 2); 
        if (randomNumber == 0)
        {
            currentSpeed = moveSpeed_slow;
        }
        else
        {
            currentSpeed = moveSpeed_fast;
        }
    }
    
    void Move(float speed)
    {
        if (movingDown)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }
}