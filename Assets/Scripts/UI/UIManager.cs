using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void EndGame()
    {
        GameManager.Instance.InitializeGame();
        SceneManager.LoadScene("Title");
    }
}
