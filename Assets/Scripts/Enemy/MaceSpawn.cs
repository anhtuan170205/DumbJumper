using System.Collections.Generic;
using UnityEngine;

public class MaceSpawn : MonoBehaviour
{
    public static MaceSpawn Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject verticalMacePrefab;
    [SerializeField] private GameObject horizontalMacePrefab;

    [Header("Spawn Points")]
    [SerializeField] private GameObject TopMaceSpawnPoint;
    [SerializeField] private GameObject BottomMaceSpawnPoint;
    [SerializeField] private GameObject LeftMaceSpawnPoint;
    [SerializeField] private GameObject RightMaceSpawnPoint;

    [Header("Spawn Range")]
    [SerializeField] private float spawnDistance_x = 2f;
    [SerializeField] private float spawnDistance_y = 2f;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxActiveMace = 5;

    [Header("Speed Control")]
    [SerializeField] private float speedIncreaseInterval = 10f;
    [SerializeField] private float speedStep = 0.5f;
    [SerializeField] private float maxSpeed = 5f;


    private List<GameObject> verticalPool = new List<GameObject>();
    private List<GameObject> horizontalPool = new List<GameObject>();

    private float spawnTimer = 0f;
    private int currentActiveMace = 0;
    private float speedTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        CreatePool();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        speedTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && currentActiveMace < maxActiveMace)
        {
            SpawnMaceFromPool();
            spawnTimer = 0f;
        }

        if (speedTimer >= speedIncreaseInterval)
        {
            IncreaseAllMaceSpeed();
            speedTimer = 0f;
        }
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var v = Instantiate(verticalMacePrefab);
            v.SetActive(false);
            v.transform.SetParent(transform);
            verticalPool.Add(v);

            var h = Instantiate(horizontalMacePrefab);
            h.SetActive(false);
            h.transform.SetParent(transform);
            horizontalPool.Add(h);
        }
    }

    private void SpawnMaceFromPool()
    {
        int random = Random.Range(0, 4);

        if ((random == 0 || random == 1) && TryGetFromPool(verticalPool, out var mace))
        {
            if (random == 0) // Top
            {
                mace.transform.position = new Vector3(RandomX(TopMaceSpawnPoint), TopMaceSpawnPoint.transform.position.y, 0);
                if (mace.TryGetComponent<Mace_Vertical>(out var vert))
                    vert.ResetMace(true);
            }
            else // Bottom
            {
                mace.transform.position = new Vector3(RandomX(BottomMaceSpawnPoint), BottomMaceSpawnPoint.transform.position.y, 0);
                if (mace.TryGetComponent<Mace_Vertical>(out var vert))
                    vert.ResetMace(false);
            }

            mace.SetActive(true);
            currentActiveMace++;
        }
        else if ((random == 2 || random == 3) && TryGetFromPool(horizontalPool, out var maceH))
        {
            if (random == 2) // Left
            {
                maceH.transform.position = new Vector3(LeftMaceSpawnPoint.transform.position.x, RandomY(LeftMaceSpawnPoint), 0);
                if (maceH.TryGetComponent<Mace_Horizontal>(out var hor))
                    hor.ResetMace(true);
            }
            else // Right
            {
                maceH.transform.position = new Vector3(RightMaceSpawnPoint.transform.position.x, RandomY(RightMaceSpawnPoint), 0);
                if (maceH.TryGetComponent<Mace_Horizontal>(out var hor))
                    hor.ResetMace(false);
            }

            maceH.SetActive(true);
            currentActiveMace++;
        }
    }

    private float RandomX(GameObject point)
    {
        return Random.Range(point.transform.position.x - spawnDistance_x, point.transform.position.x + spawnDistance_x);
    }

    private float RandomY(GameObject point)
    {
        return Random.Range(point.transform.position.y - spawnDistance_y, point.transform.position.y + spawnDistance_y);
    }

    private bool TryGetFromPool(List<GameObject> pool, out GameObject result)
    {
        foreach (var obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                result = obj;
                return true;
            }
        }
        result = null;
        return false;
    }

    public void OnMaceDisabled()
    {
        currentActiveMace--;
    }

    private void IncreaseAllMaceSpeed()
    {
        foreach (var mace in verticalPool)
        {
            if (mace.TryGetComponent<Mace_Vertical>(out var vert))
            {
                if (vert.MoveSpeed < maxSpeed)
                    vert.MoveSpeed += speedStep;
            }
        }

        foreach (var mace in horizontalPool)
        {
            if (mace.TryGetComponent<Mace_Horizontal>(out var hor))
            {
                if (hor.Speed < maxSpeed)
                    hor.Speed += speedStep;
            }
        }
    }

}
