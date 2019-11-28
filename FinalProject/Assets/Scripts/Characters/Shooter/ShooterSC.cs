﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSC : MonoBehaviour
{
    ///////////////MOVEMENT/////////////////////////
    CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    public float speed;
    public bool CanMove = true;
    ///////////////////////////////////////////////
    ///ATTACK///
    public static float attackDamage;
    public static float ultiDamage;
    public static float mana = 0;
    public static float health = 150;
    

    public GameObject ShooterBullet;
    public GameObject BulletPos;
    public GameObject UltiBullet;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(mana >=100)
        {
            mana = 100;
        }
        if (CanMove)
        {
            Movement();
        }
    }

    void Movement()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        moveDirection *= speed;

        characterController.Move(moveDirection * Time.deltaTime);
        if (Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("w"))
        {

            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    void Attack()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(ShooterBullet, BulletPos.transform.position, Quaternion.identity);
        }
    }

    void Ulti()
    {
        //channel animation
        if(mana >= 100 && Input.GetKeyDown("r"))
        {
            Instantiate(UltiBullet, BulletPos.transform.position, Quaternion.identity);

        }
            
    }
}