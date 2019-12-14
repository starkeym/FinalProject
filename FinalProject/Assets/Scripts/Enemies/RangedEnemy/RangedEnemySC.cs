using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemySC : MonoBehaviour
{
    // Start is called before the first frame update
    //Armor//
    public bool hasArmor;
    public Material Armored;
    public Material RegularMaterial;
    //Armor//
    public float health = 100;

    //heroes//
    GameObject Healer;
    GameObject Shooter;
    public GameObject Tank;

    /// heroes///
    public bool Taunted = false;
    

    //enemyselection///
    int enemyselection;
    int priority;
    float DistanceShooter;
    float DistanceTank;
    float DistanceHealer;
    public string enemytag;
    //enemyselection//
    public GameObject SelectedEnemy;
    public bool hasTarget = false;
    float Distance;

    public float nekadartauntlukalacak;
    //enemyattack///
    public GameObject ReBullet;
    public int bulletsfired = 0;
    public bool canAttack = false;
    bool cooldown = false;
    public GameObject BulletSpawnZone;

    float timer = 0;
    NavMeshAgent agent;

    /// <summary>
    /// ////////HERONUN ÖLDÜĞÜNE DAİR BİR BOOLEAN ALMASI LAZIM
    /// </summary>
    void Start()
    {
        Healer = GameObject.FindGameObjectWithTag("Healer");
        Shooter = GameObject.FindGameObjectWithTag("Shooter");
        Tank = GameObject.FindGameObjectWithTag("Tank");
        agent = GetComponent<NavMeshAgent>();
        enemyselection = Random.Range(1, 3);
        if (enemyselection == 1)
        {
            agent.SetDestination(Healer.transform.position);
            enemytag = "Healer";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            hasTarget = true;
            ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
        if (enemyselection == 2)
        {
            agent.SetDestination(Shooter.transform.position);
            enemytag = "Shooter";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            hasTarget = true;
            ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
        if (enemyselection == 3)
        {
            agent.SetDestination(Tank.transform.position);
            enemytag = "Tank";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            hasTarget = true;
            ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(gameObject.transform.position,SelectedEnemy.transform.position);
        transform.LookAt(new Vector3(SelectedEnemy.transform.position.x, 1.5f, SelectedEnemy.transform.position.z));
        EnemyBehaviour();
        
        
    }
    void EnemyBehaviour()
    {
        

        if (Distance <= 8)
        {
            
            agent.isStopped = true;
            canAttack = true;

        }
        else if (Distance >  8)
        {
            agent.isStopped = false;
            canAttack = false;
            agent.SetDestination(SelectedEnemy.transform.position);
        }
        if (canAttack == true && timer <=2 &&cooldown==false)
        {
            
            ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            gameObject.transform.LookAt(SelectedEnemy.transform.position);
            Instantiate(ReBullet, gameObject.transform.position, gameObject.transform.rotation);
            bulletsfired = 1;
           
        }
        if (bulletsfired >= 1)
        {
            cooldown = true;
            timer += Time.deltaTime;
            if (timer >= 1)
            {

                timer = 0;
                cooldown = false;
            }
        }




    }



    void ArmorRandomizer()
    {
        int a = Random.Range(0, 3);
        if (a == 0)
        {
            hasArmor = true;
        }
        else { hasArmor = false; }

    }
    void checkSelectedenemypos()
    {

    }

   
    IEnumerator isTaunted()
    {
        if(Taunted==true)
        {
            enemyselection = 3;
            agent.SetDestination(Tank.transform.position);
            enemytag = "Tank";
            ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            hasTarget = false;
            yield return new WaitForSeconds(nekadartauntlukalacak);
            Taunted = false;
            if(Taunted ==false && hasTarget ==false)
                enemyselection = Random.Range(1, 3);
            if (enemyselection == 1)
            {
                agent.SetDestination(Healer.transform.position);
                enemytag = "Healer";
                SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
                hasTarget = true;
                ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            }
            if (enemyselection == 2)
            {
                agent.SetDestination(Shooter.transform.position);
                enemytag = "Shooter";
                SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
                hasTarget = true;
                ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            }
            if (enemyselection == 3)
            {
                agent.SetDestination(Tank.transform.position);
                enemytag = "Tank";
                SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
                hasTarget = true;
                ReBullet.GetComponent<EnemyBullet>().ShootedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            }

        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        ////Shooter///
        if (hasArmor == false && other.gameObject.tag == "ShooterBullet")
        {
            health -= 20;
            ShooterSC.mana += 10;
            Destroy(other.gameObject);
            DistanceShooter = Vector3.Distance(gameObject.transform.position, Tank.transform.position);
            if(DistanceShooter < Distance)
            {
                enemytag = "Shooter";
            }
                
        }

        if (hasArmor == true && other.gameObject.tag == "ShooterBullet")
        {
            health -= 10;
            ShooterSC.mana += 10;
            Destroy(other.gameObject);
            DistanceShooter = Vector3.Distance(gameObject.transform.position, Tank.transform.position);
            if (DistanceShooter < Distance)
            {
                enemytag = "Shooter";
            }
        }
        if (hasArmor == true && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 25;
            DistanceShooter = Vector3.Distance(gameObject.transform.position, Tank.transform.position);
            if (DistanceShooter < Distance)
            {
                enemytag = "Shooter";
            }


        }
        ////Shooter////

        ////Tank////

        
        ////Tank////

        ////Healer////
        if (hasArmor == false && other.gameObject.tag == "HealerBullet")
        {
            health -= 20;
            HealerSC.mana += 25;
            Debug.Log(HealerSC.mana);
            Destroy(other.gameObject);
            DistanceHealer = Vector3.Distance(gameObject.transform.position, Healer.transform.position);
            if (DistanceHealer <Distance)
            {
                enemytag = "Healer";

            }
        }
        if (hasArmor == true && other.gameObject.tag == "HealerBullet")
        {
            health -= 10;
            HealerSC.mana += 25;
            Debug.Log(HealerSC.mana);
            Destroy(other.gameObject);
            DistanceHealer = Vector3.Distance(gameObject.transform.position, Healer.transform.position);
            if (DistanceHealer < Distance)
            {
                enemytag = "Healer";

            }
        }
        if (hasArmor == true && other.gameObject.tag == "HealerArmorBreaker")
        {
            hasArmor = false;
        }
        ////Healer////


    }
    IEnumerator waitforotherscripts()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasArmor == false && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 50;


        }
        if (other.gameObject.tag == "TAUNT")
        {


            StartCoroutine(isTaunted());

        }
    }
}
