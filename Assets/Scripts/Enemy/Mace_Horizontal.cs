using UnityEngine;

public class Mace_Horizontal : MonoBehaviour
{
    [Header("Speed and Distance Control")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float offset = 0.5f;

    [Header("Wave Movement")]
    [SerializeField] private bool useWaveMovement = false;
    [SerializeField] private float waveAmplitude = 0.5f; // Biên độ

    [Header("Spawn Points")]
    [SerializeField] GameObject spawnPointRight;
    [SerializeField] GameObject spawnPointLeft;

    public bool isRight = true;
    private float startY;
    private bool curveUp = true; // Cong 

    public float Speed {
        get => speed;
        set => speed = Mathf.Min(value, 100f);
    }

    void OnEnable() {
        startY = transform.position.y;
    }

    void Update() {
        Move();
        DestroyMace();
    }

    private void Move() {
        float direction = isRight ? 1f : -1f;
        Vector3 newPos = transform.position;
        newPos.x += direction * speed * Time.deltaTime;

        if (useWaveMovement)
        {
            // Cong theo trục y
            float traveledX = Mathf.Abs(transform.position.x - transform.parent.position.x);
            float curve = Mathf.Sin((traveledX / 5f) * Mathf.PI) * waveAmplitude; 
            newPos.y = startY + (curveUp ? curve : -curve);
        }

        transform.position = newPos;
    }

    public void ResetMace(bool moveRight, bool useWave, float amplitude) {
        isRight = moveRight;
        useWaveMovement = useWave;
        waveAmplitude = amplitude;
        curveUp = Random.value > 0.5f;
    }

    private void DestroyMace() {
        if (spawnPointRight.transform.position.x + offset < transform.position.x && isRight ||
            spawnPointLeft.transform.position.x - offset > transform.position.x && !isRight)
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
