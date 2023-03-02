using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : Actor
{
    private bool onceflag;

    private AutoStage autoStage;

    private void Start()
    {
        autoStage = FindObjectOfType<AutoStage>();
        onceflag = false;
    }

    void Update()
    {
        if(transform.position.z < Instance.transform.position.z - Instance.GetDistanceCamera() && !onceflag)
        {
            autoStage.IncrementStagePassCount();
            Debug.Log("ステージインスリメント");
            Destroy(gameObject);
            onceflag = true;
        }
    }
}