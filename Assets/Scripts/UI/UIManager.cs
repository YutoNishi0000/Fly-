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
        //�Q�[����ʂɐi��
        SceneManager.LoadScene("SampleScene");
    }

    public void EndGame()
    {
        //�������������s���ă^�C�g���ɖ߂�
        GameManager.Instance.InitializeGame();
        SceneManager.LoadScene("Title");
    }
}
