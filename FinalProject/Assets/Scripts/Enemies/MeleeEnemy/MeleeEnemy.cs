using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    //Armor//
    public bool hasArmor;
    public Material Armored;
    public Material RegularMaterial;
    //Armor//
    public int health;


    /// heroes///
    public GameObject Tank;
    
    //enemyselection///
    int enemyselection;
    int priority;
    public string enemytag = "Shooter";
    public GameObject AttackZone;

    //enemyselection//
    ///EnemyAttack///
    public bool inAttackZone = false;
    public int AttackCooldown;
    GameObject SelectedEnemy;

    ///EnemyAttack///
    ///Taunt///
    public GameObject Taunt;
    public float nekadartauntlukalacak;
    public static NavMeshAgent agent;
    bool isAlive = true;
    bool tauntedbytank = false;

    void Start()
    {
        enemytag = "Shooter";
        SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);

        agent = GetComponent<NavMeshAgent>();
        enemyselection = Random.Range(1, 3);
        if (enemyselection == 1)
        {
            
            enemytag = "Healer";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            agent.SetDestination(SelectedEnemy.transform.position);
            
        }
        if (enemyselection == 2)
        {
            
            enemytag = "Shooter";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            agent.SetDestination(SelectedEnemy.transform.position);
            
            
        }
        if (enemyselection == 3)
        {
            
            enemytag = "Tank";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            agent.SetDestination(SelectedEnemy.transform.position); 
            
        }
        ArmorRandomizer();
    }

    // Update is called once per frame
    void Update()
    {
        
        Attack();
        if(health<=0)
        {
            isAlive = false;
            Destroy(gameObject);
        }
        if (hasArmor == true)
        {
            gameObject.GetComponent<MeshRenderer>().material = Armored;
        }
        else { gameObject.GetComponent<MeshRenderer>().material = RegularMaterial; }
        
    }
    void Attack()
    {
        if(inAttackZone==true)
        {
             

            StartCoroutine(AttackCountdown());
        }
        
    }
    void Taunted()
    {

    }
    void ArmorRandomizer()
    {
        int a = Random.Range(0, 3);
        if(a == 0)
        {
            hasArmor = true;
        }
        else { hasArmor = false; }
        
    }
    
    IEnumerator AttackCountdown()
    {
        SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        yield return new WaitForSeconds(AttackCooldown);
        if(enemytag == "Shooter")
        {
            SelectedEnemy.GetComponent<ShooterSC>().health -= 10;
        }
        if (enemytag == "Healer")
        {
            SelectedEnemy.GetComponent<HealerSC>().health -= 10;
        }
        if (enemytag == "Tank")
        {
            SelectedEnemy.GetComponent<TankSC>().health -= 10;
        }
       
        
        


    }
    IEnumerator isTaunted()
    {
       
        
        yield return new WaitForSeconds(nekadartauntlukalacak);
        enemyselection = Random.Range(1, 3);
        if (enemyselection == 1)
        {
            enemytag = "Healer";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            agent.SetDestination(SelectedEnemy.transform.position);
           
        }
        if (enemyselection == 2)
        {
            enemytag = "Shooter";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            agent.SetDestination(SelectedEnemy.transform.position);

            
        }
        if (enemyselection == 3)
        {
            enemytag = "Tank";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            agent.SetDestination(SelectedEnemy.transform.position);
            
        }
    }
    private void OnTriggerStay(Collider other)
    {
        ////Shooter///
        if (hasArmor == false && other.gameObject.tag == "ShooterBullet" )
        {
            health -= 20;
            ShooterSC.mana += 10;
            Destroy(other.gameObject);
        }
        
        if (hasArmor == true && other.gameObject.tag == "ShooterBullet")
        {
            health -= 10;
            ShooterSC.mana += 10;
            Destroy(other.gameObject);
        }
        if (hasArmor == true && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 25;

        }
        ////Shooter////

        ////Tank////
       
        if(other.gameObject.tag =="Tauntcollider")
        {
            
            
            agent.SetDestination(Tank.transform.position);
            //StartCoroutine(isTaunted());
            
        }
        ////Tank////

        ////Healer////
        if (hasArmor == false && other.gameObject.tag == "HealerBullet")
        {
            health -= 20;
            HealerSC.mana += 25;
            Debug.Log(HealerSC.mana );
            Destroy(other.gameObject);
        }
        if (hasArmor == true && other.gameObject.tag == "HealerBullet")
        {
            health -= 10;
            HealerSC.mana += 25;
            Debug.Log(HealerSC.mana );
            Destroy(other.gameObject);
        }
        if (hasArmor == true && other.gameObject.tag == "HealerArmorBreaker")
        {
            hasArmor = false;
        }
        ////Healer////


    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasArmor == false && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 50;


        }
    }
}
