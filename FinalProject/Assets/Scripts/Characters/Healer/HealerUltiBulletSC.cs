using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerUltiBulletSC : MonoBehaviour
{
    Rigidbody rg;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
        gameObject.transform.LookAt(MouseLook.pointToLook);
        rg.AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        
    }
}