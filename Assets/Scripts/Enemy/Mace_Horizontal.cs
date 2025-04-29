using System.Collections;
using UnityEngine;

public class Mace_Horizontal : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] GameObject spawnPointRight;
    [SerializeField] GameObject spawnPointLeft;

    public bool isRight = true;

    void Update()
    {
        Move();
        DestroyMace();
    }

    private void Move() {
        float moveDirection = isRight ? 1f : -1f;
        transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);
    }

    public void ResetMace(bool moveRight)
    {
        isRight = moveRight;
    }

    private void DestroyMace()
    {
        if (spawnPointRight.transform.position.x < transform.position.x && isRight ||
            spawnPointLeft.transform.position.x > transform.position.x && !isRight)
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

    public float Speed
    {
        get => speed;
        set => speed = Mathf.Min(value, 100f);
    }

}
