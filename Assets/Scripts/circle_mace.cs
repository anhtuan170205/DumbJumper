using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle_mace : MonoBehaviour
{
    public float moveSpeed = 2f;         
    public float radius = 5f;            
    private Vector2 centerPosition;      
    private Vector2 moveDirection;       

    void Start()
    {
        centerPosition = transform.position;
        ChooseNewDirection();
    }

    void Update()
    {
        MoveEnemy();

        if (Vector2.Distance(transform.position, centerPosition) > radius)
        {
            // Đẩy ngược vào trong
            Vector2 toCenter = (centerPosition - (Vector2)transform.position).normalized;
            moveDirection = (moveDirection + toCenter).normalized;
        }
    }

    private void MoveEnemy()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void ChooseNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(ChooseNewDirection), 2f, 3f);  // lần đầu 2 giây, sau 3 giây 
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    // Vẽ bán kính giới hạn
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? centerPosition : transform.position, radius);
    }
}
