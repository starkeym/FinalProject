using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Start is called before the first frame update
    

    float xRotation = 0f;
    private Camera mainCamera;
    Vector3 lookPos;
    public GameObject player;
    public static Ray ray;
    public static Vector3 pointToLook;
    public static float rayLength;
  




    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        
    }
    private void Update()
    {

      
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up+new Vector3(0,5,0), Vector3.zero);
        
       
        if (groundPlane.Raycast(ray, out rayLength))  
        {
            
            pointToLook = ray.GetPoint(rayLength);
            Debug.DrawLine(ray.origin, pointToLook, Color.blue);

            
            player.transform.LookAt(new Vector3(pointToLook.x,transform.position.y,pointToLook.z));
            

        }
        /*RaycastHit hit  
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            pointToLook = hit.point;
            Debug.DrawLine(ray.origin, pointToLook, Color.blue);

            player.transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            // Do something with the object that was hit by the raycast.
        }
        */

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
     
    }

    



}

