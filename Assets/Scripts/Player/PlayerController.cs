using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : SingletonMono<PlayerController>
{
    private Rigidbody2D body2d;
    private Animator animator;
    private Collider2D collider2d;
    CinemachineImpulseSource impulseSource;
   
    //移动
    private bool isUpGround;       //是否顶墙
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
    private bool jumpPressed;       //按下跳跃按钮
    private bool jumpPressing;     //按住跳跃按钮
    private bool downJumpPressed;
    private bool isJump;
    private bool isDownJump;
    bool isFalling;       //是否下落
    [SerializeField,Header("跳跃的最大高度")] float jumpMax = 2.5f;
    [SerializeField, Header("跳跃的最小高度")] float jumpMin = 0.5f;
    [SerializeField,Header("跳跃最大速度")] float jumpSpeed = 18;
    [SerializeField,Header("跳跃加速度")] float jumpAcceration = 10;
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
        body2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        prePos = transform.position;
        
        OriginHighMax = jumpMax;
        OriginSpeedMax = m_speed;


    }
    void Update()
    {
        if (isDeath) return;
        UpdateAnim();
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

    public void OnIsJumping(InputValue value)
    {
        jumpPressing = value.isPressed;
    }

    public void OnDownJump(InputValue value)
    {
        if (isPlatform && platformEffector)
        {
            jumpPressed = false;
            downJumpPressed = true;
        }
    }

    public void OnAttract(InputValue value)
    {
        
    }
    public void OnDichotomy(InputValue value)
    {
        
    }
    public void OnAccerlerate(InputValue value)
    {
        
    }

    public void OnInteract(InputValue value)
    {
        isInteracted = value.isPressed;
    }
    private void FixedUpdate()
    {
        CheckGround();

        GroundMovement();
        Jump();
        DownJump();
        
    }

    private void UpdateAnim()
    {
        animator.SetFloat("XSpeed", Mathf.Abs(body2d.velocity.x));
        animator.SetFloat("YSpeed",body2d.velocity.y);
        isFalling = !isGround && body2d.velocity.y < -0.5;
        animator.SetBool("IsFalling",isFalling);
    }

    void CheckGround()
    {
        var colliders = Physics2D.OverlapBoxAll(transform.position+collider2d.bounds.size.y*Vector3.up, new Vector2(1.7f, 0.1f), 0, groundLayer);
        isUpGround = false;
        foreach (var col in colliders)
        {
            if (!col.GetComponent<PlatformEffector2D>())
            {
                isUpGround = true;
                break;
            }
        }
        
        colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(1.7f, 0.1f), 0, groundLayer);
        isGround = colliders.Length > 0;
        animator.SetBool("isGround",isGround);
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
            if (reverseTime > 0)
             {
             inputX = -inputX;
             }
            
            if (inputX > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                body2d.velocity=new Vector2(Mathf.Min(m_speed * Time.fixedDeltaTime / accelerateTime + body2d.velocity.x,m_speed), body2d.velocity.y);
            }
            else if (inputX < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
                body2d.velocity=new Vector2(Mathf.Max(-m_speed * Time.fixedDeltaTime / accelerateTime + body2d.velocity.x,-m_speed), body2d.velocity.y);
            }
            else
            {
                body2d.velocity = new Vector2(Mathf.MoveTowards(body2d.velocity.x,0,m_speed*Time.fixedDeltaTime/decelerateTIme), body2d.velocity.y);
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
        
        if (!isJump && jumpPressed && (isGround  || graceTimer>0))     //地面起跳
        {
           
            StopCoroutine(IntroJump());
            StartCoroutine(IntroJump());
            jumpCount--;
            jumpPressed = false;
            graceTimer = 0;
        }else if(jumpPressed && jumpCount > 0 && isJump)       //空中起跳
        {
            body2d.velocityY=0;
            StopCoroutine(IntroJump());
            StartCoroutine(IntroJump());
            jumpCount--;
            jumpPressed = false;
        }

        if(!isJump)
            jumpPressed = false;
    }
    IEnumerator IntroJump()
    {
        float dis = 0;
        float startJumpPos = transform.position.y;
        // move up
        float curJumpMin = jumpMin;
        float curJumpMax = jumpMax;
        float curJumpSpeed = jumpSpeed;
        body2d.gravityScale = 0;
        UpdateJump();
        animator.SetTrigger("Jump");
        while (dis <= curJumpMin)
        {
            
            if (isUpGround)   //返回false说明撞到墙，结束跳跃
            {
                body2d.velocityY = 0;
                isJump = false;
                body2d.gravityScale = 1;
                yield break;
            }
            //获取当前角色相对于初始跳跃时的高度
            dis = transform.position.y - startJumpPos;
            if( body2d.velocity.y < curJumpSpeed)
                body2d.velocityY = Mathf.Min(jumpAcceration * Time.fixedDeltaTime + body2d.velocityY,curJumpSpeed);
            UpdateJump();
            //Debug.Log("1\n"+dis);
            yield return new WaitForFixedUpdate();
        }
        body2d.velocityY = Mathf.Min(body2d.velocityY,curJumpSpeed);
        //Debug.Log(jumpPressing);
        while (jumpPressing && dis < curJumpMax)
        {
            if (isUpGround)   //返回false说明撞到墙，结束跳跃
            {
                body2d.velocityY = 0;
                isJump = false;
                body2d.gravityScale = 1;
                yield break;
            }
            
            dis = transform.position.y - startJumpPos;
            body2d.velocity = new Vector2(body2d.velocity.x, curJumpSpeed);
            UpdateJump();
            //Debug.Log("2\n"+dis);
            yield return new WaitForFixedUpdate();
        }
        // slow down
        while (body2d.velocity.y > 0)
        {
            if (dis >= jumpMax)
            {
                body2d.velocity -= slowFallSpeed * Time.fixedDeltaTime*Vector2.up;
            }
            else
            {
                body2d.velocity -= FastFallSpeed* Time.fixedDeltaTime * Vector2.up;
            }
            yield return new WaitForFixedUpdate();
        }
        // fall down
        body2d.velocity = new Vector2(body2d.velocity.x, 0);
        while (!isGround)
        {
            body2d.velocity -= fallSpeed * Vector2.up*Time.fixedDeltaTime;
            body2d.velocity = new Vector2(body2d.velocity.x, Mathf.Clamp(body2d.velocity.y, -fallMaxSpeed, body2d.velocity.y));
            yield return new WaitForFixedUpdate();
        }
        body2d.gravityScale = 1;
    }

    void UpdateJump()
    {
        isJump = true;
        jumpPressed = false;
    }
    void DownJump()
    {
        if (isPlatform && platformEffector && downJumpPressed)
        {
            platformEffector.useColliderMask = true;
        }

        downJumpPressed = false;
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
        //测试暂时注释
        //MeshRenderer mesh= GetComponent<MeshRenderer>();
        //mesh.material.SetFloat("_FillPhase", 0.5f);
        float invicibleTimer = 0;
        while (invicibleTimer<invincibleTime)
        {
            yield return null;
            invicibleTimer+=Time.deltaTime;
            //mesh.material.SetFloat("_FillPhase", Mathf.Lerp(0.5f,0,invicibleTimer/invincibleTime));
        }
        //mesh.material.SetFloat("_FillPhase", 0);
        gameObject.layer = LayerMask.NameToLayer("Player");
    } 
    public void GetHit()
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

    //改变高度效果
    public float OriginHighMax;
    public float SetHighTime;
    public void SetHigh(float jumpHigh,float time)
    {
        jumpMax = jumpHigh;
        SetHighTime = time;
    }
    public void  UpdateHigh()
    {
          if (SetHighTime > 0)
          {
              SetHighTime -= Time.deltaTime;
          }
          else
          {
              SetHighTime = 0;
              jumpMax = OriginHighMax;
          }
    }
    
    //改变速度效果
    public float OriginSpeedMax;
    public float SetSpeedTime;
    public void SetSpeed(float speed,float time)
    {
        m_speed = speed;
        SetSpeedTime = time;
    }
    public void  UpdateSpeed()
    {
          if (SetSpeedTime > 0)
          {
              SetSpeedTime -= Time.deltaTime;
          }
          else
          {
              SetSpeedTime = 0;
              m_speed = OriginSpeedMax;
          }
    }
    
    //左右颠倒效果
    public float reverseTime;
    public void SetReverse(float time)
    {
        reverseTime = time;
    }
    public void UpdateReverseTime()
    {
        reverseTime -= Time.deltaTime;
        if (reverseTime < 0)
        {
            reverseTime = 0;
        }

    }
    void update()
    {
      UpdateHigh();
      UpdateSpeed();
      UpdateReverseTime();
    }
    
}