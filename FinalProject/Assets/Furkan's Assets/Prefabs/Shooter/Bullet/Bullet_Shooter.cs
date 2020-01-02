using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shooter : MonoBehaviour
{
    public float damage=-5;

  void OnCollisionEnter(Collision collision){
    if(collision.gameObject.tag=="Obs"){
        //instantiate hit particle
        Destroy(gameObject);

    }
  }
}
