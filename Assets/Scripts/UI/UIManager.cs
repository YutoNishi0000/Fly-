using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private void Update()
    {
        EndApplication();
    }

    public void EndApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        //ゲーム画面に進む
        SceneManager.LoadScene("SampleScene");
    }

    public void EndGame()
    {
        //初期化処理を行ってタイトルに戻る
        GameManager.Instance.InitializeGame();
        SceneManager.LoadScene("Title");
    }
}
