using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float lifeTime = 8f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
