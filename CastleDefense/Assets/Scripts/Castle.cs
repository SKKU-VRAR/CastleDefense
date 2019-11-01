using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    private float hp = 100f;

    public float Hp { get => hp; set => hp = value; }

    void Update()
    {
        if (hp <= 0)
        {
            Debug.Log("Game over");
        }
    }
}
