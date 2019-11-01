using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Magic
{
    // speed
    public float speed = 10.0f;
    private int toexplode = 0 ;
    public int ToExplode { get => toexplode; set => toexplode = value; }
    public GameObject bomb;
    private int is_exploed = 0;
    void Start()
    {
        // type fire
        Element = ElementType.Fire;
        // cost mana
        Cost = 0;
        // damage
        Damage = 160;
    }

    void Update()
    {
        // 단순히 해일을 앞으로 이동
        transform.Translate(new Vector3(0f,-1f,0f) * speed * Time.deltaTime);
        transform.localScale += new Vector3(0.01f,0.01f,0.01f);
        // Debug.Log("asdfsdf");
        if(is_exploed==1)
            bomb.transform.localScale += new Vector3(0.3f,0.3f,0.3f);
        
    }

    void OnTriggerEnter(Collider collider)

    {
       if(collider.gameObject.tag == "Ground")
       {
           Instantiate(bomb, new Vector3(0, 0f, 0f), Quaternion.identity);
           is_exploed = 1;
           //Destroy(this.gameObject);
       }
    }
}