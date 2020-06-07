using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public static CameraControler instance;//单例
    public float speed = 4.0f;
    public Transform tarGet;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(tarGet != null)
        {
            //摄像机位置超target方向移动，但是要保持我自己的z轴不变，2d游戏默认是场景与摄像机的z轴之间有一个差值，这样才能看到场景
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(tarGet.position.x,tarGet.position.y,transform.position.z),speed*Time.deltaTime);
        }
    }
    public void ChangeTarget(Transform newTarget)
    {
        tarGet = newTarget;//把新地图的位置传过来，让摄像机移动过去
    }
}
