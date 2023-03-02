using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region シングルトン化

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private int numStars;      //星を何個取ったか

    private bool StageLock;   //ステージ生成を行うかどうか

    public TimeManager timeManager;

    private int numStage;    //今何ステージ目なのか

    private int passCount;

    private StageDifficultyComponent stage;

    private float SCORE;    //スコア
    private float HIGH_SCORE;    //ハイスコア

    // Start is called before the first frame update
    void Start()
    {
        numStars = 0;
        passCount = 0;
        numStage = 1;
        StageLock = true;
        HIGH_SCORE = 0;
        timeManager = GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DifficulityManager(numStage);
    }

    public void InitializeGame()
    {
        numStars = 0;
        passCount = 0;
        numStage = 1;
        StageLock = true;
        timeManager.InitializeTime();
    }

    //ステージの難易度調整を行う関数
    //第一引数：今何ステージ目か
    void DifficulityManager(int stageNumber)
    {
        switch(stageNumber)
        {
            //毎フレームnewするのはメモリ消費が激しいので各要素ごとに一回だけインスタンスを生成する
            case 1:
                if(passCount >= 1)
                {
                    return;
                }

                stage = new StageDifficultyComponent(20, 7, 6);
                passCount++;
                break;
            case 2:
                if(passCount >= 2)
                {
                    return;
                }

                stage = new StageDifficultyComponent(18, 10, 10);
                passCount++;
                break;
            case 3:
                if(passCount >= 3)
                {
                    return;
                }

                stage = new StageDifficultyComponent(15, 15, 13);
                passCount++;
                break;
            case 4:
                if (passCount >= 4)
                {
                    return;
                }

                //ゲームクリア
                Result();
                break;
        }
    }

    public StageDifficultyComponent GetStageDifficultComponent()
    {
        return stage;
    }

    public void IncrementNumStars()
    {
        //取得した星の数をインクリメント
        numStars++;
    }

    public void Result()
    {
        SCORE = CalScore(timeManager.GetGameTime(), numStars);
        //リザルト画面に移行
        SceneManager.LoadScene("Result");
    }

    public float CalScore(float time, int numStar)
    {
        // 小数点以下切り捨て
        return Mathf.FloorToInt(time * numStar);
    }

    public int GetNumStars()
    {
        return numStars;
    }

    public bool GetStageLockFlag()
    {
        return StageLock;
    }

    public void SetStageLockFlag(bool flag)
    {
        StageLock = flag;
    }

    public void GameStart()
    {
        StageLock = false;
    }

    public void NextStage(float gameTime)
    {
        gameTime = 0;
    }

    public int GetStageNumber()
    {
        return numStage;
    }

    public void IncrementStageNumber()
    {
        numStage++;
    }

    //スコアを取得
    public int GetTotalScore()
    {
        return Mathf.FloorToInt(SCORE);
    }

    public void SetHighScore(int score)
    {
        HIGH_SCORE = score;
    }

    public float GetHighScore()
    {
        return HIGH_SCORE;
    }
}

//インスタンスを生成する時コンストラクタで値を設定しやすくしたい
public class StageDifficultyComponent
{
    public int STAGE_WIDTH;    //ステージの間隔
    public float MOVE_SPEED;   //移動速度
    public int BIRDS_NUM;      //生成する鳥の数

    //コンストラクタ
    public StageDifficultyComponent(int width, float speed, int birds)
    {
        STAGE_WIDTH = width;
        MOVE_SPEED = speed;
        BIRDS_NUM = birds;
    }
}