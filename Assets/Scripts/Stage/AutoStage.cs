using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoStage : Actor
{
    int StageSize = 15;
    int StageIndex;

    public Transform Target;    //�v���C���[�I�u�W�F�N�g
    public GameObject[] stagenum;//�X�e�[�W�̃v���n�u
    public int FirstStageIndex;//�X�^�[�g���ɂǂ̃C���f�b�N�X����X�e�[�W�𐶐�����̂�
    public int aheadStage; //���O�ɐ������Ă����X�e�[�W
    public List<GameObject> StageList = new List<GameObject>();//���������X�e�[�W�̃��X�g
    public static float generateTime;   //�o�ߎ��Ԃ��i�[����ϐ�
    private readonly float STAGE_TIME = 15;
    private readonly float STAGE_COOL_TIME = 15;         //�X�e�[�W�Ԃ̕b��
    [SerializeField] private Text stage;
    [SerializeField] private Text explain;
    [SerializeField] private GameObject frame;
    private readonly float SHOW_TIME = 0.5f;     //���ۂɃX�e�[�W���������鉽�b�O�ɉ��X�e�[�W�ڂ���\���e�L�X�g��\�������邩

    private enum StageState
    {
        None,
        FirstStage,
        SecondStage,
        ThirdStage,
    }

    private StageState stageState;

    void Start()
    {
        stageState = new StageState();
        generateTime = 0;
        StageIndex = FirstStageIndex - 1;
        explain.enabled = true;
        stage.enabled = false;
        frame.SetActive(false);
    }

    void Update()
    {
        //���Ԃ��v��
        generateTime += Time.deltaTime;

        //�o�ߎ��Ԃ��N�[���^�C����菬����������
        if (generateTime < STAGE_COOL_TIME)
        {
            if(generateTime > STAGE_COOL_TIME - 1)
            {
                explain.enabled = false;
                frame.SetActive(true);
            }

            //���������̏����͍s�킹�Ȃ�
            return;
        }
        //�N�[���^�C�����߂��ăX�e�[�W�𐶐����邱�ƂɂȂ�����
        else if (generateTime >= STAGE_COOL_TIME && generateTime <= (STAGE_COOL_TIME + STAGE_TIME))
        {
            stage.enabled = true;
            stage.text = "Level" + GameManager.Instance.GetStageNumber();

            if (generateTime > (STAGE_COOL_TIME + SHOW_TIME))
            {
                stage.enabled = false;
            }
        }
        //�X�e�[�W�𐶐����I�������
        else if(generateTime > (STAGE_COOL_TIME + STAGE_TIME))
        {
            generateTime = 0;
            Invoke(nameof(NextStage), STAGE_COOL_TIME - 3);
            return;
            //�o�ߎ��Ԃ����������A���̃X�e�[�W�Ɉڍs
        }

        //���v���C���[���ǂ̃u���b�N�ɂ��邩
        int targetPosIndex = (int)(Instance.transform.position.z / GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH);

        //�������������������X�e�[�W�̃u���b�N��������v���C���[�̃u���b�N����Ahead�X�e�[�W���傫���ꍇ
        if (targetPosIndex + aheadStage > StageIndex)
        {
            StageManager(targetPosIndex + aheadStage);
        }
    }

    //���̃X�e�[�W�̐i�ނƂ��ɓ�Փx���グ�邽�߂̊֐�
    void NextStage()
    {
        GameManager.Instance.IncrementStageNumber();
    }

    //�X�e�[�W�̐������s���֐�
    //�������F������������u���b�N
    void StageManager(int maps)
    {
        if (maps <= StageIndex)
        {
            return;
        }

        //�w�肵���X�e�[�W�܂ō쐬����
        for (int i = StageIndex + 1; i <= maps; i++)
        {
            GameObject stage = MakeStage(i, GetStage(GetStageState(GameManager.Instance.GetStageNumber()), stagenum));
            StageList.Add(stage);
        }

        //�Â��X�e�[�W���폜����
        while (StageList.Count > aheadStage + 1)
        {
            DestroyStage();
        }

        StageIndex = maps;
    }

    //�X�e�[�W�𐶐�����
    GameObject MakeStage(int index, GameObject stageObj)
    {
        GameObject stageObject = Instantiate(stageObj, new Vector3(0, 0, GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH * (index + 2)), Quaternion.identity);
        return stageObject;
    }

    //�X�e�[�W�̃X�e�[�g�ɉ������u�Q�[���I�u�W�F�N�g�v���擾����֐�
    GameObject GetStage(StageState state, GameObject[] stageType)
    {
        switch(state)
        {
            case StageState.FirstStage:
                return stageType[(int)StageState.FirstStage];
            case StageState.SecondStage:
                return stageType[(int)StageState.SecondStage];
            case StageState.ThirdStage:
                return stageType[(int)StageState.ThirdStage];
            default:
                return stageType[(int)StageState.None];
        }
    }

    //�X�e�[�W�������X�e�[�W�ڂȂ̂���\���u�X�e�[�g�v���擾����֐�
    StageState GetStageState(int STAGE_ID)
    {
        switch(STAGE_ID)
        {
            case 1:
                return StageState.FirstStage;
            case 2:
                return StageState.SecondStage;
            case 3:
                return StageState.ThirdStage;
            default:
                return StageState.None;
        }
    }

    //�X�e�[�W�𐶐����邨�����Ƃ̃I�u�W�F�N�g��j��
    void DestroyStage()
    {
        GameObject oldStage = StageList[0];
        StageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
