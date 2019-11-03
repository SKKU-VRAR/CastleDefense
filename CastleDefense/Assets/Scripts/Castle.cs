using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Entity
{
    void Start()
    {
        Hp = 100f;
    }
    void Update()
    {
        if (Hp <= 0)
        {
            Debug.Log("Game over");
        }
    }
}
