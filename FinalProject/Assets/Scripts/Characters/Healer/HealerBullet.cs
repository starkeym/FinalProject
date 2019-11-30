using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerBullet : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rg;
    public float speed;

    GameObject hero;
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
        if(other.gameObject.tag =="Shooter")
        {
           
            hero = GameObject.FindGameObjectWithTag("Shooter");
            hero.GetComponent<ShooterSC>().health += 20;
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Tank")
        {
            hero = GameObject.FindGameObjectWithTag("Tank");
            hero.GetComponent<TankSC>().health += 20;
            Destroy(gameObject);
        }
    }
}
