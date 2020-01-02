using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyTestNW : NetworkBehaviour
{


    [SyncVar(hook = "CheckDeath")]
    public float health = 5;


    void CheckDeath(float updatedHealth)
    {
        if (updatedHealth<=0)
        {
            Destroy(gameObject);
        }
    }
}
