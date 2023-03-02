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
        //�����I�ɃI�u�W�F�N�g�z�u���s��
        CreateStage();
    }

    void CreateStage()
    {
        InitializeField(_field);
        SetBirds(GameManager.Instance.GetStageDifficultComponent().BIRDS_NUM, _field, _birdsNum);
        RenderObstacles(_birdPref, _starPref, _birdPos, _field);
    }

    void InitializeField(Field field)
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
            //���ꂼ��̃u���b�N�ɃA�h���X��t����
            birdsList.Add(i);
        }

        //�z�u���钹�̐��������[�v����
        for (int j = 0; j < num; j++)
        {
            //�d�����Ȃ��悤�Ƀt���O���g���Ăǂ��ɒ���z�u����̂������_���Ɍ��߂�
            int index = Random.Range(0, birdsList.Count);

            int rand = birdsList[index];

            for (int y = 0; y < BLOCK_NUM; y ++)
            {
                for(int x = 0; x < BLOCK_NUM; x++)
                {
                    field.bird[rand % BLOCK_NUM, Mathf.FloorToInt(rand / BLOCK_NUM)] = true;
                }
            }

            //�󂪕t���I������v�f�̓��X�g����r��
            birdsList.RemoveAt(index);
        }

        //�X�^�[�̔z�u�͐�قǐ����������X�g�̗]�����v�f�̒����烉���_���Ɏ��o���X�^�[�z�u�̈��t����
        int star = Random.Range(0, birdsList.Count);

        field.star[birdsList[star] % BLOCK_NUM, Mathf.FloorToInt(birdsList[star] / BLOCK_NUM)] = true;
    }

    //�I�u�W�F�N�g�����ۂ�SetBirds�֐��ŕt����������Ƃɐ�������
    void RenderObstacles(GameObject birdPref, GameObject starPref, GameObject[] pos, Field field)
    {
        for(int y = 0; y < BLOCK_NUM; y++)
        {
            for (int x = 0; x < BLOCK_NUM; x++)
            {
                //���̈󂪂��Ă�����
                if (field.bird[y, x])
                {
                    //���𐶐�
                    Instantiate(birdPref, pos[y * BLOCK_NUM + x].transform.position, birdPref.transform.rotation);
                }
                //���̈󂪂��Ă�����
                else if (field.star[y, x])
                {
                    //�X�^�[�𐶐�
                    Instantiate(starPref, pos[y * BLOCK_NUM + x].transform.position, starPref.transform.rotation);
                }
            }
        }
    }
}

//�t�B�[���h�N���X
public class Field
{
    public bool[,] bird;     //���̈�����邽�߂̓񎟌��z��

    public bool[,] star;    //�X�^�[�̈�����邽�߂̓񎟌��z��
}

