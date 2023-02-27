using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Actor
{
    private Rigidbody _rb;      //重力
    private readonly float MOVE_SPEED_Z = 3;      //Z軸方向の移動速度 
    private readonly float MOVE_SPEED_X;      //X軸方向の移動速度 
    private readonly float MOVE_SPEED_Y;      //Y軸方向の移動速度 
    private readonly float DIS_CAMERA = 4;      //Y軸方向の移動速度 
    private readonly float GET_MOUSE_TIME = 0.8f;      //何秒おきにプレイヤーの位置を更新するか
    private Vector3 previousPos;
    private Vector3 differenceMousePos;
    private Animator _animator;
    private AnimationState _animState;
    private bool _getMousePos;

    //アニメーションの状態
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

    //プレイヤーの動きの関数
    public override void Move()
    {
        //マウスのワールド座標を取得
        Vector3 playerDirection = GetMouseInput();
        //Z軸方向にスピードを加算し、プレイヤーの最終的な移動位置を取得
        playerDirection += new Vector3(0, 0, MOVE_SPEED_Z) * Time.deltaTime;
        //実際にプレイヤーを移動
        transform.position = playerDirection;
        //プレイヤーを追従するためにカメラがいるべき座標を取得
        var cameraPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, playerDirection.z - DIS_CAMERA);
        //実際にカメラを移動
        Camera.main.transform.position = cameraPos;
    }

    //マウスカーソルのワールド座標を取得する関数
    private Vector3 GetMouseInput()
    {
        //マウスのスクリーン座標を取得
        var mousePos = Input.mousePosition;
        //マウスのスクリーン座標からワールド座標を取得
        var returnPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, DIS_CAMERA));
        //マウスのワールド座標を返す
        return returnPos;
    }

    //マウスが動いた方向のベクトルを取得
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

        //0.5秒ごとにマウスの位置を取得したい->アニメーション再生を安定させるため
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

    //アニメーションを管理する関数
    void AnimationController()
    {
        //マウスの移動ベクトルを取得
        GetMouseMoveVector();

        //X軸上の内積を取得
        var dotX = Vector3.Dot(Vector3.right, differenceMousePos.normalized);
        //Y軸上の内積を取得
        var dotY = Vector3.Dot(Vector3.up, differenceMousePos.normalized);

        //もしもX軸を基準に取得した内積がcos45より大きければ
        if (dotX > GetCos(45))
        {
            //if (!_right)
            AnimationTransion(AnimationState.Right);
        }
        //もしもX軸を基準に取得した内積がcos135より大きければ
        else if (dotX < GetCos(135))
        {
            AnimationTransion(AnimationState.Left);
        }
        //もしもX軸を基準に取得した内積が上記にあてはまらなかったら
        else
        {
            //Y軸を基準に取得した内積が0より大きければ
            if(dotY > 0)
            {
                AnimationTransion(AnimationState.Up);
            }
            //Y軸を基準に取得した内積が0より小さければ
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

    //指定した「角度」のcosの値を返す関数
    float GetCos(float angle)
    {
        angle *= Mathf.Deg2Rad;
        return Mathf.Cos(angle);
    }

    //アニメーションをステートを入力するだけで再生することができる関数
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

//オブジェクト基底クラス
public class Actor : MonoBehaviour
{
    //プレイヤーのインスタンスを宣言
    protected PlayerController Instance;

    private void Awake()
    {
        //プレイヤーのインスタンスを取得
        Instance = FindObjectOfType<PlayerController>();
    }

    public virtual void Move() { }
}
