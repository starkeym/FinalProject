using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SetupLocalPlayer : NetworkBehaviour
{
    public GameObject[] Characters;
    GameObject localGameOBJ;
    // Start is called before the first frame update
    void Update()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        if (Input.GetKeyDown("1"))
        {

            CmdspawnCharacter(0);
            gameObject.name="ShSpawner";
        }

        if (Input.GetKeyDown("2"))
        {

            CmdspawnCharacter(1);
        }

        if (Input.GetKeyDown("3"))
        {

            CmdspawnCharacter(2);
        }
    }

    [Command]
    void CmdspawnCharacter(int id) {
        localGameOBJ = Instantiate(Characters[id],GameObject.Find("SpawnPos").transform.position,Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(localGameOBJ, connectionToClient);
    }
    [Command]
    public void CmdassignAuth(GameObject bullet,GameObject obj){
        bullet.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    obj.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(bullet.GetComponent<NetworkIdentity>().connectionToClient);

    }
}
