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
        RESULT.text = "�X�R�A�F" + GameManager.Instance.GetTotalScore();
        if(GameManager.Instance.GetTotalScore() > GameManager.Instance.GetHighScore())
        {
            GameManager.Instance.SetHighScore(GameManager.Instance.GetTotalScore());
        }

        HIGH_SCORE.text = "�n�C�X�R�A�F" + GameManager.Instance.GetHighScore();
    }
}
