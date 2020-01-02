using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AnimationsNetwork : NetworkBehaviour
{
    // Start is called before the first frame update

        ////////////////ANIMATON/////////////////////
    Animator an = new Animator();
    GameObject forward;

        public SyncListFloat SynclayerWeight = new SyncListFloat();
    void OnIntChanged(SyncListFloat.Operation op, int index)
    {
        float[] localFloat = new float[5];
        if (SynclayerWeight.Count != 5)
        {
           // Debug.Log("asd");
            return;
        }
        for (int i = 0; i < 5; i++)
        {
           // Debug.Log("bsd");
            localFloat[i] = SynclayerWeight[i];
        }
        OnChangeLayerWeight(localFloat);
    }
    [SyncVar(hook ="OnChangeAnimationState")]
    public string SyncAnimState = ("idle");

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            SynclayerWeight.Callback=OnIntChanged;
            if(hasAuthority)HandleAnimations();        
    }
        void HandleAnimations() {
        // float angle = Mathf.Atan2(a, b) * Mathf.Rad2Deg;
        // Vector3 targetDir = test.mouseposition - transform.position;

        Vector2 a = new Vector2(gameObject.transform.position.z/*ortadakinokta*/ - forward.transform.position.z, gameObject.transform.position.x - forward.transform.position.x);
        Vector2 b = new Vector2(gameObject.transform.position.z - GetMousePosition().z, gameObject.transform.position.x - GetMousePosition().x);

        float angle = Vector2.SignedAngle(b, a) + 90;
        if (angle > 180 && angle < 270)
        {
            angle -= 360;
        }
        //Debug.Log(angle);

        ////Angle Chart Area 1
        if (angle > 0 && angle < 90)
        {
            float animationHardness = (angle * 1) / 90;
            an.SetLayerWeight(3, animationHardness);
            an.SetLayerWeight(2, (100 - animationHardness));

        }

        ////Angle Chart Area 2
        if (angle > 90 && angle < 180)
        {
            float localAngle = angle - 90;
            float animationHardness = (localAngle * 1) / 90;

            an.SetLayerWeight(4, animationHardness);

            an.SetLayerWeight(3, (100 - animationHardness));

        }

        ////Angle Chart Area 3
        if (angle > -180 && angle < -90)
        {
            float localAngle = angle + 180;
            float animationHardness = (localAngle * 1) / 90;

            an.SetLayerWeight(5, animationHardness);

            an.SetLayerWeight(4, (100 - animationHardness));

        }

        if (angle > -90 && angle < 0)
        {
            float localAngle = angle + 90;
            float animationHardness = (localAngle * 1) / 90;

            an.SetLayerWeight(2, animationHardness);

            an.SetLayerWeight(1, (100 - animationHardness));

            an.SetLayerWeight(5, 0);

            an.SetLayerWeight(4, 0);

            an.SetLayerWeight(3, 0);

        }
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            an.SetBool("isWalking", true);
            CmdChangeAnimationState("walk");
        }
        else
        {
            an.SetBool("isWalking", false);
            CmdChangeAnimationState("idle");
        }

        float[] localFloat=new float[5];
        for (int i = 0; i < 5; i++)
        {
          localFloat[i]= an.GetLayerWeight(i+1);
        }

        CmdAnimationLayerWeight(localFloat);



    }


    //////////Updating on other clients that are  not us or server//////////////////
    void OnChangeLayerWeight(float[] anState) {
        if (hasAuthority)
        {
            return;
        }
        UpdateLayerWeight(anState);
        Debug.Log("1");
    }
    void OnChangeAnimationState(string state) {
        if (hasAuthority)
        {
            return;
        }
        UpdateAnimationState(state);
    }
    //////////Updating on the server//////////////////////////////////////////////////
    [Command]
    void CmdAnimationLayerWeight(float[] anState){
        UpdateLayerWeight(anState);
    }
    [Command]
    void CmdChangeAnimationState(string state) {
        UpdateAnimationState(state);
    }
    ////////////////////////////////////////////////////////////////////////////////

    void UpdateLayerWeight(float[] anState)
    {
        /*
        bool localBool = true;
        for (int i = 0; i < 5; i++)
        {
            if (SynclayerWeight.Count!=5)
            {
                localBool = false;
            }
            else if (anState[i]!=SynclayerWeight[i])
            {
                localBool = false;
            }
        }
        if (localBool)
        {
            return;
        }*/
        //else
        //{
            if (SynclayerWeight.Count!=5)
            {
                SynclayerWeight.Clear();
                for (int i = 0; i < 5; i++)
                {
                    SynclayerWeight.Add(anState[i]);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    SynclayerWeight[i] = anState[i];
                }
            }

        //}

        for (int i = 0; i < 5; i++)
        {
            an.SetLayerWeight(i+1,anState[i]);    
        }
    }
    void UpdateAnimationState(string state)
    {
        if (SyncAnimState == state) return;
        SyncAnimState = state;
        if (state == "walk")
        {
            an.SetBool("isWalking", true);
        }
        if (state=="idle")
        {
            an.SetBool("isWalking",false);
        }
    }
    public Vector3 GetMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            return new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z);
        }
        return Vector3.zero;
    }
}
