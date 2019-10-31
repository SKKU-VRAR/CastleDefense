using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 마법 발동 테스트를 위한 플레이어 클래스
public class TestPlayer : MonoBehaviour
{
    // 마법 prefab 받아오기
    public GameObject magic;
    void Update()
    {
        // 마우스 왼클릭하면 마법 발동
        if (Input.GetMouseButtonDown(0))
            // Instantiate(마법 prefab, 마법 prefab 소환할 좌표, 마법 prefab의 회전값 설정)
            // 이건 해일을 위한 Instantiate
            Instantiate(magic, new Vector3(0, 4.5f, -40f), Quaternion.identity);
    }
}
