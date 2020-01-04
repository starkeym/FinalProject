using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet_Healer : MonoBehaviour
{
    public float damage=-3;
    public float ManaGain=5;


    private void OnTriggerEnter(Collider collision)
    {
       if(collision.gameObject.tag=="RangedEnemy"||collision.gameObject.tag=="MeleeEnemy"){
          GameObject.Find("Healer").GetComponent<HealerNETWORK>().changeMana(ManaGain);
        }
        Destroy(gameObject);

            
    }
}
