using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ult_Bullet_Shooter : MonoBehaviour
{
    // Start is called before the first frame update
    public float damage;
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider col){
         if (col.gameObject.tag=="Enemy")
        {
            col.gameObject.GetComponent<EnemyTestNW>().health -= damage;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
