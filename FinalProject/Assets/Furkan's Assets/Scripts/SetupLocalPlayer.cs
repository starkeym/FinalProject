using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class SetupLocalPlayer : NetworkBehaviour
{
    public static int chID;
    public GameObject[] Characters;
    GameObject localGameOBJ;
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer==false)
        {
            return;
        }
        CmdspawnCharacter();
    }

    [Command]
    void CmdspawnCharacter() {
        localGameOBJ = Instantiate(Characters[chID]);
        NetworkServer.Spawn(localGameOBJ);
    }
}
