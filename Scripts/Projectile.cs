using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 Direction;
    public TileType Type;

    public float speed = 10f;
    public float timeToLive = 5f;
    public float damage = 5f;
    public Team firedBy;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + Direction * speed * Time.deltaTime;
        timeToLive -= Time.deltaTime;

        if (timeToLive <= 0) Destroy(this.gameObject);
    }
}