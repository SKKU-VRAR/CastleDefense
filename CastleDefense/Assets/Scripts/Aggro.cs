using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 흙마법 만들 때 이거 참고해서 만들면 될듯
// 흙마법 벽의 prefab에는 Entity를 상속하는 이런 script + Magic을 상속하는 script 두개가 들어가면 될 것 같음
public class Aggro : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        Hp = 20f;
        GameManager.instance.AddCastle(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp <= 0)
        {
            GameManager.instance.DestroyCastle(gameObject);
            Destroy(gameObject);
        }
    }
}
