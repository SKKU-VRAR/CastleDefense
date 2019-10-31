using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 마법의 부모 클래스
public class Magic : MonoBehaviour
{
    // 마법 원소를 위한 enum
    public enum ElementType { None, Fire, Water, Earth, Air };
    // 마법의 원소 타입
    private ElementType element;
    // 마법의 비용(마나)
    private int cost;
    // 마법의 데미지
    private int damage;
    // 이후 생각나는대로 추가

    // getters & setters
    public ElementType Element { get => element; set => element = value; }
    public int Cost { get => cost; set => cost = value; }
    public int Damage { get => damage; set => damage = value; }
}
