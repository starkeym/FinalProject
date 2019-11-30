using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool hasArmor;

    public int health;
    
    //heroes//
    public GameObject Healer;
    public GameObject Shooter;
    public GameObject Tank;

    /// heroes///

    //enemyselection///
    int enemyselection;
    int priority;
    public string enemytag;
    public GameObject AttackZone;

    //enemyselection//
    ///EnemyAttack///
    public bool inAttackZone = false;
    public int AttackCooldown;
    GameObject SelectedEnemy;

    ///EnemyAttack///
    ///Taunt///
    public float nekadartauntlukalacak;
    public static NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyselection = Random.Range(1, 3);
        if (enemyselection == 1)
        {
            agent.SetDestination(Healer.transform.position);
            enemytag = "Healer";
        }
        if (enemyselection == 2)
        {
            agent.SetDestination(Shooter.transform.position);
            enemytag = "Shooter";
        }
        if (enemyselection == 3)
        {
            agent.SetDestination(Tank.transform.position);
            enemytag = "Tank";
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = SelectedEnemy.transform.rotation;
        Attack();
    }
    void Attack()
    {
        if(inAttackZone==true)
        {
            agent.isStopped = true;
            

            StartCoroutine(AttackCountdown());
        }
        else
        {
            agent.isStopped = false;
            SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
            agent.SetDestination(SelectedEnemy.transform.position);
        }
    }
    IEnumerator AttackCountdown()
    {
        SelectedEnemy = GameObject.FindGameObjectWithTag(enemytag);
        yield return new WaitForSeconds(AttackCooldown);
        SelectedEnemy.GetComponent<HealerSC>().health -= 10;
        SelectedEnemy.GetComponent<ShooterSC>().health -= 10;
        SelectedEnemy.GetComponent<TankSC>().health -= 10;


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
        }
        if (enemyselection == 2)
        {
            agent.SetDestination(Shooter.transform.position);
            enemytag = "Shooter";
        }
        if (enemyselection == 3)
        {
            agent.SetDestination(Tank.transform.position);
            enemytag = "Tank";
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        ////Shooter///
        if (hasArmor == false && other.gameObject.tag == "ShooterBullet")
        {
            health -= 20;
            ShooterSC.mana += 10;
            Destroy(other.gameObject);
        }
        if (hasArmor == false && other.gameObject.tag == "ShooterUltiBullet")
        {
            health -= 50;


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
        if (hasArmor == false && other.gameObject.tag == "TankAttack")
        {
            health -= 20;
            TankSC.mana += 20;

        }
        if (hasArmor == true && other.gameObject.tag == "TankAttack")
        {
            health -= 10;
            TankSC.mana += 20;
        }
        if(other.gameObject.tag =="Tauntcollider")
        {
            isTaunted();
        }
        ////Tank////

        ////Healer////
        if (hasArmor == false && other.gameObject.tag == "HealerBullet")
        {
            health -= 20;
            HealerSC.mana = 25;
            Destroy(other.gameObject);
        }
        if (hasArmor == true && other.gameObject.tag == "HealerBullet")
        {
            health -= 10;
            HealerSC.mana = 25;
            Destroy(other.gameObject);
        }
        if (hasArmor == true && other.gameObject.tag == "HealerArmorBreaker")
        {
            hasArmor = false;
        }
        ////Healer////


    }
}
