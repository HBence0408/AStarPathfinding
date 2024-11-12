using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 dir;
    private int speed = 10;
    private float damage;

    public void SetUp(Vector3 dir, float damage)
    {
        this.dir = dir;
        this.damage = damage;
    }

    private void Update()
    {
        this.transform.position += dir * speed * Time.deltaTime;    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

}
