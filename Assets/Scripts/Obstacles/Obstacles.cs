using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : Actor
{
    void Update()
    {
        if(transform.position.z < Instance.transform.position.z - Instance.GetDistanceCamera())
        {
            //プレイヤーのz座標が自身のz座標よりカメラのz座標が大きくなったら自身を消す
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}