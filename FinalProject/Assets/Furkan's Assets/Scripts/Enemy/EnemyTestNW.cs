using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyTestNW : NetworkBehaviour
{

    GameObject[] players;

    [SyncVar(hook = "CheckDeath")]
    public float health = 5;
    [SyncVar]
    public GameObject Target;
    [SyncVar]
    public bool isTaunted = false;





    public float speed = 5;
    public float shootingRange;

    void Start()
    {
        if (isServer)
        {
            Target = players[Random.Range(0,players.Length)];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            attack(Target);
            //movement;
        }
    }
    void attack(GameObject target) {
        if (Vector3.Distance(gameObject.transform.position,target.transform.position)>shootingRange)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,target.transform.position,speed);
            return;
        }
        ///yield shoot

    }

    void CheckDeath(float updatedHealth)
    {
        if (updatedHealth<=0)
        {
            Destroy(gameObject);
        }
    }
}
