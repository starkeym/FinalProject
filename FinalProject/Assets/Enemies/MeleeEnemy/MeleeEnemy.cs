using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool hasArmor;

    public int health;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void takenDamage()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        ////Shooter///
        if (hasArmor == false && other.gameObject.tag=="ShooterBullet")
        {
            health -= 20;
            ShooterSC.mana += 10;
        }
        if (hasArmor == false && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 50;
            
        }
        if (hasArmor == true && other.gameObject.tag == "ShooterBullet")
        {
            health -= 10;
            ShooterSC.mana += 10;
        }
        if (hasArmor == true && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 25;
            
        }
        ////Shooter////

        ////Tank////
        if(hasArmor==false && other.gameObject.tag == "TankAttack")
        {
            health -= 20;
            TankSC.mana += 20;
           
        }
        if(hasArmor ==true && other.gameObject.tag =="TankAttack")
        {
            health -= 10;
            TankSC.mana += 20;
        }
        ////Tank////
        
        ////Healer////
        if(hasArmor ==false && other.gameObject.tag =="HealerBullet")
        {
            health -= 20;
            HealerSC.mana = 25;
        }
        if (hasArmor == true && other.gameObject.tag == "HealerBullet")
        {
            health -= 10;
            HealerSC.mana = 25;
        }
        if(hasArmor ==true && other.gameObject.tag =="HealerArmorBreaker")
        {
            hasArmor = false;
        }
        ////Healer////


    }
}
