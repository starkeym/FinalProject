using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
   public GameObject OwnerofthisBullet;
   GameObject ShootedEnemy;
   Rigidbody rg;
   public float speed;

    public int Damage;
    // Start is called before the first frame update
    void Start()
    {
       
        rg = GetComponent<Rigidbody>();
        rg.AddForce(OwnerofthisBullet.GetComponent<RangedEnemySC>().SelectedEnemy.transform.position * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag=="Tank" || other.gameObject.tag =="Shooter" || other.gameObject.tag =="Healer")
        {
            if(other.gameObject.tag=="Healer")
            {
               other.GetComponent<HealerSC>().health -= Damage;
                Destroy(gameObject);
            }
            if(other.gameObject.tag=="Tank")
            {
               other.GetComponent<TankSC>().health -= Damage;
                Destroy(gameObject);
            }
            if(other.gameObject.tag=="Shooter")
            {
                other.GetComponent<ShooterSC>().health -= Damage;
                Destroy(gameObject);
            }
            
           
        }
    }
}
