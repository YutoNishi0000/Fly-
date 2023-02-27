using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class CreateObstacles : MonoBehaviour
{
    [SerializeField] private GameObject _birdPref;

    //���𐶐�����ʒu(�V�[����Œ�������ق������₷��)
    [SerializeField] private GameObject[] _birdPos;

    private readonly Field[,] FIELD = new Field[4, 4]; 

    //���~���}�X�ɂ��邩
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
        //�C���X�^���X����
        _field = new bool[BLOCK_NUM, BLOCK_NUM];

        //����������
        for (int y = 0; y < BLOCK_NUM; y++)
        {
            for (int x = 0; x < BLOCK_NUM; x++)
            {
                _field[y, x] = false;
            }
        }
    }

    //�w�肳�ꂽ����������z�u
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

//�t�B�[���h�N���X
public class Field
{
    public bool isBird;
}

