using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cyclops : Enemy
{
    [Header("Other")]
    public Transform detect;
    public float startWaitTime;
    private float waitTime;
    private Vector2 leftBottomPoint, rightTopPoint;
    protected override void Start()
    {
        base.Start();
        detect.position = UpdataDetect();//一开始便要更新目标点
    }
    protected override void Update()
    {
        base.Update();

        CyclopsIsDead();
    }
    private void FixedUpdate()
    {
        CyclopsMovement();
    }
    public void GenerateDetect(Transform Newdetect)
    //用于获取房间的中心点
    {
        // Debug.Log(Newdetect);
        leftBottomPoint = new Vector2(Newdetect.position.x - 8, Newdetect.position.y - 2);
        rightTopPoint = new Vector2(Newdetect.position.x + 8, Newdetect.position.y + 4);
    }
    public Vector2 UpdataDetect()
    //用于更新随机点
    {
        Vector2 rndPos = new Vector2(Random.Range(leftBottomPoint.x, rightTopPoint.x), Random.Range(leftBottomPoint.y, rightTopPoint.y));
        return rndPos;
    }
    public void CyclopsMovement()
    //怪物移动
    {

        if (detect.position.x < transform.position.x)//面部朝向
        {
            transform.localScale = new Vector3(-1, 1, 0);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 0);
        }

        transform.position = Vector2.MoveTowards(transform.position, detect.position, speed * Time.fixedDeltaTime);//朝目标点移动

        anim.SetFloat("Speed", speed);

        if (Vector2.Distance(transform.position, detect.position) < 0.5f)//如果无限接近，相当于到达了点
        {
            if (waitTime <= 0)//如果时间已经到了
            {
                detect.position = UpdataDetect();//更新目标点
                waitTime = startWaitTime;//重新赋值
            }
            else
            {
                anim.SetFloat("Speed", 0);
                waitTime -= Time.fixedDeltaTime;//否则要等待时间流逝
            }
        }
    }
    protected override void SwitchAnim()
    //用于控制动画切换
    {
        base.SwitchAnim();
    }
    public void CyclopsIsDead()
    //怪物死亡
    {
        if (currentLife <= 0)
        {
            detect.position = this.transform.position;
            GetComponent<Rigidbody2D>().simulated = false;//关闭物理引擎
        }
    }
}
