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
    [SyncVar(hook = "OnHealthChange")]

    public float health = 100;
    public float mana = 100;
    public float damage = 5;
    public float ultRange=50;
    public float ultSpeed=500;

    ////////////////ANIMATON/////////////////////
    Animator an = new Animator();



    ////////////////ATTACKING/////////////////////
    public GameObject SlicerPlane;
    public GameObject slicerArea;
    public LayerMask m_LayerMaskEnemies;
    public LayerMask m_LayerMaskObstacles;
    Vector3 ultStartPoint;
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
        Debug.DrawRay(gameObject.transform.position, GetMousePosition() - gameObject.transform.position);
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
        if (Input.GetMouseButtonDown(0))
        {
            if (hitColliders.Length==0)
            {
                return;
            }
            foreach (var item in hitColliders)
            {
                item.GetComponent<EnemyTestNW>().health -= damage;
            }
        }
        if (Input.GetKeyDown("f"))
        {
            if (mana>=100f)
            {
                Debug.Log("hi");

                Vector3 localDestination =GetMousePosition();
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position,localDestination-gameObject.transform.position,out hit,ultRange,m_LayerMaskObstacles))
                {
                    localDestination = hit.point;
                }
                ultStartPoint = gameObject.transform.position;
                ultDestination = localDestination;



                cc.detectCollisions = false;
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
        if (Vector3.Distance(ultStartPoint,gameObject.transform.position)>=ultRange)
        {
            cc.detectCollisions = true;
            CastingUlt = false;
            CanMove = true;
            return;
        }
        if (Vector3.Distance(gameObject.transform.position,ultDestination)>0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position,ultDestination,ultSpeed*Time.deltaTime);
        }
        else
        {
            cc.detectCollisions = true;
            CanMove = true;
            CastingUlt = false;
        }
    }

    void OnHealthChange(float newHealt)
    {
        if (newHealt > health)
        {
            //healParticle
        }
        else
        {
            //damageParticle
        }
        if (health <= 0)
        {
            //DeathAnimation
            CanMove = false;
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
