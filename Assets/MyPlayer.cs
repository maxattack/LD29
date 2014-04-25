using UnityEngine;
using System.Collections;


public class MyPlayer : CustomBehaviour 
{
    void Awake()
    {
        xform = transform;
    }

    public Vector2 Speed = Vec(0,0);

    public Transform bulletPrefab;

    Transform xform;

    void Update()
    {
        //Time.deltaTime



        var p = xform.position;

        if (Input.GetKeyDown(KeyCode.W))
            p.z += Speed.x * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.S))
            p.z -= Speed.x * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
            p.x -= Speed.y * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.D))
            p.x += Speed.y * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.B))
        {
            var bullet = Dup(bulletPrefab);
            bullet.position = xform.position;

        }

        xform.position = p;
    }


	
}
