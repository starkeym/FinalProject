﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Shooter : MonoBehaviour
{
  public float damage=-5;
  public float ManaGain=5;



  void Start (){
    Destroy(gameObject, 5.0f);
  }
  void OnTriggerEnter(Collider collision){
    if(collision.gameObject.tag=="RangedEnemy"||collision.gameObject.tag=="MeleeEnemy"){
      GameObject.Find("Shooter").GetComponent<ShooterNETWORK>().changeMana(ManaGain);
    }
    if(collision.gameObject.name!="Tank"&&collision.gameObject.name!="Shooter")Destroy(gameObject);
  }
}
