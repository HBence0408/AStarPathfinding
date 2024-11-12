using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private int speed;
    [SerializeField] private int maxSpeed;
    [SerializeField] private GameObject projectile;

    // Update is called once per frame
    void Update()
    {
        Input.GetAxisRaw("Horizontal");
        Input.GetAxisRaw("Vertical");
        Vector2 mv = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            mv += new Vector2(0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            mv += new Vector2(0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            mv += new Vector2(-1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            mv += new Vector2(1, 0);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        mv.Normalize();

        //rigid.AddForce(mv*speed);
        rigid.velocity = mv * speed;

        if (rigid.velocity.magnitude > maxSpeed)
        {
            
        }
    }

    private void Shoot()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = new Vector3(mousePos.x, mousePos.y,0) - this.transform.position; 
        

        dir.Normalize();
        GameObject go = Instantiate(projectile, this.transform.position, this.transform.rotation);
        go.GetComponent<Projectile>().SetUp(dir,10);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
      //  Gizmos.DrawSphere(Camera.main.ScreenToViewportPoint(
    }
}
