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
        localGameOBJ = Instantiate(Characters[id]);
        NetworkServer.SpawnWithClientAuthority(localGameOBJ, connectionToClient);
    }
}
