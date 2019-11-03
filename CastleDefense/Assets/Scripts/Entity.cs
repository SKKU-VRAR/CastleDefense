using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 체력을 지닌 객체가 상속할 class
public class Entity : MonoBehaviour
{
    private float hp;

    public float Hp { get => hp; set => hp = value; }
}
