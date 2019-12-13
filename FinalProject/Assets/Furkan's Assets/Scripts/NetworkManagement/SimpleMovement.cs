using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleMovement : NetworkBehaviour
{

    ///////////////MOVEMENT/////////////////////////
    private Vector3 moveDirection = Vector3.zero;
    public float speed = 5;
    public bool CanMove = true;
    ///////////////////////////////////////////////

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            movement();
        }
    }

    void movement() {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        moveDirection *= speed;

        GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
        if (Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("w"))
        {

           transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }
    
}
