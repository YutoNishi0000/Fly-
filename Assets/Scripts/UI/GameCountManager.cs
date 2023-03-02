using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCountManager : MonoBehaviour
{
    [SerializeField] private Text Stars;
    [SerializeField] private Text PassTime;

    void Update()
    {
        //���X�^�[��\����������\������
        UIController();
    }

    void UIController()
    {
        Stars.text = "x" + GameManager.Instance.GetNumStars();
    }
}
