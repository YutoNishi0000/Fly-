using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float _gameTime;

    void Start()
    {
        _gameTime = 0;
    }

    void Update()
    {
        //�Q�[�����Ԃ��擾
        _gameTime += Time.deltaTime;
    }

    //�Q�[�����Ԃ�������
    public void InitializeTime()
    {
        _gameTime = 0;
    }

    //�Q�[�����Ԃ��擾
    public float GetGameTime()
    {
        return _gameTime;
    }
}
