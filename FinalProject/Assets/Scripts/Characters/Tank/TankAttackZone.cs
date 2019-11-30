using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttackZone : MonoBehaviour
{
    public GameObject OwnerofthisAttackZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if ( other.gameObject.tag == "MeleeEnemy")
        {
            if(Input.GetMouseButtonDown(1))
            {
                other.gameObject.GetComponent<MeleeEnemy>().health -= 20;
                OwnerofthisAttackZone.GetComponent<TankSC>().mana += 20;
                Debug.Log(other.gameObject.GetComponent<MeleeEnemy>().health);
            }
           
        }
        if (Input.GetMouseButtonDown(1) && other.gameObject.tag == "RangedEnemy")
        {
           
            other.gameObject.GetComponent<RangedEnemySC>().health -= 20;
            OwnerofthisAttackZone.GetComponent<TankSC>().mana += 20;
        }

    }
}
