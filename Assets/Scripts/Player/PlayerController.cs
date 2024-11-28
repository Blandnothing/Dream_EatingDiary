using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : SingletonMono<PlayerController>
{
    private Rigidbody2D m_body2d;
    CinemachineImpulseSource impulseSource;
   
    //移动
    private bool isGround;         //是否在地面上，用于跳跃
    private bool isPlatform;       //是否在平台上，用于跳下平台
    [SerializeField] LayerMask groundLayer;
    private PlatformEffector2D platformEffector;
    private float inputX;
    private float graceTimer;
    [SerializeField,Header("土狼时间")] float graceTime;    
    [SerializeField,Header("最大速度")] float m_speed = 4.0f;
    [SerializeField, Header("加速到最大速度时间")] float accelerateTime = 0.1f;
    [SerializeField, Header("减速时间")] float decelerateTIme = 0.05f;
    Vector2 prePos;       //角色上一帧的位置
    [SerializeField] float invincibleTime;
    //跳跃
    public int maxJumpCount = 1;
    private int jumpCount;          //可跳跃次数
    private bool jumpPressed;
    private bool downJumpPressed;
    private bool isJump;
    private bool isDownJump;
    bool isFalling;       //是否下落
    [SerializeField,Header("跳跃的最大高度")] float jumpMax = 2.5f;
    [SerializeField, Header("跳跃的最小高度")] float jumpMin = 0.5f;
    [SerializeField,Header("跳跃速度")] float jumpSpeed = 18;
    [SerializeField,Header("跳跃高度超过跳跃最大高度时的降落速度")] float slowFallSpeed=100f;
    [SerializeField,Header("跳跃高度小于跳跃最大高度时的降落速度")] float FastFallSpeed=200f;
    [SerializeField,Header("落下阶段的降落加速度")] float fallSpeed=150f;
    [SerializeField,Header("落下阶段的最大降落速度")] float fallMaxSpeed=24;
    bool isDeath;
    //交互
    [HideInInspector] public bool isInteracted;

    

    protected override void Awake()
    {
        base.Awake();
        m_body2d = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        prePos = transform.position;
        
    }
    void Update()
    {
        if (isDeath) return;
        
    }

    public void OnMovement(InputValue value)
    {
        inputX = value.Get<float>();
    }

    public void OnJump(InputValue value)
    {
        if (!downJumpPressed && jumpCount>0)
        {
            jumpPressed = true;
        }
    }

    public void OnDownJump(InputValue value)
    {
        if (isPlatform && platformEffector)
        {
            jumpPressed = false;
            downJumpPressed = true;
        }
    }

    public void OnInteract(InputValue value)
    {
        isInteracted = value.isPressed;
    }
    private void FixedUpdate()
    {
        CheckGround();
        isFalling = m_body2d.velocity.y < -0.01;

        GroundMovement();
        Jump();
        DownJump();
    }

    void CheckGround()
    {
        var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.7f, 0.3f), 0, groundLayer);
        isGround = colliders.Length > 0;
        foreach (var collider in colliders)
        {
            var platform = collider.GetComponent<PlatformEffector2D>();
            if (platform != null)
            {
                isPlatform = true;
                if (platform != platformEffector)
                {
                    downJumpPressed = false;
                    if (platformEffector != null)
                        StartCoroutine(StopDownJump(platformEffector.transform));
                    platformEffector = platform;
                }
                return;
            }
        }
        isPlatform = false;
        downJumpPressed = false;
        if(platformEffector!=null)
            StartCoroutine(StopDownJump(platformEffector.transform));
        platformEffector = null;
    }
    void GroundMovement()
    {

            if (inputX > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                m_body2d.velocity=new Vector2(Mathf.Min(m_speed * Time.fixedDeltaTime / accelerateTime + m_body2d.velocity.x,m_speed), m_body2d.velocity.y);
            }
            else if (inputX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                m_body2d.velocity=new Vector2(Mathf.Max(-m_speed * Time.fixedDeltaTime / accelerateTime + m_body2d.velocity.x,-m_speed), m_body2d.velocity.y);
            }
            else
            {
                m_body2d.velocity = new Vector2(Mathf.MoveTowards(m_body2d.velocity.x,0,m_speed*Time.fixedDeltaTime/decelerateTIme), m_body2d.velocity.y);
            }
        
    }
    void Jump()
    {
        if (isGround)
        {
            jumpCount = maxJumpCount;
            isJump = false;
            graceTimer = graceTime;
        }
        else
        {
            graceTimer-=Time.fixedDeltaTime;
        }
        
        if (jumpPressed && (isGround  || graceTimer>0))     //地面起跳
        {
            isJump = true;
            StopCoroutine(IntroJump());
            StartCoroutine(IntroJump());
            jumpCount--;
            jumpPressed = false;
            graceTimer = 0;
        }else if(jumpPressed && jumpCount > 0 && isJump)       //空中起跳
        {
            StopCoroutine(IntroJump());
            StartCoroutine(IntroJump());
            jumpCount--;
            jumpPressed = false;
        } 
    }
    IEnumerator IntroJump()
    {
        float dis = 0;
        float startJumpPos = transform.position.y;
        // move up
        float curJumpMin = jumpMin;
        float curJumpMax = jumpMax;
        float curJumpSpeed = jumpSpeed;
        while (dis <= curJumpMin && m_body2d.velocity.y < curJumpSpeed)
        {
            //if (!CheckUpMove())   //返回false说明撞到墙，结束跳跃
            //{
            //    Velocity.y = 0;
            //    isIntroJump = false;
            //    isMove = true;
            //    yield break;
            //}
            //获取当前角色相对于初始跳跃时的高度
            dis = transform.position.y - startJumpPos;
            m_body2d.velocity += 240 * Time.fixedDeltaTime*Vector2.up;
            yield return new WaitForFixedUpdate();
        }
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, curJumpSpeed);
        while (Input.GetButton("Jump") && dis < curJumpMax)
        {
            dis = transform.position.y - startJumpPos;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, curJumpSpeed);
            yield return new WaitForFixedUpdate();
        }
        // slow down
        while (m_body2d.velocity.y > 0)
        {
            if (dis > jumpMax)
            {
                m_body2d.velocity -= slowFallSpeed * Time.fixedDeltaTime*Vector2.up;
            }
            else
            {
                m_body2d.velocity -= FastFallSpeed* Time.fixedDeltaTime * Vector2.up;
            }
            yield return new WaitForFixedUpdate();
        }
        // fall down
        m_body2d.velocity = new Vector2(m_body2d.velocity.x, 0);
        while (!isGround)
        {
            m_body2d.velocity -= fallSpeed * Vector2.up*Time.fixedDeltaTime;
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, Mathf.Clamp(m_body2d.velocity.y, -fallMaxSpeed, m_body2d.velocity.y));
            yield return new WaitForFixedUpdate();
        }
    }

    void DownJump()
    {
        if (isPlatform && platformEffector && downJumpPressed)
        {
            platformEffector.useColliderMask = true;
        }
    }

    IEnumerator StopDownJump(Transform platform)
    {
        float height = platform.gameObject.GetComponent<Collider2D>().bounds.size.y+GetComponent<Collider2D>().bounds.size.y;
        while ((platform.position-transform.position).magnitude < height)
        {
            yield return null;
        }
        platform.gameObject.GetComponent<PlatformEffector2D>().useColliderMask = false;
    }

    IEnumerator Invincible()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        MeshRenderer mesh= GetComponent<MeshRenderer>();
        mesh.material.SetFloat("_FillPhase", 0.5f);
        float invicibleTimer = 0;
        while (invicibleTimer<invincibleTime)
        {
            yield return null;
            invicibleTimer+=Time.deltaTime;
            mesh.material.SetFloat("_FillPhase", Mathf.Lerp(0.5f,0,invicibleTimer/invincibleTime));
        }
        mesh.material.SetFloat("_FillPhase", 0);
        gameObject.layer = LayerMask.NameToLayer("Player");
    } 
    public void GetHit(Vector2 direction, float attackPower)
    {        
        StopCoroutine(Invincible());
        StartCoroutine(Invincible());
    }
    
        
    IEnumerator Dead()
    {
        impulseSource.GenerateImpulse(1);
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        //EventCenter.Instance.Invoke(EventName.dead);  
        float deadTime = 2;
        float deadTimer = 0;
        while (deadTimer<deadTime)
        {
            deadTimer+=Time.deltaTime;
            GetComponent<SpriteRenderer>().color=(new Color(1, 1, 1, Mathf.Lerp(1, 0, deadTimer / deadTime)));
            yield return null;
        }
    }
    
}