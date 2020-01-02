
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_ShooterBullet : MonoBehaviour
{
    //public GameObject StartParticle;
    //public GameObject explosionParticle;
    void Start()
    {
        Destroy(gameObject, 5.0f);
        //GameObject particle = Instantiate(StartParticle, gameObject.transform.position,Quaternion.identity) ;
        //Destroy(particle,1);

    }
    void OnTriggerEnter(Collider collider){
        Destroy(gameObject);
    }
    
}
