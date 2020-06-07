using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtValue : MonoBehaviour
{
    private float speed = 1.5f;
    private float timer = 0f;//计时器
    private float time = 0.8f;//摧毁时间
    private RectTransform pos;
    private Text value;
    private void Start()
    {
        pos = GetComponent<RectTransform>();
        value = GetComponent<Text>();

        RangePos();
    }
    void Update()
    {
        Scroll();
    }
    private void Scroll()
    //滚动
    {
        transform.Translate(Vector2.up * speed);//向上移动
        timer += Time.deltaTime;
        value.fontSize--;
        value.color = new Color(1, 0, 0, 1 - timer);
        Destroy(gameObject, time);
    }
    private void RangePos()
    //随机位置
    {
        float x = Random.Range(-30, 30);
        float y = Random.Range(36, 60);
        // transform.localPosition = new Vector3(x, y, 0);//相对位置，功能一样
        pos.anchoredPosition3D = new Vector3(x, y, 0);
    }
}
