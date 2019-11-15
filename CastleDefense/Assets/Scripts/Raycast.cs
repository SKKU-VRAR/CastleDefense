using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public GameObject intersect_point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update (){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // if(Physics.Raycast(ray,out hit))
        // {
        //     // Debug.Log(hit.transform.name);
        //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //     Debug.Log("Did Hit");
        // }
        if (Physics.Raycast(ray, out hit, 100)){
            Debug.DrawLine(ray.origin, hit.point);
            Debug.Log(hit.point);
            if(hit.transform.name == "Ground"){
                // intersect_point.transform.Translate(new Vector3(hit.point.x,0.0f,hit.point.z));
                intersect_point.transform.position = new Vector3(hit.point.x,0.0f,hit.point.z);
            }
        }
	}
}
