using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemySC : MonoBehaviour
{
    // Start is called before the first frame update
    public bool hasArmor;
    public static float health = 100;

    //heroes//
    public GameObject Healer;
    public GameObject Shooter;
    public GameObject Tank;

    /// heroes///
    

    //enemyselection///
    int enemyselection;
    int priority;
    public float stoppingDistance;
    //enemyselection//

    //enemyattack///
    public GameObject ReBullet;


    NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyselection = Random.Range(1, 3);
        if(enemyselection ==1)
        {
            agent.SetDestination(Healer.transform.position);
        }
        if (enemyselection == 2)
        {
            agent.SetDestination(Shooter.transform.position);
        }
        if (enemyselection == 3)
        {
            agent.SetDestination(Tank.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyBehaviour();
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

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1);
        Instantiate(ReBullet, gameObject.transform.position, Quaternion.identity);
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
