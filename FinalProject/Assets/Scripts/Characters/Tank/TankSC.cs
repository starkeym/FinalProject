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
    public static float mana = 0;
    public float health = 250;
    public static float attackDamage;
    GameObject TauntCollider;
    ///Taunt////
    Rigidbody rg;
    float tauntSpeed;




    void Start()
    {
        TauntCollider = GameObject.FindGameObjectWithTag("Tauntcollider");
        rg = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        TauntCollider.SetActive(false);
    }

    void Update()
    {
        if (CanMove)
        {
            Movement();
        }
    }

    void Attack()
    {
        ///BurasıFurkan'ın slicer mekaniği ile yapması için ona bırakıldı.///
    }
    void Taunt()
    {
        if(mana >= 100 && Input.GetMouseButtonDown(0))
        {
            TauntCollider.SetActive(true);
            gameObject.GetComponent<Collider>().enabled = false;
            tauntSpeed = 4;
            rg.AddForce(MouseLook.pointToLook * tauntSpeed);
            StartCoroutine(TauntJumpCooldown());
            
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
        yield return new WaitForSeconds(1);
        tauntSpeed = 0;
        mana = 0;
        gameObject.GetComponent<Collider>().enabled = true;
        TauntCollider.SetActive(false);

    }
}
