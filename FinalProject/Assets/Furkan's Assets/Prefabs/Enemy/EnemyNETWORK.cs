using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyNETWORK : NetworkBehaviour
{
    
    public Slider HelathbarPrefab;
    public Slider healthbar;

    [SyncVar]
    Vector3 realPosition = Vector3.zero;
    [SyncVar]
    Quaternion realRotation;
    private float updateInterval;

    public bool canPatrol=true;
    public bool canAttack=true;
    public bool canLookForPlayer=true;

    [SyncVar(hook = "CheckDeath")]
    public float health = 5;
    public float speed = 5;
    public float shootingRange;
    public float attackCooldown=1f;
    public float bulletSpeed=20;
    bool onCooldown=false;
    float viewRange= Mathf.Infinity;


    GameObject Target;
    public bool playerDetected = false;
    [SyncVar]
    bool hasArmor;
    public GameObject armorPrefab;
    public GameObject armor;


    public GameObject bullet;
    public GameObject forward;
    Vector3 Destination;
    Vector3 Direction1;
    Vector3 Direction2;


    GameObject[] players;
    NavMeshAgent agent;
    public LayerMask obsLayerMask;
    public LayerMask PlayerLayerMask;


    ///requirities;
    //2 obj as children for patrolling
    //empty gamobj for bulletspawn pos.
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
         
          if (isServer)
        {
            int rng=Random.Range(1,2);
            if(rng==1){
                hasArmor=true;
                armor = Instantiate(armorPrefab,gameObject.transform.position,Quaternion.identity);
                armor.transform.SetParent(gameObject.transform);
            }
            ////Getting patrol destinations.
            foreach (Transform tr in gameObject.transform){
                if(tr.name=="Direction1")
                {
                    Direction1=tr.position;
                }
                else if(tr.name=="Direction2")
                {
                    Direction2=tr.position;
                }
            }
            ////Choosing a random target
            //Target = players[Random.Range(0,players.Length)];
            agent = GetComponent<NavMeshAgent>();
            agent.speed=speed;
            if(canPatrol)agent.SetDestination(Direction1);
        }
        else{
            if(hasArmor){
                armor = Instantiate(armorPrefab,gameObject.transform.position,Quaternion.identity);
                armor.transform.SetParent(gameObject.transform);
            }
        }
        
        healthbar = Instantiate(HelathbarPrefab,Vector3.zero,Quaternion.identity)as Slider;
        healthbar.transform.SetParent(GameObject.Find("Canvas").transform);
        healthbar.value=health;

    }
    void Update()
    {
         if (isServer)
        {
            if (playerDetected)
            { 
                if(canAttack)attack(Target);
            }else
            {            
               if(canPatrol)patrol();
              if(canLookForPlayer)lookForPlayer();
            }
            updateInterval += Time.deltaTime;
            if (updateInterval > 0.06f) // 9 times per second
            {
                updateInterval = 0;
                realPosition = transform.position;
                realRotation = transform.rotation;
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
    void UpdatePlayersList(){
        players=GameObject.FindGameObjectsWithTag("Player");
    }
    void lookForPlayer(){
        if(players.Length==0){

            UpdatePlayersList();
            return;
            }
           
        foreach (var item in players)
        {

          Vector2 a = new Vector2(gameObject.transform.position.z - forward.transform.position.z, gameObject.transform.position.x - forward.transform.position.x);
          Vector2 b = new Vector2(gameObject.transform.position.z - item.transform.position.z, gameObject.transform.position.x - item.transform.position.x);
        
          float angle = Vector2.SignedAngle(b, a) + 90;
          
          if (angle>80&&angle<110)
          {

              RaycastHit hit;
              
              Vector3 dir =item.transform.position-gameObject.transform.position;
              dir.y=0;
              Debug.DrawRay(transform.position,dir);
              
              if(Physics.Raycast(transform.position,dir,out hit,viewRange,PlayerLayerMask))
              {

                  if (hit.collider.gameObject==item){
                     Target=item;
                     playerDetected=true;                  
                  }
              }
          }
        }

    }
    void patrol(){

        if(agent.destination.x==Direction1.x &&
         agent.destination.z==Direction1.z &&
        Vector2.Distance(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z),new Vector2(Direction1.x,Direction1.z))<=1)agent.SetDestination(Direction2);

        if(agent.destination.x==Direction2.x &&
         agent.destination.z==Direction2.z &&
        Vector2.Distance(new Vector2(gameObject.transform.position.x,gameObject.transform.position.z),new Vector2(Direction2.x,Direction2.z))<=1)agent.SetDestination(Direction1);
    }
    void attack(GameObject target) {
        gameObject.transform.LookAt(target.transform.position);
        if (Vector3.Distance(gameObject.transform.position,target.transform.position)>shootingRange||
        Physics.Raycast(transform.position,target.transform.position-gameObject.transform.position,Vector3.Distance(gameObject.transform.position,target.transform.position),obsLayerMask))
        {
            agent.isStopped=false;
            agent.SetDestination(target.transform.position);
            return;
        }else{
            agent.isStopped=true;
        } 

        ///yield shoot
        if (!onCooldown)
        {
            //network spawn?
            onCooldown=true;
            RpcShoot(target);
            GameObject localBullet = Instantiate(bullet,forward.transform.position,Quaternion.identity);
            localBullet.transform.LookAt(new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z));
            localBullet.GetComponent<Rigidbody>().velocity=localBullet.transform.forward * bulletSpeed;
            StartCoroutine(cooldownCountdown());
        }
    }
    [ClientRpc]
    void RpcShoot(GameObject ShootingTarget){
        if(isServer)return;
            GameObject localBullet = Instantiate(bullet,forward.transform.position,Quaternion.identity);
            localBullet.transform.LookAt(new Vector3(ShootingTarget.transform.position.x,transform.position.y,ShootingTarget.transform.position.z));
            localBullet.GetComponent<Rigidbody>().velocity=localBullet.transform.forward * bulletSpeed;
    }
    IEnumerator cooldownCountdown(){        
        yield return new WaitForSeconds(attackCooldown);
        onCooldown=false;
    }
    void CheckDeath(float updatedHealth)
    {
        health=updatedHealth;
        healthbar.value=health;
        if (updatedHealth<=0)
        {
            
            Destroy(healthbar.gameObject);
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider collider){
        if(!isServer)return;
        if(collider.gameObject.tag=="shooterBullet"){
            getDamage(collider.GetComponent<Bullet_Shooter>().damage);
        }else if(collider.gameObject.tag=="healingBullet"){
            getDamage(collider.GetComponent<Bullet_Healer>().damage);
        }
    }
    public void getDamage(float value){
        if(!isServer){return;}
        if(!playerDetected){
            getAgro();
        }
        if(hasArmor){
            value=value/2;
        }
        health+=value;
        healthbar.value=health;

    }
    public void getTaunted(GameObject taunter){
        if(isServer){
            playerDetected=true;
            Target=taunter;
        }
    }

    public void getAgro(){
        UpdatePlayersList();
        Target=players[Random.Range(0,players.Length)];
        playerDetected=true;
    }
    public void armorBreak(){
        if(isServer){
            hasArmor=false;
            Destroy(armor);
            RpcdestroyArmor();
        }
    }

     [ClientRpc]
     void RpcdestroyArmor(){
        if(isServer)return;

        hasArmor=false;
        Destroy(armor);
     }
}
