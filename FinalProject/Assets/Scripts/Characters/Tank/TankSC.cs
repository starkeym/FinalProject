using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSC : MonoBehaviour
{
    ///////////////MOVEMENT/////////////////////////
    CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    public float speed;
    public bool CanMove = true;
    ///////////////////////////////////////////////
    public float mana = 0;
    public float health = 250;
    public static float attackDamage;
    GameObject TauntCollider;
    GameObject Atkzone;
    ///Taunt////
    Rigidbody rg;
    public float tauntSpeed;




    void Start()
    {
        TauntCollider = GameObject.FindGameObjectWithTag("TAUNT");
        rg = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        
        Atkzone = GameObject.FindGameObjectWithTag("Tankatkzone");
    }

    void Update()
    {
        if (CanMove)
        {
            Movement();
        }
        Taunt();
    }

    void Attack()
    {
        
    }
    void Taunt()
    {
        if(mana >= 100 && Input.GetMouseButtonDown(0))
        {
            TauntCollider.SetActive(true);
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            speed = 10;
            Atkzone.GetComponent<BoxCollider>().enabled = false;
            
            
            
            //StartCoroutine(TauntJumpCooldown());
            
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
    IEnumerator TauntJumpCooldown()
    {
        yield return new WaitForSeconds(3);
        tauntSpeed = 0;
        mana = 0;
        Atkzone.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;
       
        TauntCollider.SetActive(false);

    }
}
