using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStage : Actor
{
    int StageSize = 15;
    int StageIndex;

    public Transform Target;    //プレイヤーオブジェクト
    public GameObject[] stagenum;//ステージのプレハブ
    public int FirstStageIndex;//スタート時にどのインデックスからステージを生成するのか
    public int aheadStage; //事前に生成しておくステージ
    public List<GameObject> StageList = new List<GameObject>();//生成したステージのリスト
    public static float generateTime;   //経過時間を格納する変数
    private readonly float STAGE_TIME = 15;
    private readonly float STAGE_COOL_TIME = 15;         //ステージ間の秒数
    private int STAGE_PASS_COUNT;

    private enum StageState
    {
        None,
        FirstStage,
        SecondStage,
        ThirdStage,
    }

    private StageState stageState;

    void Start()
    {
        stageState = new StageState();
        generateTime = 0;
        StageIndex = FirstStageIndex - 1;
    }

    void Update()
    {
        //時間を計測
        generateTime += Time.deltaTime;

        //経過時間がクールタイムより小さかったら
        if (generateTime < STAGE_COOL_TIME)
        {
            //ここから先の処理は行わせない
            return;
        }
        //クールタイムを過ぎてステージを生成することになったら
        else if (generateTime >= STAGE_COOL_TIME && generateTime <= (STAGE_COOL_TIME + STAGE_TIME))
        {
            //特に何の処理も行わない
        }
        //ステージを生成し終わったら
        else if(generateTime > (STAGE_COOL_TIME + STAGE_TIME))
        {
            generateTime = 0;
            Invoke(nameof(NextStage), STAGE_COOL_TIME - 1);
            return;
            //経過時間を初期化し、次のステージに移行
        }

        int targetPosIndex = (int)(Target.position.z / GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH);

        if (targetPosIndex + aheadStage > StageIndex)
        {
            StageManager(targetPosIndex + aheadStage);
        }
    }

    void NextStage()
    {
        GameManager.Instance.IncrementStageNumber();
    }

    public void IncrementStagePassCount()
    {
        STAGE_PASS_COUNT++;
    }

    public int GetStagePassCount()
    {
        return STAGE_PASS_COUNT;
    }

    void StageManager(int maps)
    {
        if (maps <= StageIndex)
        {
            return;
        }

        for (int i = StageIndex + 1; i <= maps; i++)//指定したステージまで作成する
        {
            GameObject stage = MakeStage(i, GetStage(GetStageState(GameManager.Instance.GetStageNumber()), stagenum));
            StageList.Add(stage);
        }

        while (StageList.Count > aheadStage + 1)//古いステージを削除する
        {
            DestroyStage();
        }

        StageIndex = maps;
    }

    GameObject MakeStage(int index, GameObject stageObj)//ステージを生成する
    {
        GameObject stageObject = Instantiate(stageObj, new Vector3(0, 0, index * GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH + 2 * GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH), Quaternion.identity);

        return stageObject;
    }

    GameObject GetStage(StageState state, GameObject[] stageType)
    {
        switch(state)
        {
            case StageState.FirstStage:
                return stageType[(int)StageState.FirstStage];
            case StageState.SecondStage:
                return stageType[(int)StageState.SecondStage];
            case StageState.ThirdStage:
                return stageType[(int)StageState.ThirdStage];
            default:
                return stageType[(int)StageState.None];
        }
    }

    //
    StageState GetStageState(int STAGE_ID)
    {
        switch(STAGE_ID)
        {
            case 1:
                return StageState.FirstStage;
            case 2:
                return StageState.SecondStage;
            case 3:
                return StageState.ThirdStage;
            default:
                return StageState.None;
        }
    }

    void DestroyStage()
    {
        GameObject oldStage = StageList[0];
        StageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
