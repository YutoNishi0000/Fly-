using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : Actor
{
    void Update()
    {
        if(transform.position.z < Instance.transform.position.z - Instance.GetDistanceCamera())
        {
            //�v���C���[��z���W�����g��z���W���J������z���W���傫���Ȃ����玩�g������
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