using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum SpawnSide { Top, Bottom, Left, Right }

    [SerializeField] private float speed = 1f;
    private Vector2 moveDirection;

    public void Initialize(SpawnSide side)
    {
        switch (side)
        {
            case SpawnSide.Top:
                moveDirection = Vector2.down;
                break;
            case SpawnSide.Bottom:
                moveDirection = Vector2.up;
                break;
            case SpawnSide.Left:
                moveDirection = Vector2.right;
                break;
            case SpawnSide.Right:
                moveDirection = Vector2.left;
                break;
            default:
                moveDirection = Vector2.zero;
                break;
        }
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }

    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0f, 100f);
    }
}
