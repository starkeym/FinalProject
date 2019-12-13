using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TankNETWORK : NetworkBehaviour
{
    ///////////////MOVEMENT/////////////////////////
    CharacterController cc;
    private Vector3 moveDirection = Vector3.zero;
    bool CanMove = true;
    bool CastingUlt;

    //////////////stats///////////////////
    public float speed = 5;
    public float health = 100;
    public float mana = 0;
    public float damage = 5;
    public float ultRange;

    ////////////////ANIMATON/////////////////////
    Animator an = new Animator();



    ////////////////ATTACKING/////////////////////
    public GameObject SlicerPlane;
    public GameObject slicerArea;
    public LayerMask m_LayerMaskEnemies;
    public LayerMask m_LayerMaskObstacles;

    Vector3 ultDestination;




    ////////////NETWORK VARIABLES////////////////
    [SyncVar(hook = "OnChangeAnimationState")]
    public string SyncAnimState = ("idle");

    
    void Start()
    {
        slicerArea = Instantiate(SlicerPlane);
        NetworkServer.Spawn(slicerArea);
        cc = gameObject.GetComponent<CharacterController>();
        an = gameObject.GetComponent<Animator>();
    }



    void Update()
    {
        if (hasAuthority&&CanMove)
        {
            movement();
            Attack();
            //handleAnimations();
        }
        if (CastingUlt)
        {
            Ult();
        }
    }
    void movement()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        moveDirection *= speed;

        cc.Move(moveDirection * Time.deltaTime);
        if (Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("w"))
        {

            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    void Attack()
    {

        Collider[] hitColliders = Physics.OverlapBox(slicerArea.transform.position, slicerArea.transform.localScale, Quaternion.identity, m_LayerMaskEnemies);
        Debug.Log(hitColliders[0].gameObject.name);
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var item in hitColliders)
            {
                item.GetComponent<EnemyTestNW>().health -= damage;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (mana>=100)
            {
                Vector3 localDestination =GetMousePosition();
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position,localDestination-gameObject.transform.position,out hit,ultRange,m_LayerMaskObstacles))
                {
                    localDestination = hit.point;
                }

                CanMove = false;
                CastingUlt = true;
                mana = 0;
            }
            else
            {
               //Message that says your ult is not ready yet;
            }
        }
    }
    void Ult()
    {
        if (true)
        {

        }
        else
        {
            CanMove = true;
            CastingUlt = false;
        }
    }
    
    void handleAnimations() {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            an.SetBool("isWalking", true);
            CmdChangeAnimationState("walk");
        }
        else
        {
            an.SetBool("isWalking", false);
            CmdChangeAnimationState("idle");
        }
        if (Input.GetMouseButtonDown(0))
        {
            an.SetBool("isAttacking",true);
            CmdChangeAnimationState("attack");

        }
        if (Input.GetMouseButtonUp(0))
        {
            an.SetBool("isAttacking",false);
            CmdChangeAnimationState("idle");
        }
    }


    void OnChangeAnimationState(string state)
    {
        if (hasAuthority)
        {
            return;
        }
        UpdateAnimationState(state);
    }

    [Command]
    void CmdChangeAnimationState(string state)
    {
        UpdateAnimationState(state);
    }

    void UpdateAnimationState(string state)
    {
        if (SyncAnimState == state) return;
        SyncAnimState = state;
        if (state == "walk")
        {
            an.SetBool("isWalking", true);
        }
        if (state == "idle")
        {
            an.SetBool("isWalking", false);
        }
        if (state=="attack")
        {
            an.SetBool("isWalking", false);
            an.SetBool("isAttacking", true);
        }
    }

    public Vector3 GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(hit.point.x,gameObject.transform.position.y,hit.point.z);
        }
        return Vector3.zero;
    }
}
