using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOLO : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up * 150 * Time.deltaTime);
    }
}
