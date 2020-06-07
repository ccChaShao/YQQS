using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public GameObject doorLeft, doorRight, doorUp, doorBottom;

    public bool roomLeft, roomRight, roomUp, roomBottom;//默认是false
    public int stepToStart;//用来存放距离值
    // public Text text;
    public int doorNum;
    void Start()
    {
        OpenDoor();
    }
    public void OpenDoor()
    //用于开门
    {
        doorLeft.SetActive(roomLeft);
        doorRight.SetActive(roomRight);
        doorUp.SetActive(roomUp);
        doorBottom.SetActive(roomBottom);
    }
    public void UpdataRoom(float xOffset, float yOffset)
    //更新房间信息，步数（决定远近），开门数（决定周围房间数量）
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));//算出他们之间的距离,走多少步隔了多少房间

        // text.text = stepToStart.ToString();

        if (roomLeft)
            doorNum++;
        if (roomRight)
            doorNum++;
        if (roomBottom)
            doorNum++;
        if (roomUp)
            doorNum++;
        // Debug.Log(doorNum);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("玩家到达其他房间");
            CameraControler.instance.ChangeTarget(transform);
        }
    }
}
