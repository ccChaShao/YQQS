using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMananger : MonoBehaviour
{
    [Header("当前子弹属性")]
    public float force;
    public float bullet_value;
    [Header("组件管理")]
    public AudioSource hit;
    private Animator anim;
    private float bullet_a_value = 8;
    private float bullet_b_value = 2;
    private float bullet_laser_value = 6;
    public enum bullet { bullet_a, bullet_b, bullet_laser };
    public bullet bulletChoose;
    private Rigidbody2D rb;
    protected virtual void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 1.0f);

        FollowMouseRotate();
        BulletChoose();
    }
    protected virtual void FixedUpdate()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        rb.AddForce(transform.right * force);
    }
    protected void FollowMouseRotate()
    {
        //获取鼠标的坐标，鼠标是屏幕坐标，Z轴为0，这里不做转换  
        Vector3 mouse = Input.mousePosition;
        //获取物体坐标，物体坐标是世界坐标，将其转换成屏幕坐标，和鼠标一直  
        Vector3 obj = Camera.main.WorldToScreenPoint(transform.position);
        //屏幕坐标向量相减，得到指向鼠标点的目标向量  
        Vector3 direction = mouse - obj;
        //将Z轴置0,保持在2D平面内  
        direction.z = 0f;
        //将目标向量长度变成1，即单位向量，这里的目的是只使用向量的方向，不需要长度，所以变成1  
        direction = direction.normalized;
        //物体自身的Y轴和目标向量保持一直，这个过程XY轴都会变化数值  
        transform.right = direction;
    }
    protected virtual void BulletChoose()//用于选择子弹类型
    {
        switch (bulletChoose)
        {
            case bullet.bullet_a:
                bullet_value = bullet_a_value;
                break;
            case bullet.bullet_b:
                bullet_value = bullet_b_value;
                break;
            case bullet.bullet_laser:
                bullet_value = bullet_laser_value;
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(isCrit())
            {
                enemy.currentLife -= bullet_value*2;//暴击乘2伤害
            }
            else
            {
                enemy.currentLife -= bullet_value;//生命值扣除
            }
            // Debug.Log(enemy.currentLife);
            hit.Play();
            anim.SetBool("Hit", true);
            rb.simulated = false;//马上停止力的模拟（停下来）
        }
    }
    protected virtual void BoomAnim()
    {
        Destroy(gameObject);
    }
    protected virtual bool isCrit()
    {
        float value = Random.Range(0, 20);
        if (value == 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
