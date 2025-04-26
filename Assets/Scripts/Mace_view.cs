using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace_view : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float distance = 5f;

    private Vector3 startPos;
    private bool movingDown = true;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move() 
    {
        float moveDown = startPos.y - distance;
        float moveUp = startPos.y + distance;
        if (movingDown)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            if (transform.position.y <= moveDown)
            {
                movingDown = false;
            }
        }
        else
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
            if (transform.position.y >= moveUp)
            {
                movingDown = true;
            }
        }
    }
}
