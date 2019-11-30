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
    GameObject Tank;

    /// heroes///
    

    //enemyselection///
    int enemyselection;
    int priority;
    public float stoppingDistance;
    public string enemytag;
    //enemyselection//
    public GameObject SelectedEnemy;

    public float nekadartauntlukalacak;
    //enemyattack///
    public GameObject ReBullet;


    NavMeshAgent agent;
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
        }
        if (enemyselection == 2)
        {
            agent.SetDestination(Shooter.transform.position);
            enemytag = "Shooter";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
        if (enemyselection == 3)
        {
            agent.SetDestination(Tank.transform.position);
            enemytag = "Tank";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBehaviour();
        isTaunted();
    }
    void EnemyBehaviour()
    {
        if(agent.remainingDistance <= stoppingDistance)
        {
            EnemyAttack();
        }
    }
    void EnemyAttack()
    {
        StartCoroutine(AttackCooldown());
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

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1);
        Instantiate(ReBullet, gameObject.transform.position, Quaternion.identity);
    }
    IEnumerator isTaunted()
    {
        enemyselection = 3;
        yield return new WaitForSeconds(nekadartauntlukalacak);
        enemyselection = Random.Range(1, 3);
        if (enemyselection == 1)
        {
            agent.SetDestination(Healer.transform.position);
            enemytag = "Healer";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
        if (enemyselection == 2)
        {
            agent.SetDestination(Shooter.transform.position);
            enemytag = "Shooter";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
        if (enemyselection == 3)
        {
            agent.SetDestination(Tank.transform.position);
            enemytag = "Tank";
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ////Shooter///
        if (hasArmor == false && other.gameObject.tag == "ShooterBullet")
        {
            health -= 20;
            ShooterSC.mana += 10;
        }
        if (hasArmor == false && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 50;

        }
        if (hasArmor == true && other.gameObject.tag == "ShooterBullet")
        {
            health -= 10;
            ShooterSC.mana += 10;
        }
        if (hasArmor == true && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 25;

        }
        ////Shooter////

        ////Tank////
       
        ////Tank////

        ////Healer////
        if (hasArmor == false && other.gameObject.tag == "HealerBullet")
        {
            health -= 20;
            HealerSC.mana = 25;
        }
        if (hasArmor == true && other.gameObject.tag == "HealerBullet")
        {
            health -= 10;
            HealerSC.mana = 25;
        }
        if (hasArmor == true && other.gameObject.tag == "HealerArmorBreaker")
        {
            hasArmor = false;
        }
        ////Healer////


    }
}
