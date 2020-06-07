using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("怪物属性")]
    public float life;
    public float speed;
    public float hurtForcef;
    [Header("怪物状态")]
    public float currentLife;
    public bool isHurt;
    public bool isDead;
    [Header("公用组件")]
    public Canvas canvas;
    [SerializeField]
    private Transform lifeBar;
    [SerializeField]
    private GameObject enemyLife;
    protected Animator anim;
    protected Collider2D coll;
    protected virtual void Start()
    {
        currentLife = life;
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    protected virtual void Update()
    {
        SwitchAnim();
        LifeMonitor();
    }
    protected virtual void LifeMonitor()//生命监测（可以被继承）
    {
        lifeBar.localScale = new Vector3(currentLife / life, 1, 1);
        if (currentLife <= 0)
        {
            isDead = true;
            coll.enabled = false;//关闭碰撞体
            enemyLife.SetActive(false);//生命条消失
        }
    }
    protected virtual void SwitchAnim()//动画切换（可以被继承）
    {
        if (isDead)
        {
            anim.SetBool("Dead", isDead);
            Debug.Log("怪物死亡");
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            BulletMananger bullet = other.gameObject.GetComponent<BulletMananger>();
            // Debug.Log(bullet.bullet_value);
            canvas.GetComponent<HurtValue_Canvas>().HHD(bullet.bullet_value);//生成伤害字体
        }
    }
}
