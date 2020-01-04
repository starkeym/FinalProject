using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collider){

        if(collider.gameObject.name=="RangedEnemy"){
            collider.gameObject.GetComponent<EnemyNETWORK>().getTaunted(GameObject.Find("Tank"));
        }
    }
}
