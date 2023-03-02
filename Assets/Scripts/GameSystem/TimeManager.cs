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
        //if (!GameManager.Instance.GetStageLockFlag())
        {
            _gameTime += Time.deltaTime;
        }
    }

    public void InitializeTime()
    {
        _gameTime = 0;
    }

    public float GetGameTime()
    {
        return _gameTime;
    }
}
