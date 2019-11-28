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


    public GameObject ShooterBullet;
    public GameObject BulletPos;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
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

    void PoweredAttack()
    {
        //channel animation
        //Senna ult?
    }
}
