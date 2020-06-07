using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatRoom : MonoBehaviour
{
    public enum Direction { up, down, lef, right };//枚举型，你要用哪个变量来代表Direction，相当于tag下拉菜单
    public Direction direction;//创建一个枚举型的变量

    public GameObject Cyclops_Prefab;
    [Header("房间信息")]
    public List<GameObject> floorPrefab = new List<GameObject>();//地板预制体的数组
    public int roomNumber;
    public Color startColor, endColor;
    private GameObject endRoom;
    [Header("位置控制")]
    public Transform createPoint;//生成房间的点
    public float xOffset, yOffset;
    public LayerMask roomLayer;
    public List<Room> rooms = new List<Room>();//创建一个Room型的列表,里面存的那个游戏目标的Room脚本
    public int MaxStep;//存放最大距离的房间的距离数
    [Header("房间数组")]
    public List<GameObject> farRoom = new List<GameObject>();//用于存放最远的房间的数组
    public List<GameObject> lessRoom = new List<GameObject>();//用于存放最远的房间的附近，周围的房间
    public List<GameObject> oneWayRoom = new List<GameObject>();//用于存放上面两个列表当中只有单侧门的房间
    public WallType wallType;
    void Start()
    {
        CreatFloor();//调用生成地板的函数

        foreach (var room in rooms)//用于给控制每个房间的房门.按顺序遍历数组rooms,rooms数组里面的存的是每个房间自己的Room类
        {
            SetupRoom(room, room.transform.position);//用于控制房间的门和要生成的预制体
        }

        FindEndRoom();//调用寻找最后一件房间

        rooms[0].GetComponent<SpriteRenderer>().color = startColor;//给第一件房间颜色
        endRoom.GetComponent<SpriteRenderer>().color = endColor;//给最后一件房间颜色
    }
    public void ChangePointPos()
    //用于判断是随机方向的生成，随机更改生成点的位置
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);//强制转换为枚举型的数值类型

            switch (direction)
            {
                case Direction.up://direction的值，当他等于0时是等于Direction.up
                    {
                        createPoint.position += new Vector3(0, yOffset, 0);
                        break;
                    }
                case Direction.down://direction的值，当他等于1时是等于Direction.down
                    {
                        createPoint.position += new Vector3(0, -yOffset, 0);
                        break;
                    }
                case Direction.lef:
                    {
                        createPoint.position += new Vector3(-xOffset, 0, 0);
                        break;
                    }
                case Direction.right:
                    {
                        createPoint.position += new Vector3(xOffset, 0, 0);
                        break;
                    }
            }
        } while (Physics2D.OverlapCircle(createPoint.position, 0.2f, roomLayer));//作用是让生成点找到没有重复位置的地方，当生成点碰到房间时，则返回true再次进入循环再位移一次...重复下去直到没有重复的房间
    }
    public void SetupRoom(Room newRoom, Vector3 roomPos)
    //用于设置房间的墙壁，门的那个预制体
    {
        newRoom.roomUp = Physics2D.OverlapCircle(roomPos + new Vector3(0, yOffset, 0), 0.2f, roomLayer);//方向上有房则开门
        newRoom.roomBottom = Physics2D.OverlapCircle(roomPos + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPos + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPos + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);

        newRoom.UpdataRoom(xOffset, yOffset);//调用房间信息更新方法

        switch (newRoom.doorNum)
        {
            case 1://只有一个门的情况
                if (newRoom.roomUp)
                    Instantiate(wallType.singleUp, roomPos, Quaternion.identity);
                if (newRoom.roomBottom)
                    Instantiate(wallType.singleBottom, roomPos, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(wallType.singleLeft, roomPos, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(wallType.singleRight, roomPos, Quaternion.identity);
                break;
            case 2://有两个门的情况
                if (newRoom.roomLeft && newRoom.roomUp)//左上
                    Instantiate(wallType.doubleLU, roomPos, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomBottom)//左下
                    Instantiate(wallType.doubleLB, roomPos, Quaternion.identity);
                if (newRoom.roomRight && newRoom.roomBottom)//右下
                    Instantiate(wallType.doubleRB, roomPos, Quaternion.identity);
                if (newRoom.roomRight && newRoom.roomUp)//右上
                    Instantiate(wallType.doubleUR, roomPos, Quaternion.identity);
                if (newRoom.roomRight && newRoom.roomLeft)//左右
                    Instantiate(wallType.doubleLR, roomPos, Quaternion.identity);
                if (newRoom.roomBottom && newRoom.roomUp)//上下
                    Instantiate(wallType.doubleUB, roomPos, Quaternion.identity);
                break;
            case 3://有三个门的情况
                if (newRoom.roomRight && newRoom.roomUp && newRoom.roomLeft)//左右上
                    Instantiate(wallType.tripleLUR, roomPos, Quaternion.identity);
                if (newRoom.roomRight && newRoom.roomBottom && newRoom.roomLeft)//左右下
                    Instantiate(wallType.tripleLRB, roomPos, Quaternion.identity);
                if (newRoom.roomBottom && newRoom.roomUp && newRoom.roomLeft)//上下左
                    Instantiate(wallType.tripleLUB, roomPos, Quaternion.identity);
                if (newRoom.roomRight && newRoom.roomUp && newRoom.roomBottom)//上下右
                    Instantiate(wallType.tripleURB, roomPos, Quaternion.identity);
                break;
            case 4://有四个门的亲狂
                Instantiate(wallType.fourDoors, roomPos, Quaternion.identity);
                break;
        }
    }
    public void FindEndRoom()
    //用于找到最后一个房间
    {
        for (int i = 0; i < rooms.Count; i++)//循环获取到最大的距离值
        {
            if (rooms[i].stepToStart > MaxStep)
            {
                MaxStep = rooms[i].stepToStart;
            }
        }
        // Debug.Log(MaxStep);

        foreach (var room in rooms)
        {
            if (room.stepToStart == MaxStep)//获取最大值房间
            {
                farRoom.Add(room.gameObject);
            }
            if (room.stepToStart == MaxStep - 1)//获取次大值房间
            {
                lessRoom.Add(room.gameObject);
            }
        }

        for (int i = 0; i < farRoom.Count; i++)//在2个数组中找到只有单侧门的房间
        {
            if (farRoom[i].GetComponent<Room>().doorNum == 1)
            {
                oneWayRoom.Add(farRoom[i]);
            }
        }
        for (int i = 0; i < lessRoom.Count; i++)//在2个数组中找到只有单侧门的房间
        {
            if (lessRoom[i].GetComponent<Room>().doorNum == 1)
            {
                oneWayRoom.Add(lessRoom[i]);
            }
        }

        if (oneWayRoom.Count != 0)//找到最终的门
        {
            endRoom = oneWayRoom[Random.Range(0, oneWayRoom.Count)];
        }
        else
        {
            endRoom = farRoom[Random.Range(0, farRoom.Count)];
        }
        
        GameMananger.instance_GameMananger.CreatKey(endRoom.transform);//生成demo通关钥匙
    }
    public void CreatFloor()
    //用于生成不同的地板，地板相当于一间房
    {
        for (int i = 0; i < roomNumber; i++)
        {
            int rangeNum = Random.Range(0, 3);
            // Debug.Log(rangeNum);
            rooms.Add(Instantiate(floorPrefab[rangeNum], createPoint.position, Quaternion.identity).GetComponent<Room>());//实例化之后将他们对应的room脚本添加到数组中，roomprefab是地板

            GameMananger.instance_GameMananger.CyclopsCreat(createPoint);//生成地板的同时在随机位置生成巨人

            ChangePointPos();//重新改变point的位置(相当于改变房间生成的位置)
        }
    }
}
[System.Serializable]
public class WallType
//用于获取所有墙壁样式的gameobject
{
    public GameObject singleLeft, singleRight, singleUp, singleBottom,
                        doubleLU, doubleLB, doubleUR, doubleUB, doubleLR, doubleRB,
                        tripleLUR, tripleLUB, tripleURB, tripleLRB,
                        fourDoors;
}