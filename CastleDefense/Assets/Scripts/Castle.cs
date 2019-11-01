using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public float hp = 100f;

    void Update()
    {
        if (hp <= 0)
        {
            Debug.Log("Game over");
        }
    }
}
