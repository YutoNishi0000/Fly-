using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCountManager : MonoBehaviour
{
    [SerializeField] private Text Stars;
    [SerializeField] private Text PassTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UIController();
    }

    void UIController()
    {
        Stars.text = "x" + GameManager.Instance.GetNumStars();
        PassTime.text = Mathf.FloorToInt(GameManager.Instance.timeManager.GetGameTime() / 60) + "•ª" + Mathf.FloorToInt(GameManager.Instance.timeManager.GetGameTime() % 60) + "•b";
    }
}
