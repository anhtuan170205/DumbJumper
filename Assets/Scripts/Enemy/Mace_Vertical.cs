using UnityEngine;

public class Mace_Vertical : MonoBehaviour
{
    [Header("Speed and Distance Control")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float offset = 1f;

    [Header("Wave Movement")]
    [SerializeField] private bool useWaveMovement = false;
    [SerializeField] private float waveAmplitude = 0.5f; // biên độ

    [Header("Spawn Points")]
    [SerializeField] GameObject spawnPointTop;
    [SerializeField] GameObject spawnPointBottom;

    public bool isDown = true;
    private float startX;
    private bool curveRight = true; // cong

    public float MoveSpeed {
        get => moveSpeed;
        set => moveSpeed = Mathf.Min(value, 100f);
    }

    void OnEnable() {
        startX = transform.position.x;
    }

    void Update() {
        Move();
        DestroyMace();
    }

    private void Move() {
        float direction = isDown ? -1f : 1f;
        Vector3 newPos = transform.position;
        newPos.y += direction * moveSpeed * Time.deltaTime;

        if (useWaveMovement)
        {
            // Cong theo trục X
            float traveledY = Mathf.Abs(transform.position.y - transform.parent.position.y);
            float curve = Mathf.Sin((traveledY / 5f) * Mathf.PI) * waveAmplitude;
            newPos.x = startX + (curveRight ? curve : -curve);
        }

        transform.position = newPos;
    }

    public void ResetMace(bool moveDown, bool useWave, float amplitude) {
        isDown = moveDown;
        useWaveMovement = useWave;
        waveAmplitude = amplitude;
        curveRight = Random.value > 0.5f;
    }


    private void DestroyMace() {
        if (spawnPointTop.transform.position.y + offset < transform.position.y && !isDown ||
            spawnPointBottom.transform.position.y - offset > transform.position.y && isDown)
        {
            gameObject.SetActive(false);
            MaceSpawn.Instance?.OnMaceDisabled();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
            Debug.Log("Collision");
    }
}
