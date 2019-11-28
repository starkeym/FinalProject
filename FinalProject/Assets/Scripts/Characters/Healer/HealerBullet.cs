using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBullet : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rg;
    public float speed;
    void Start()
    {
        gameObject.transform.LookAt(MouseLook.pointToLook);
        rg.AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Shooter")
        {
            ShooterSC.health += 20;
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Tank")
        {
            TankSC.health += 20;
            Destroy(gameObject);
        }
    }
}
