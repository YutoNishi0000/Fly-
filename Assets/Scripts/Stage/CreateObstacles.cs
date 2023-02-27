using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class CreateObstacles : MonoBehaviour
{
    [SerializeField] private GameObject _birdPref;

    //鳥を生成する位置(シーン上で調整するほうがやりやすい)
    [SerializeField] private GameObject[] _birdPos;

    private readonly Field[,] FIELD = new Field[4, 4]; 

    //何×何マスにするか
    private readonly int BLOCK_NUM = 4;

    private bool[,] _field;

    private List<int> _birdsNum = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        CreateStage();
    }

    void CreateStage()
    {
        InitializeFirld();
        SetBirds(6, _field, _birdsNum);
        GenerateBirds(_birdPref, _birdPos, _field);
    }

    void InitializeFirld()
    {
        //インスタンス生成
        _field = new bool[BLOCK_NUM, BLOCK_NUM];

        //初期化処理
        for (int y = 0; y < BLOCK_NUM; y++)
        {
            for (int x = 0; x < BLOCK_NUM; x++)
            {
                _field[y, x] = false;
            }
        }
    }

    //指定された数だけ鳥を配置
    void SetBirds(int num, bool[,] field, List<int> birdsList)
    {
        int allBlocks = BLOCK_NUM * BLOCK_NUM;
        List<int> garbageList = new List<int>();

        for (int i = 0; i < allBlocks; i++) 
        {
            birdsList.Add(i);
        }

        for (int j = 0; j < num; j++)
        {
            int rand = Random.Range(0, allBlocks);

            garbageList.Add(rand);

            //while(!CompareNum(rand, garbageList))
            //{
            //    rand = Random.Range(0, allBlocks);
            //}

            for(int y = 0; y < BLOCK_NUM; y ++)
            {
                for(int x = 0; x < BLOCK_NUM; x++)
                {
                    field[rand % BLOCK_NUM, Mathf.FloorToInt(rand / BLOCK_NUM)] = true;
                }
            }

            //birdsList.RemoveAt(rand);
        }
    }

    bool CompareNum(int num, List<int> list)
    {
        for(int i = 0; i < list.Count - 1; i++)
        {
            if(num == list[i])
            {
                return true;
            }
        }

        return false;
    }

    void GenerateBirds(GameObject birdPref, GameObject[] pos, bool[,] field)
    {
        for(int y = 0; y < BLOCK_NUM; y++)
        {
            for (int x = 0; x < BLOCK_NUM; x++)
            {
                if (field[y, x])
                {
                    Instantiate(birdPref, pos[y * BLOCK_NUM + x].transform.position, Quaternion.identity);
                }
            }
        }
    }
}

//フィールドクラス
public class Field
{
    public bool isBird;
}

