using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("人物属性")]
    public float playerLife = 100;//人物的血量
    public float speed = 20;//速度
    public float hurtForce = 20.0f;//被集中的击退速度
    public float hurtTime = 3.0f;//无敌时间
    [Header("人物状态")]
    public bool isHurt;
    public bool isInvincible;
    public bool isDead;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private float x, y;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");//获取整数
        y = Input.GetAxisRaw("Vertical");
        movement = new Vector2(x, y);

        LifeMonitoring();//玩家生命监控

        SwitchAnim();
        //人物动画的切换
        PlayerScale();
        // 人物面朝像的切换
    }
    private void FixedUpdate()
    {
        if (x != 0 || y != 0)
        {
            rb.MovePosition(transform.position + new Vector3(x, y, 0) * speed * Time.fixedDeltaTime);
        }
    }
    private void SwitchAnim()
    {
        //跑步动画的切换,movement.magnitude涵盖了x和y两个轴
        anim.SetFloat("speed", movement.magnitude);

        if (isDead)
        {
            // isHurt = false;
            anim.SetBool("Dead", isDead);
        }

        if (isHurt)
        {
            anim.SetBool("Hurt", isHurt);
        }
        else
        {
            anim.SetBool("Hurt", isHurt);
        }
    }
    private void PlayerScale()
    {
        if (movement.x != 0)
        {
            transform.localScale = new Vector3(x, 1, 1);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collection"))
        {
            GameMananger.instance_GameMananger.ScoreMananger(other);
        }
        if(other.CompareTag("Key"))
        {
            GameMananger.instance_GameMananger.isWinDemo = true;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            isHurt = true;
            Debug.Log(other.gameObject.name);
            EnemyCollider(other);
        }
    }
    private void EnemyCollider(Collision2D other)
    //撞击到怪物的管理
    {
        if (!isInvincible)//如果不是无敌状态
        {
            GameMananger.instance_GameMananger.LifeMananger(other, ref playerLife);//进行扣血
            StartCoroutine("InvincibleCountDown", hurtTime);//开启一个无敌倒计时的携程
        }

        isInvincible = true;//扣完血后进入无敌状态

        GameMananger.instance_GameMananger.Repel(other.transform.position, hurtForce, this.gameObject);
    }
    IEnumerator InvincibleCountDown(float time)
    //用于无敌倒计时的携程
    {
        Debug.Log("开启携程");
        while (time >= 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            Debug.Log(time);
        }
        isHurt = false;//并且动画取消
        isInvincible = false;//如果时间到了，则取消无敌
    }
    private void LifeMonitoring()
    //玩家生命检测
    {
        if (playerLife <= 0)
        {
            isDead = true;
        }
    }
    private void PlayerDead()
    //玩家死亡
    {
        GameMananger.instance_GameMananger.GameOver(true);
    }
}
