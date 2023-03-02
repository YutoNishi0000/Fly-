using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Text RESULT;
    [SerializeField] private Text HIGH_SCORE;

    // Start is called before the first frame update
    void Start()
    {
        //リザルト表示を行う
        RESULT.text = "スコア：" + GameManager.Instance.GetTotalScore() + "pt";
        if(GameManager.Instance.GetTotalScore() > GameManager.Instance.GetHighScore())
        {
            GameManager.Instance.SetHighScore(GameManager.Instance.GetTotalScore());
        }

        HIGH_SCORE.text = "ハイスコア：" + GameManager.Instance.GetHighScore() + "pt";
    }
}
