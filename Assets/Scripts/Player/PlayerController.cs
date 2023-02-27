using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    private Rigidbody _rb;      //�d��
    private readonly float MOVE_SPEED_Z = 3;      //Z�������̈ړ����x 
    private readonly float MOVE_SPEED_X;      //X�������̈ړ����x 
    private readonly float MOVE_SPEED_Y;      //Y�������̈ړ����x 
    private readonly float DIS_CAMERA = 4;      //Y�������̈ړ����x 
    private readonly float GET_MOUSE_TIME = 0.8f;      //���b�����Ƀv���C���[�̈ʒu���X�V���邩
    private Vector3 previousPos;
    private Vector3 differenceMousePos;
    private Animator _animator;
    private AnimationState _animState;
    private bool _getMousePos;

    //�A�j���[�V�����̏��
    private enum AnimationState
    {
        Idle,
        Right,
        Left,
        Up,
        Down
    }

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _animState = new AnimationState();
        AnimationTransion(AnimationState.Idle);
        _getMousePos = false;
    }

    void Update()
    {
        Move();
        AnimationController();
    }

    //�v���C���[�̓����̊֐�
    public override void Move()
    {
        //�}�E�X�̃��[���h���W���擾
        Vector3 playerDirection = GetMouseInput();
        //Z�������ɃX�s�[�h�����Z���A�v���C���[�̍ŏI�I�Ȉړ��ʒu���擾
        playerDirection += new Vector3(0, 0, MOVE_SPEED_Z) * Time.deltaTime;
        //���ۂɃv���C���[���ړ�
        transform.position = playerDirection;
        //�v���C���[��Ǐ]���邽�߂ɃJ����������ׂ����W���擾
        var cameraPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, playerDirection.z - DIS_CAMERA);
        //���ۂɃJ�������ړ�
        Camera.main.transform.position = cameraPos;
    }

    //�}�E�X�J�[�\���̃��[���h���W���擾����֐�
    private Vector3 GetMouseInput()
    {
        //�}�E�X�̃X�N���[�����W���擾
        var mousePos = Input.mousePosition;
        //�}�E�X�̃X�N���[�����W���烏�[���h���W���擾
        var returnPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, DIS_CAMERA));
        //�}�E�X�̃��[���h���W��Ԃ�
        return returnPos;
    }

    //�}�E�X�������������̃x�N�g�����擾
    void GetMouseMoveVector()
    {
        var mousePos = Input.mousePosition;

        if(mousePos != previousPos)
        {
            differenceMousePos = mousePos - previousPos;
        }
        else
        {
            differenceMousePos = Vector3.zero;
        }

        //0.5�b���ƂɃ}�E�X�̈ʒu���擾������->�A�j���[�V�����Đ������肳���邽��
        if(!_getMousePos)
        {
            previousPos = mousePos;
            Invoke(nameof(GetPreviousPos), GET_MOUSE_TIME);
            _getMousePos = true;
        }
    }

    void GetPreviousPos()
    {
        _getMousePos = false;
    }

    //�A�j���[�V�������Ǘ�����֐�
    void AnimationController()
    {
        //�}�E�X�̈ړ��x�N�g�����擾
        GetMouseMoveVector();

        //X����̓��ς��擾
        var dotX = Vector3.Dot(Vector3.right, differenceMousePos.normalized);
        //Y����̓��ς��擾
        var dotY = Vector3.Dot(Vector3.up, differenceMousePos.normalized);

        //������X������Ɏ擾�������ς�cos45���傫�����
        if (dotX > GetCos(45))
        {
            //if (!_right)
            AnimationTransion(AnimationState.Right);
        }
        //������X������Ɏ擾�������ς�cos135���傫�����
        else if (dotX < GetCos(135))
        {
            AnimationTransion(AnimationState.Left);
        }
        //������X������Ɏ擾�������ς���L�ɂ��Ă͂܂�Ȃ�������
        else
        {
            //Y������Ɏ擾�������ς�0���傫�����
            if(dotY > 0)
            {
                AnimationTransion(AnimationState.Up);
            }
            //Y������Ɏ擾�������ς�0��菬�������
            else if (dotY < 0)
            {
                AnimationTransion(AnimationState.Down);
            }
            else
            {
                AnimationTransion(AnimationState.Idle);
            }
        }
    }

    //�w�肵���u�p�x�v��cos�̒l��Ԃ��֐�
    float GetCos(float angle)
    {
        angle *= Mathf.Deg2Rad;
        return Mathf.Cos(angle);
    }

    //�A�j���[�V�������X�e�[�g����͂��邾���ōĐ����邱�Ƃ��ł���֐�
    void AnimationTransion(AnimationState animState)
    {
        switch (animState)
        {
            case AnimationState.Idle:
                _animator.SetBool("Idle", true);
                _animator.SetBool("Right", false);
                _animator.SetBool("Left", false);
                _animator.SetBool("Up", false);
                _animator.SetBool("Down", false);
                break;
            case AnimationState.Right:
                _animator.SetBool("Right", true);
                _animator.SetBool("Left", false);
                _animator.SetBool("Up", false);
                _animator.SetBool("Down", false);
                break;
            case AnimationState.Left:
                _animator.SetBool("Left", true);
                _animator.SetBool("Right", false);
                _animator.SetBool("Up", false);
                _animator.SetBool("Down", false);
                break;
            case AnimationState.Up:
                _animator.SetBool("Right", false);
                _animator.SetBool("Left", false);
                _animator.SetBool("Down", false);
                _animator.SetBool("Up", true);
                break;
            case AnimationState.Down:
                _animator.SetBool("Down", true);
                _animator.SetBool("Right", false);
                _animator.SetBool("Left", false);
                _animator.SetBool("Up", false);
                break;
        }
    }
}

//�I�u�W�F�N�g���N���X
public class Actor : MonoBehaviour
{
    //�v���C���[�̃C���X�^���X��錾
    protected PlayerController Instance;

    private void Awake()
    {
        //�v���C���[�̃C���X�^���X���擾
        Instance = FindObjectOfType<PlayerController>();
    }

    public virtual void Move() { }
}
