using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   public GameObject OwnerofthisBullet;
   GameObject ShootedEnemy;

    public int Damage;
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
        if(other.gameObject.tag==OwnerofthisBullet.GetComponent<RangedEnemySC>().enemytag)
        {

            other.GetComponent<HealerSC>().health -= Damage;
            other.GetComponent<TankSC>().health -= Damage;
            other.GetComponent<ShooterSC>().health -= Damage;
            Destroy(gameObject);
        }
    }
}
