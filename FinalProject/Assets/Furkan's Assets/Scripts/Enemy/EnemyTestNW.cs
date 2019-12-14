using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyTestNW : NetworkBehaviour
{
    [SyncVar(hook = "CheckDeath")]
    public float health = 5;
    [SyncVar]
    public GameObject Target;
    [SyncVar(hook ="Taunted")]
    public bool isTaunted = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
           //movement;
        }
    }

    void Taunted(bool tauntStatus) {
        if (isServer)
        {

        }
    }

    void CheckDeath(float updatedHealth)
    {
        if (updatedHealth<=0)
        {
            Destroy(gameObject);
        }
    }
}
