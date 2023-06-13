using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Text stage;
    [SerializeField] private Text explain;
    [SerializeField] private GameObject frame;
    private readonly float SHOW_TIME = 0.5f;     //実際にステージ生成させる何秒前に何ステージ目かを表すテキストを表示させるか

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
        explain.enabled = true;
        stage.enabled = false;
        frame.SetActive(false);
    }

    void Update()
    {
        //時間を計測
        generateTime += Time.deltaTime;

        //経過時間がクールタイムより小さかったら
        if (generateTime < STAGE_COOL_TIME)
        {
            if(generateTime > STAGE_COOL_TIME - 1)
            {
                explain.enabled = false;
                frame.SetActive(true);
            }

            //ここから先の処理は行わせない
            return;
        }
        //クールタイムを過ぎてステージを生成することになったら
        else if (generateTime >= STAGE_COOL_TIME && generateTime <= (STAGE_COOL_TIME + STAGE_TIME))
        {
            stage.enabled = true;
            stage.text = "Level" + GameManager.Instance.GetStageNumber();

            if (generateTime > (STAGE_COOL_TIME + SHOW_TIME))
            {
                stage.enabled = false;
            }
        }
        //ステージを生成し終わったら
        else if(generateTime > (STAGE_COOL_TIME + STAGE_TIME))
        {
            generateTime = 0;
            Invoke(nameof(NextStage), STAGE_COOL_TIME - 3);
            return;
            //経過時間を初期化し、次のステージに移行
        }

        //今プレイヤーがどのブロックにいるか
        int targetPosIndex = (int)(Instance.transform.position.z / GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH);

        //もしも自動生成されるステージのブロックが今いるプレイヤーのブロックよりもAheadステージ分大きい場合
        if (targetPosIndex + aheadStage > StageIndex)
        {
            StageManager(targetPosIndex + aheadStage);
        }
    }

    //次のステージの進むときに難易度を上げるための関数
    void NextStage()
    {
        GameManager.Instance.IncrementStageNumber();
    }

    //ステージの生成を行う関数
    //第一引数：今自分がいるブロック
    void StageManager(int maps)
    {
        if (maps <= StageIndex)
        {
            return;
        }

        //指定したステージまで作成する
        for (int i = StageIndex + 1; i <= maps; i++)
        {
            GameObject stage = MakeStage(i, GetStage(GetStageState(GameManager.Instance.GetStageNumber()), stagenum));
            StageList.Add(stage);
        }

        //古いステージを削除する
        while (StageList.Count > aheadStage + 1)
        {
            DestroyStage();
        }

        StageIndex = maps;
    }

    //ステージを生成する
    GameObject MakeStage(int index, GameObject stageObj)
    {
        GameObject stageObject = Instantiate(stageObj, new Vector3(0, 0, GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH * (index + 2)), Quaternion.identity);
        return stageObject;
    }

    //ステージのステートに応じた「ゲームオブジェクト」を取得する関数
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

    //ステージが今何ステージ目なのかを表す「ステート」を取得する関数
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

    //ステージを生成するおおもとのオブジェクトを破棄
    void DestroyStage()
    {
        GameObject oldStage = StageList[0];
        StageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
