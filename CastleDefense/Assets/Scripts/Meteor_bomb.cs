using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Magic
{
    
     public float speed = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0f,-1f,0f) * speed * Time.deltaTime);
        
        
            Debug.Log("WTF");
            // transform.localScale = new Vector3(1.1f,1.1f,1.1f);
            transform.localScale += new Vector3(3f,3f,3f);

        
    }
    
}
