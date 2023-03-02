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
        //ƒQ[ƒ€ŠÔ‚ğæ“¾
        _gameTime += Time.deltaTime;
    }

    //ƒQ[ƒ€ŠÔ‚ğ‰Šú‰»
    public void InitializeTime()
    {
        _gameTime = 0;
    }

    //ƒQ[ƒ€ŠÔ‚ğæ“¾
    public float GetGameTime()
    {
        return _gameTime;
    }
}
