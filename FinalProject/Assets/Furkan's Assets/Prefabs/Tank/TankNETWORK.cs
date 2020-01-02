using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TankNETWORK : NetworkBehaviour
{
    ///////////////MOVEMENT/////////////////////////
        [SyncVar]
    Vector3 realPosition = Vector3.zero;
    [SyncVar]
    Quaternion realRotation;
        private float updateInterval;

    CharacterController cc;
    private Vector3 moveDirection = Vector3.zero;
    bool CanMove = true;
    bool CastingUlt;

    //////////////stats///////////////////
    public Slider HelathbarPrefab;
    public Slider healthbar;
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
    GameObject slicerArea;
    public GameObject PlanePos;
    public LayerMask m_LayerMaskEnemies;
    public LayerMask m_LayerMaskObstacles;
    Vector3 ultStartPoint;
    Vector3 ultDestination;
    bool m_Started;




    ////////////NETWORK VARIABLES////////////////
    [SyncVar(hook = "OnChangeAnimationState")]
    public string SyncAnimState = ("idle");

    override public void OnStartAuthority(){
         slicerArea = Instantiate(SlicerPlane);
         cc = gameObject.GetComponent<CharacterController>();
        }
    void Start()
    {
        
        healthbar = Instantiate(HelathbarPrefab,Vector3.zero,Quaternion.identity)as Slider;
        healthbar.transform.SetParent(GameObject.Find("Canvas").transform);
        
        an = gameObject.GetComponent<Animator>();
        m_Started = true;
    }



    void Update()
    {
        Debug.Log(CanMove);
        Debug.DrawRay(gameObject.transform.position, GetMousePosition() - gameObject.transform.position);
        if (hasAuthority&&CanMove)
        {
            movement();
            Attack();
            //handleAnimations();
            
            updateInterval += Time.deltaTime;
            if (updateInterval > 0.11f) // 9 times per second
            {
             updateInterval = 0;
             CmdSync(transform.position, transform.rotation);
            }
        }
        if(!hasAuthority){          
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
        }
          if (CastingUlt)
        {
             Ult();
        }
        
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
        bool onScreen= screenPoint.z>0&&screenPoint.x>0&&screenPoint.x<1&&screenPoint.y>0&&screenPoint.y<1;
        if(onScreen){
            Vector3 pos=Camera.main.WorldToScreenPoint(transform.position);
            pos += new Vector3(0,25,0);
            healthbar.transform.position=pos;
        }else{
            healthbar.transform.position=new Vector3(-1000,-1000,0);
        }

           
    }
    
        [Command]
    void CmdSync(Vector3 position, Quaternion rotation)
    {
        realPosition = position;
        realRotation = rotation;
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (m_Started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(slicerArea.transform.position, slicerArea.transform.localScale*8);
    }
    void Attack()
    {
        slicerArea.transform.position=PlanePos.transform.position;
        slicerArea.transform.rotation=PlanePos.transform.rotation;
        Collider[] hitColliders = Physics.OverlapBox(slicerArea.transform.position, slicerArea.transform.localScale*4, SlicerPlane.transform.rotation,m_LayerMaskEnemies);
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
                Vector3 localDestination =GetMousePosition();
                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position,localDestination-gameObject.transform.position,out hit,Mathf.Infinity,m_LayerMaskObstacles))
                {
                    localDestination = hit.point;
                }
                ultStartPoint = gameObject.transform.position;
                ultDestination = localDestination;
                


                cc.detectCollisions = false;
                CanMove = false;
                CastingUlt = true;
                mana -= 100;
            }
            else
            {
               //Message that says your ult is not ready yet;
            }
        }
    }
    void Ult()
    {
        Debug.Log("1");
        if (Vector3.Distance(ultStartPoint,gameObject.transform.position)>=ultRange)
        {
            cc.detectCollisions = true;
            CastingUlt = false;
            CanMove = true;

            return;
        }
        if (Vector3.Distance(gameObject.transform.position,ultDestination)>2f)
        {
            transform.position = Vector3.MoveTowards(transform.position,ultDestination,ultSpeed*Time.deltaTime);
        }
        else
        {
                    Debug.Log("3");

            cc.detectCollisions = true;
            CastingUlt = false;

            CanMove = true;
        }
    }

    void OnHealthChange(float newHealt)
    {
        health=newHealt;
        if (health <= 0)
        {
            Destroy(gameObject);
            //DeathAnimation
            //CanMove = false;
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
    
    
    [Command]
    public void CmdChangeHealth(float value) {
        health+=value;
    }
    void OnTriggerEnter(Collider collision){
        if(hasAuthority){
            if(collision.gameObject.tag=="enemyBullet"){
                CmdChangeHealth(collision.gameObject.GetComponent<Bullet_Enemy>().damage);
            }else if(collision.gameObject.tag=="healingBullet"){
                CmdChangeHealth(collision.gameObject.GetComponent<Bullet_Healer>().damage);

            }
        }
    }
}
