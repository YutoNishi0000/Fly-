using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int STAGE_PASS_COUNT;

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
    }

    void Update()
    {
        //���Ԃ��v��
        generateTime += Time.deltaTime;

        //�o�ߎ��Ԃ��N�[���^�C����菬����������
        if (generateTime < STAGE_COOL_TIME)
        {
            //���������̏����͍s�킹�Ȃ�
            return;
        }
        //�N�[���^�C�����߂��ăX�e�[�W�𐶐����邱�ƂɂȂ�����
        else if (generateTime >= STAGE_COOL_TIME && generateTime <= (STAGE_COOL_TIME + STAGE_TIME))
        {
            //���ɉ��̏������s��Ȃ�
        }
        //�X�e�[�W�𐶐����I�������
        else if(generateTime > (STAGE_COOL_TIME + STAGE_TIME))
        {
            generateTime = 0;
            Invoke(nameof(NextStage), STAGE_COOL_TIME - 1);
            return;
            //�o�ߎ��Ԃ����������A���̃X�e�[�W�Ɉڍs
        }

        int targetPosIndex = (int)(Target.position.z / GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH);

        if (targetPosIndex + aheadStage > StageIndex)
        {
            StageManager(targetPosIndex + aheadStage);
        }
    }

    void NextStage()
    {
        GameManager.Instance.IncrementStageNumber();
    }

    public void IncrementStagePassCount()
    {
        STAGE_PASS_COUNT++;
    }

    public int GetStagePassCount()
    {
        return STAGE_PASS_COUNT;
    }

    void StageManager(int maps)
    {
        if (maps <= StageIndex)
        {
            return;
        }

        for (int i = StageIndex + 1; i <= maps; i++)//�w�肵���X�e�[�W�܂ō쐬����
        {
            GameObject stage = MakeStage(i, GetStage(GetStageState(GameManager.Instance.GetStageNumber()), stagenum));
            StageList.Add(stage);
        }

        while (StageList.Count > aheadStage + 1)//�Â��X�e�[�W���폜����
        {
            DestroyStage();
        }

        StageIndex = maps;
    }

    GameObject MakeStage(int index, GameObject stageObj)//�X�e�[�W�𐶐�����
    {
        GameObject stageObject = Instantiate(stageObj, new Vector3(0, 0, index * GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH + 2 * GameManager.Instance.GetStageDifficultComponent().STAGE_WIDTH), Quaternion.identity);

        return stageObject;
    }

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

    //
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

    void DestroyStage()
    {
        GameObject oldStage = StageList[0];
        StageList.RemoveAt(0);
        Destroy(oldStage);
    }
}
