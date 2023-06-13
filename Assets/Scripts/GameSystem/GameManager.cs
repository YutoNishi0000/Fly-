using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private int numStage;    //今何ステージ目なのか
    private int passCount;   //各ステートごとに一回だけしか処理をさせないときに使う
    private float SCORE;    //スコア
    private float HIGH_SCORE;    //ハイスコア

    public TimeManager timeManager;
    private StageDifficultyComponent stage;


    // Start is called before the first frame update
    void Start()
    {
        numStars = 0;
        passCount = 0;
        numStage = 1;
        HIGH_SCORE = 0;
        timeManager = GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DifficulityManager(numStage);
        EndApplication();
    }

    public void InitializeGame()
    {
        numStars = 0;
        passCount = 0;
        numStage = 1;
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
                passCount++;
                break;
        }
    }

    //アプリケーションを終了させる関数
    public void EndApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    //ゲームの難易度を管理するクラスのインスタンスを取得
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
        //スコアを計算
        SCORE = CalScore(timeManager.GetGameTime(), numStars);
        //リザルト画面に移行
        SceneManager.LoadScene("Result");
    }

    public float CalScore(float time, int numStar)
    {
        // 小数点以下切り捨て
        return Mathf.FloorToInt(time * numStar);
    }

    //今取得している星の数を取得
    public int GetNumStars()
    {
        return numStars;
    }

    //今何ステージ目なのかを取得する
    public int GetStageNumber()
    {
        return numStage;
    }

    //ステージ番号を１増やす
    public void IncrementStageNumber()
    {
        numStage++;
    }

    //スコアを取得
    public int GetTotalScore()
    {
        return Mathf.FloorToInt(SCORE);
    }

    //ハイスコアをセット
    public void SetHighScore(int score)
    {
        HIGH_SCORE = score;
    }

    //ハイスコアを取得
    public float GetHighScore()
    {
        return HIGH_SCORE;
    }
}