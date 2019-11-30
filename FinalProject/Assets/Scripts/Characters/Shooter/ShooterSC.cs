using System.Collections;
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
    public float health = 150;
    float timer = 0;
    

    public GameObject ShooterBullet;
    public GameObject UltiBullet;


    void Start()
    {
        mana = 100;
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
        Attack();
        Ulti();
        Debug.Log(mana);
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
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
        }
        if (Input.GetMouseButtonDown(1) && timer <= 1)
        {
            Instantiate(ShooterBullet, gameObject.transform.position, Quaternion.identity);
        }
    }

    void Ulti()
    {
        //channel animation
        if(mana >= 100 && Input.GetMouseButtonDown(0))
        {
            Instantiate(UltiBullet, gameObject.transform.position, Quaternion.identity);
            mana = 0;
        }
            
    }
   
}
