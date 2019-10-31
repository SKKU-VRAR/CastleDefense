using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// non-VR 환경에서의 테스트를 위한 카메라 스크립트(1인칭)
public class TestCamera : MonoBehaviour
{
    public Transform player;
    public float sensitivity = 100f;
    float rotate;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotate -= y;
        player.Rotate(Vector3.up * x);
        transform.localRotation = Quaternion.Euler(rotate, 0, 0);
    }
}
