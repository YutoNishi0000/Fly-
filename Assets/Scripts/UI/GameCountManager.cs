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
        //何個スターを表示したかを表示する
        UIController();
    }

    void UIController()
    {
        Stars.text = "x" + GameManager.Instance.GetNumStars();
    }
}
