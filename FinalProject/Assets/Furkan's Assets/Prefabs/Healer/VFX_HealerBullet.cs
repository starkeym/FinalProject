using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_ : MonoBehaviour
{
    
    
    public GameObject StartParticle;
    public GameObject explosionParticle;
    public GameObject healParticle;
    void Start()
    {
        Destroy(gameObject, 5.0f);
        //GameObject particle = Instantiate(StartParticle, gameObject.transform.position, Quaternion.identity);
        //Destroy(particle, 1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            //GameObject particle = Instantiate(healParticle, gameObject.transform.position, Quaternion.identity);
            // Destroy(particle, 1);
            Destroy(gameObject);
        }

        //GameObject particle2 = Instantiate(explosionParticle, gameObject.transform.position, Quaternion.identity);
        //Destroy(particle2, 1);
        Destroy(gameObject);
    }
}
