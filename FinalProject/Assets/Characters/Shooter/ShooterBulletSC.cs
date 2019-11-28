using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBulletSC : MonoBehaviour
{
    Rigidbody rg;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rg.AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Bullet()
    {
        
    }
}
