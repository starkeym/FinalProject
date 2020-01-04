using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    // TODO: VFX
    public float damage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }    
    void OnTriggerEnter(Collider collider){
        if(collider.tag!="enemyBullet"||collider.tag!="healingBullet"||collider.tag!="shooterBullet"){
           Destroy(gameObject);

        }    
    }
}
