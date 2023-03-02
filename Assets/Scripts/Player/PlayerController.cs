using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


public class PlayerController : Actor
{
    private Rigidbody _rb;      //重力
    private readonly float MOVE_SPEED_Z = 10;      //Z軸方向の移動速度 
    private readonly float MOVE_SPEED_X;      //X軸方向の移動速度 
    private readonly float MOVE_SPEED_Y;      //Y軸方向の移動速度 
    private readonly float DIS_CAMERA = 4;      //Y軸方向の移動速度 
    private readonly float GET_MOUSE_TIME = 0.8f;      //何秒おきにプレイヤーの位置を更新するか
    private Vector3 previousPos;                   //更新前のプレイヤーの位置を取得
    private Vector3 differenceMousePos;            //マウスがどれだけ動いたかを格納する
    private Animator _animator;
    private bool _getMousePos;                    //マウスの位置を一フレームだけ取得したいときに使うフラグ
    [SerializeField] private ParticleSystem star;  //スター取得時のエフェクト
    [SerializeField] private AudioClip getStar;    //鳥にぶつかったときの音
    [SerializeField] private AudioClip dead;      //鳥にぶつかったときの音
    [SerializeField] private AudioSource audioSource;
    private readonly float MAX_X_RIGHT = 1500;
    private readonly float MIN_X_LEFT = 420;
    private readonly float MAX_Y_TOP = 1080;
    private readonly float MIN_Y_BUTTOM = 0;

    //アニメーションの状態
    private enum AnimationState
    {
        Idle,     //何も動きがない状態
        Right,    //右に進行中
        Left,     //左に進行中
        Up,       //上に進行中
        Down      //下に進行中
    }

    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        AnimationTransion(AnimationState.Idle);
        _getMousePos = false;
        audioSource = GetComponent<AudioSource>();
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
        playerDirection += new Vector3(0, 0, GameManager.Instance.GetStageDifficultComponent().MOVE_SPEED) * Time.deltaTime;
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

        //マウスカーソルの移動制限
        if (mousePos.x <= MAX_X_RIGHT && mousePos.x >= MIN_X_LEFT && mousePos.y >= MIN_Y_BUTTOM && mousePos.y <= MAX_Y_TOP)
        {
            //マウスのスクリーン座標からワールド座標を取得
            var returnPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, DIS_CAMERA));
            //マウスのワールド座標を返す
            return returnPos;
        }
        else
        {
            //マウスカーソルが制限範囲を超えてしまったときはプレイヤーの位置は制限範囲内に収まるようにする
            float x;
            float y;
            if(mousePos.x <= MIN_X_LEFT)
            {
                x = MIN_X_LEFT;
            }
            else if(mousePos.x >= MAX_X_RIGHT)
            {
                x = MAX_X_RIGHT;
            }
            else
            {
                x = mousePos.x;
            }

            if(mousePos.y <= MIN_Y_BUTTOM)
            {
                y = MIN_Y_BUTTOM;
            }
            else if (mousePos.y >= MAX_Y_TOP)
            {
                y = MAX_Y_TOP;
            }
            else
            {
                y = mousePos.y;
            }

            var returnPos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, DIS_CAMERA));
            return returnPos;
        }
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

    //プレイヤーの更新前の位置を取得するときに使うフラグを取得する関数
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

    //カメラとプレイヤーとの距離を取得する関数
    public float GetDistanceCamera()
    {
        return DIS_CAMERA;
    }

    private void OnTriggerEnter(Collider other)
    {
        //鳥とぶつかったら
        if (other.gameObject.CompareTag("Bird"))
        {
            //リザルト画面に移行
            GameManager.Instance.Result();
            //se再生
            audioSource.PlayOneShot(dead);
        }
        //スタートぶつかったら
        else if (other.gameObject.CompareTag("Star"))
        {
            //取得したスターの数を１増やす
            GameManager.Instance.IncrementNumStars();
            //エフェクト再生(エフェクトは自動的に削除される)
            Instantiate(star, transform);
            star.Play();
            //se再生
            audioSource.PlayOneShot(getStar);
        }
    }
}