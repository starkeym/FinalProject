using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HealerNETWORK : NetworkBehaviour
{


    ///////////////MOVEMENT/////////////////////////
    public Vector3 CameraOffset;
    [SyncVar]
    Vector3 realPosition = Vector3.zero;
    [SyncVar]
    Quaternion realRotation;
    private float updateInterval;
    CharacterController cc;
    private Vector3 moveDirection = Vector3.zero;
    public bool CanMove = true;



    //////////////Properties///////////////////
    public Slider HelathbarPrefab;
    public Slider healthbar;
    public float speed = 5;
    [SyncVar (hook ="OnHealthChange")]
    public float health = 100;
    [SyncVar]
    public float mana = 0;



    ////////////////ANIMATON/////////////////////
    Animator an = new Animator();
    GameObject forward;


    ///////////////Shooting/////////////////////////
    
    public GameObject UltiBullet1;
    public GameObject UltiBullet2;

    [SyncVar]
    Vector3 ShootingRot;
    public float UltChannelDuration;
    public GameObject bulletSpawnPos;
    public GameObject bullet;
    public float bulletSpeed=50;
    public float ultBulletSpeed=80;



    /////////////////////Network Variables///////
    
    public SyncListFloat SynclayerWeight = new SyncListFloat();
    void OnIntChanged(SyncListFloat.Operation op, int index)
    {
        float[] localFloat = new float[5];
        if (SynclayerWeight.Count != 5)
        {
           // Debug.Log("asd");
            return;
        }
        for (int i = 0; i < 5; i++)
        {
           // Debug.Log("bsd");
            localFloat[i] = SynclayerWeight[i];
        }
        OnChangeLayerWeight(localFloat);
    }
    [SyncVar(hook ="OnChangeAnimationState")]
    public string SyncAnimState = ("idle");




    void Start()
    {
        gameObject.name="Healer";
        cc = gameObject.GetComponent<CharacterController>();
        an = gameObject.GetComponent<Animator>();
        forward = GameObject.Find("HealerForward");
        
        healthbar = Instantiate(HelathbarPrefab,Vector3.zero,Quaternion.identity)as Slider;
        healthbar.transform.SetParent(GameObject.Find("Canvas").transform);
        healthbar.value=health;
    }

    void Update()
    {
        
        SynclayerWeight.Callback=OnIntChanged;
        if (hasAuthority && CanMove)
        {
            Camera.main.transform.position=gameObject.transform.position+CameraOffset;
            
            movement();
         
            HandleAnimations();
         
            if (Input.GetMouseButtonDown(0))
            {
                ShootingRot=GetMousePosition();
                Attack();
            }
            if (Input.GetMouseButtonDown(1) && mana>=100)
            {
                //Ult Not Finished Yet
                ShootingRot=GetMousePosition();
                StartCoroutine(Ult());
            }
            updateInterval += Time.deltaTime;
            if (updateInterval > 0.06f) // 9 times per second
            {
                updateInterval = 0;
                CmdSync(transform.position, transform.rotation);
            }
        }else{
            
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.1f);
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
        moveDirection = new Vector3(-Input.GetAxis("Horizontal"), 0.0f,-Input.GetAxis("Vertical"));
        moveDirection *= speed;

        cc.Move(moveDirection * Time.deltaTime);
        if (Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d") || Input.GetKey("w"))
        {

            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }


    [Command]
    void CmdShoot(Vector3 pos,Vector3 rot)
    {
        //GameObject localBullet = Instantiate(bullet, bulletSpawnPos.transform.position, bulletSpawnPos.transform.rotation);
        //localBullet.GetComponent<Rigidbody>().velocity = bulletSpawnPos.transform.forward * bulletSpeed;
        //NetworkServer.Spawn(localBullet);
        //  Destroy(localBullet, 5.0f);
        RpcShoot(pos,rot);
    }
    [ClientRpc]
    void RpcShoot(Vector3 pos,Vector3 rot) {
        if (hasAuthority)return;
        
        GameObject localBullet = 
        /*Bullet*/   Instantiate(bullet,
        /*Position*/ pos,
        /*Rotation*/ Quaternion.identity);
        localBullet.transform.LookAt(rot);
        localBullet.GetComponent<Rigidbody>().velocity = localBullet.transform.forward * bulletSpeed;
        Destroy(localBullet,3f);

        }

    void Attack() {

        Vector3 pos=new Vector3( bulletSpawnPos.transform.position.x,gameObject.transform.position.y,bulletSpawnPos.transform.position.z);
        //Quaternion rot= bulletSpawnPos.transform.rotation;rot.x=0;rot.z=0;      

        GameObject localBullet = 
        /*Bullet*/   Instantiate(bullet,
        /*Position*/ pos,
        /*Rotation*/ Quaternion.identity);
        localBullet.transform.LookAt(ShootingRot);
        CmdShoot(pos,ShootingRot);
        localBullet.GetComponent<Rigidbody>().velocity = localBullet.transform.forward * bulletSpeed;
        Destroy(localBullet,3f);
    }


    [Command]
    void CmdUlt(Vector3 pos,Vector3 rot,bool first)
    {
        RpcUlt(pos,rot,first);
    }
    [ClientRpc]
    void RpcUlt(Vector3 pos,Vector3 rot,bool  first)
    {
        if (hasAuthority) return;
        mana=0;

        if(first){

          GameObject localBullet = 
        /*Bullet*/   Instantiate(UltiBullet1,
        /*Position*/ pos,
        /*Rotation*/ Quaternion.identity);
        localBullet.transform.LookAt(rot);
        localBullet.GetComponent<Rigidbody>().velocity = localBullet.transform.forward * ultBulletSpeed;
        Destroy(localBullet,3f);  
        
        }
        else{


        }
        
    }

    IEnumerator Ult()
    {
        //channel animation?
        CanMove=false;
        GameObject firstBullet = 
        /*Bullet*/   Instantiate(UltiBullet1,
        /*Position*/ bulletSpawnPos.transform.position,
        /*Rotation*/ Quaternion.identity);
        ShootingRot.y+=50;
        firstBullet.transform.LookAt(ShootingRot);
        CmdUlt(firstBullet.transform.position,ShootingRot,true);
        firstBullet.GetComponent<Rigidbody>().velocity=firstBullet.transform.forward * ultBulletSpeed;
        Destroy(firstBullet,0.5f);

        yield return new WaitForSeconds(UltChannelDuration);
        
        CanMove=true;
        Vector3 pos=new Vector3( bulletSpawnPos.transform.position.x,gameObject.transform.position.y,bulletSpawnPos.transform.position.z); 
        GameObject SecondBullet = 
        /*Bullet*/   Instantiate(UltiBullet2,
        /*Position*/ pos,
        /*Rotation*/ Quaternion.identity);
        SecondBullet.transform.LookAt(ShootingRot);
        CmdUlt(pos,ShootingRot,false);
        SecondBullet.GetComponent<Rigidbody>().velocity = -SecondBullet.transform.up * ultBulletSpeed;
        Destroy(SecondBullet,3f);
        mana -= 100;
    }



    void HandleAnimations() {
        // float angle = Mathf.Atan2(a, b) * Mathf.Rad2Deg;
        // Vector3 targetDir = test.mouseposition - transform.position;

        Vector2 a = new Vector2(gameObject.transform.position.z - forward.transform.position.z, gameObject.transform.position.x - forward.transform.position.x);
        Vector2 b = new Vector2(gameObject.transform.position.z - GetMousePosition().z, gameObject.transform.position.x - GetMousePosition().x);

        float angle = Vector2.SignedAngle(b, a) + 90;
        if (angle > 180 && angle < 270)
        {
            angle -= 360;
        }
        //Debug.Log(angle);

        ////Angle Chart Area 1
        if (angle > 0 && angle < 90)
        {
            float animationHardness = (angle * 1) / 90;
            an.SetLayerWeight(3, animationHardness);
            an.SetLayerWeight(2, (100 - animationHardness));

        }

        ////Angle Chart Area 2
        if (angle > 90 && angle < 180)
        {
            float localAngle = angle - 90;
            float animationHardness = (localAngle * 1) / 90;

            an.SetLayerWeight(4, animationHardness);

            an.SetLayerWeight(3, (100 - animationHardness));

        }

        ////Angle Chart Area 3
        if (angle > -180 && angle < -90)
        {
            float localAngle = angle + 180;
            float animationHardness = (localAngle * 1) / 90;

            an.SetLayerWeight(5, animationHardness);

            an.SetLayerWeight(4, (100 - animationHardness));

        }

        if (angle > -90 && angle < 0)
        {
            float localAngle = angle + 90;
            float animationHardness = (localAngle * 1) / 90;

            an.SetLayerWeight(2, animationHardness);

            an.SetLayerWeight(1, (100 - animationHardness));

            an.SetLayerWeight(5, 0);

            an.SetLayerWeight(4, 0);

            an.SetLayerWeight(3, 0);

        }
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

        float[] localFloat=new float[5];
        for (int i = 0; i < 5; i++)
        {
          localFloat[i]= an.GetLayerWeight(i+1);
        }

        CmdAnimationLayerWeight(localFloat);



    }


    //////////Updating on other clients that are  not us or server//////////////////
    void OnChangeLayerWeight(float[] anState) {
        if (hasAuthority)
        {
            return;
        }
        UpdateLayerWeight(anState);
        Debug.Log("1");
    }
    void OnChangeAnimationState(string state) {
        if (hasAuthority)
        {
            return;
        }
        UpdateAnimationState(state);
    }
    //////////Updating on the server//////////////////////////////////////////////////
    [Command]
    void CmdAnimationLayerWeight(float[] anState){
        UpdateLayerWeight(anState);
    }
    [Command]
    void CmdChangeAnimationState(string state) {
        UpdateAnimationState(state);
    }
    ////////////////////////////////////////////////////////////////////////////////

    void UpdateLayerWeight(float[] anState)
    {
        
            if (SynclayerWeight.Count!=5)
            {
                SynclayerWeight.Clear();
                for (int i = 0; i < 5; i++)
                {
                    SynclayerWeight.Add(anState[i]);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    SynclayerWeight[i] = anState[i];
                }
            }

        

        for (int i = 0; i < 5; i++)
        {
            an.SetLayerWeight(i+1,anState[i]);    
        }
    }
    void UpdateAnimationState(string state)
    {
        if (SyncAnimState == state) return;
        SyncAnimState = state;
        if (state == "walk")
        {
            an.SetBool("isWalking", true);
        }
        if (state=="idle")
        {
            an.SetBool("isWalking",false);
        }
    }


    public Vector3 GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z);
        }
        return Vector3.zero;
    }


    void OnHealthChange(float newHealt)
    {
        health=newHealt;
        healthbar.value=health;

        if (newHealt<=0)
        {
            //DeathAnimation
            healthbar.gameObject.SetActive(false);
            CanMove = false;
        }
    }
    public void changeMana(float changeAmount){
        if(hasAuthority)
        CmdchangeMana(changeAmount);
    }

    [Command]
    public void CmdchangeMana(float changeAmount){
        mana+=changeAmount;
    }

     [Command]
    public void CmdChangeHealth(float value) {
        health+=value;
        healthbar.value=health;

    }
    void OnTriggerEnter(Collider collision){
        if(hasAuthority){
            if(collision.gameObject.tag=="enemyBullet"){
                CmdChangeHealth(collision.gameObject.GetComponent<Bullet_Enemy>().damage);
                //Instansiate Damage Particle
            }
        }
        Destroy(collision.gameObject);
    }

}
