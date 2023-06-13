using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//�C���X�^���X�𐶐����鎞�R���X�g���N�^�Œl��ݒ肵�₷��������
public class StageDifficultyComponent
{
    public int STAGE_WIDTH;    //�X�e�[�W�̊Ԋu
    public float MOVE_SPEED;   //�ړ����x
    public int BIRDS_NUM;      //�������钹�̐�

    //�R���X�g���N�^
    public StageDifficultyComponent(int width, float speed, int birds)
    {
        STAGE_WIDTH = width;
        MOVE_SPEED = speed;
        BIRDS_NUM = birds;
    }
}

public class GameManager : MonoBehaviour
{
    #region �V���O���g����

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private int numStars;      //�������������
    private int numStage;    //�����X�e�[�W�ڂȂ̂�
    private int passCount;   //�e�X�e�[�g���ƂɈ�񂾂����������������Ȃ��Ƃ��Ɏg��
    private float SCORE;    //�X�R�A
    private float HIGH_SCORE;    //�n�C�X�R�A

    public TimeManager timeManager;
    private StageDifficultyComponent stage;


    // Start is called before the first frame update
    void Start()
    {
        numStars = 0;
        passCount = 0;
        numStage = 1;
        HIGH_SCORE = 0;
        timeManager = GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DifficulityManager(numStage);
        EndApplication();
    }

    public void InitializeGame()
    {
        numStars = 0;
        passCount = 0;
        numStage = 1;
        timeManager.InitializeTime();
    }

    //�X�e�[�W�̓�Փx�������s���֐�
    //�������F�����X�e�[�W�ڂ�
    void DifficulityManager(int stageNumber)
    {
        switch(stageNumber)
        {
            //���t���[��new����̂̓���������������̂Ŋe�v�f���ƂɈ�񂾂��C���X�^���X�𐶐�����
            case 1:
                if(passCount >= 1)
                {
                    return;
                }

                stage = new StageDifficultyComponent(20, 7, 6);
                passCount++;
                break;
            case 2:
                if(passCount >= 2)
                {
                    return;
                }

                stage = new StageDifficultyComponent(18, 10, 10);
                passCount++;
                break;
            case 3:
                if(passCount >= 3)
                {
                    return;
                }

                stage = new StageDifficultyComponent(15, 15, 13);
                passCount++;
                break;
            case 4:
                if (passCount >= 4)
                {
                    return;
                }

                //�Q�[���N���A
                Result();
                passCount++;
                break;
        }
    }

    //�A�v���P�[�V�������I��������֐�
    public void EndApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    //�Q�[���̓�Փx���Ǘ�����N���X�̃C���X�^���X���擾
    public StageDifficultyComponent GetStageDifficultComponent()
    {
        return stage;
    }

    public void IncrementNumStars()
    {
        //�擾�������̐����C���N�������g
        numStars++;
    }

    public void Result()
    {
        //�X�R�A���v�Z
        SCORE = CalScore(timeManager.GetGameTime(), numStars);
        //���U���g��ʂɈڍs
        SceneManager.LoadScene("Result");
    }

    public float CalScore(float time, int numStar)
    {
        // �����_�ȉ��؂�̂�
        return Mathf.FloorToInt(time * numStar);
    }

    //���擾���Ă��鐯�̐����擾
    public int GetNumStars()
    {
        return numStars;
    }

    //�����X�e�[�W�ڂȂ̂����擾����
    public int GetStageNumber()
    {
        return numStage;
    }

    //�X�e�[�W�ԍ����P���₷
    public void IncrementStageNumber()
    {
        numStage++;
    }

    //�X�R�A���擾
    public int GetTotalScore()
    {
        return Mathf.FloorToInt(SCORE);
    }

    //�n�C�X�R�A���Z�b�g
    public void SetHighScore(int score)
    {
        HIGH_SCORE = score;
    }

    //�n�C�X�R�A���擾
    public float GetHighScore()
    {
        return HIGH_SCORE;
    }
}