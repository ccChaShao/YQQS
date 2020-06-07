using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMap : MonoBehaviour
{
    GameObject mapSprite;//显示对应图片的Sprite
    
    private void OnEnable()
    {
        mapSprite = transform.parent.GetChild(0).gameObject;//等于当前挂载了脚本的父集的第一个

        mapSprite.SetActive(false);//一开始先全部关闭，等玩家进去了之后再去显示
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            mapSprite.SetActive(true);
        }
    }
}
