using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class CreateObstacles : MonoBehaviour
{
    [SerializeField] private GameObject _birdPref;
    [SerializeField] private GameObject _starPref;

    //���𐶐�����ʒu(�V�[����Œ�������ق������₷��)
    [SerializeField] private GameObject[] _birdPos;

    //���~���}�X�ɂ��邩
    private readonly int BLOCK_NUM = 4;

    //���H�������邩
    private readonly int BIRDS_NUM = 7;

    private Field _field;

    private List<int> _birdsNum = new List<int>();
    private List<GameObject> obstaclesList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _field = new Field();
        CreateStage();
    }

    private void Update()
    {

    }

    void CreateStage()
    {
        InitializeFirld(_field);
        SetBirds(GameManager.Instance.GetStageDifficultComponent().BIRDS_NUM, _field, _birdsNum);
        RenderObstacles(_birdPref, _starPref, _birdPos, _field);
    }

    void InitializeFirld(Field field)
    {
        //�C���X�^���X����
        field.bird = new bool[BLOCK_NUM, BLOCK_NUM];
        field.star = new bool[BLOCK_NUM, BLOCK_NUM];

        //����������
        for (int y = 0; y < BLOCK_NUM; y++)
        {
            for (int x = 0; x < BLOCK_NUM; x++)
            {
                field.bird[y, x] = false;
                field.star[y, x] = false;
            }
        }
    }

    //�w�肳�ꂽ����������z�u
    void SetBirds(int num, Field field, List<int> birdsList)
    {
        int allBlocks = BLOCK_NUM * BLOCK_NUM;

        for (int i = 0; i < allBlocks - 1; i++) 
        {
            birdsList.Add(i);
        }

        for (int j = 0; j < num; j++)
        {
            int index = Random.Range(0, birdsList.Count);

            int rand = birdsList[index];

            for (int y = 0; y < BLOCK_NUM; y ++)
            {
                for(int x = 0; x < BLOCK_NUM; x++)
                {
                    field.bird[rand % BLOCK_NUM, Mathf.FloorToInt(rand / BLOCK_NUM)] = true;
                }
            }

            birdsList.RemoveAt(index);
        }

        Debug.LogWarning(birdsList.Count);

        int star = Random.Range(0, birdsList.Count);

        field.star[birdsList[star] % BLOCK_NUM, Mathf.FloorToInt(birdsList[star] / BLOCK_NUM)] = true;
    }

    void RenderObstacles(GameObject birdPref, GameObject starPref, GameObject[] pos, Field field)
    {
        for(int y = 0; y < BLOCK_NUM; y++)
        {
            for (int x = 0; x < BLOCK_NUM; x++)
            {
                if (field.bird[y, x])
                {
                    Instantiate(birdPref, pos[y * BLOCK_NUM + x].transform.position, birdPref.transform.rotation);
                }
                else if (field.star[y, x])
                {
                    Instantiate(starPref, pos[y * BLOCK_NUM + x].transform.position, starPref.transform.rotation);
                }
            }
        }
    }
}

//�t�B�[���h�N���X
public class Field
{
    public bool[,] bird;
    public bool[,] star;
}

