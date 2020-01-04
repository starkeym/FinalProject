using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyAreas : NetworkBehaviour
{
    public LayerMask enemiesLayer;
    Collider[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        enemies= Physics.OverlapBox(gameObject.transform.position,gameObject.transform.localScale/2,Quaternion.identity,enemiesLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isServer)return;
        
        foreach(Collider item in enemies){

            if(item.GetComponent<EnemyNETWORK>().playerDetected){
            
                foreach(Collider angryItem in enemies){
                    if(angryItem!=item){
                        angryItem.GetComponent<EnemyNETWORK>().getAgro();
                    }
                }
                Destroy(gameObject);
            }
        }   
    }
}
