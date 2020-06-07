using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtValue_Canvas : MonoBehaviour
{
    public GameObject hurtValue;
    public void HHD(float value)//用于生成伤害数值
    {
        // Debug.Log(transform.position);
        GameObject hub = Instantiate(hurtValue,this.transform);
        hub.GetComponent<Text>().text = "-" + value.ToString();
    }
}
