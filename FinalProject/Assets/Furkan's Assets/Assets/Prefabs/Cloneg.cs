using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloneg : MonoBehaviour
{
    public Vector3 minVel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(gameObject.GetComponent<Rigidbody>().velocity, minVel));
        

    }
}
