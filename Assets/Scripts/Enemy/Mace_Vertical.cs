using System.Collections;
using UnityEngine;

public class Mace_Vertical : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] GameObject spawnPointTop;
    [SerializeField] GameObject spawnPointBottom;

    public bool isDown = true;

    void Update()
    {
        Move();
        DestroyMace();
    }

    private void Move() 
    {
        float moveDirection = isDown ? 1f : -1f;
        transform.Translate(Vector2.down * moveDirection * moveSpeed * Time.deltaTime);
    }

    public void ResetMace(bool moveDown)
    {
        isDown = moveDown;
    }

    private void DestroyMace()
    {
        if (spawnPointTop.transform.position.y < transform.position.y && !isDown ||
            spawnPointBottom.transform.position.y > transform.position.y && isDown)
        {
            gameObject.SetActive(false);
            MaceSpawn.Instance?.OnMaceDisabled();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            Debug.Log("Enemy hit sword");
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy hit player");
        }
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Min(value, 100f); // safety limit
    }

}
