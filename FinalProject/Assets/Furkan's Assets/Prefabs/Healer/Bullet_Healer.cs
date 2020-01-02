using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Bullet_Healer : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObj = collision.gameObject;

            if (collidedObj.tag=="Enemy")
            {
                collidedObj.GetComponent<EnemyTestNW>().health -= damage;
            }



            else if (collidedObj.tag=="Player")
            { 
                 switch (collidedObj.name)
                 {
                     case "Tank":
                         collidedObj.GetComponent<TankNETWORK>().health += damage;
                         break;
                     case "Shooter":
                         collidedObj.GetComponent<ShooterNETWORK>().health += damage;
                         break;
                 }
            }
    }
}
