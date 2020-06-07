using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMananger : MonoBehaviour
{
    [Header("公用组件")]
    public GameObject Player;
    public GameObject Map;
    public GameObject Cyclops_Prefab;
    public GameObject Key_Prefab;
    public Text scoreNum;//分数text
    public Text lifeNum;//生命值text
    public AudioSource BGM;//背景音乐喇叭
    public GameObject canvas_UI;
    [Header("游戏状态")]
    public bool isWinDemo;
    public bool isPause;
    private float score;//得分
    private PlayerControler det;
    public static GameMananger instance_GameMananger;//创建单例类对象
    private GameMananger() { }//私有化单例类的构建函数，使得在其他地方不能创建单例类的实例对象
    private void Awake()
    {
        instance_GameMananger = this;
    }
    void Start()
    {
        InstancePlayer();
        //调用生成玩家函数
    }
    void Update()
    {
        MapToggle();//地图的调用

        WinGame();
    }
    public void GameOver(bool isDead)
    //游戏结束
    {
        if (isDead)
        {
            BGM.Stop();
            UIControl ui = canvas_UI.GetComponent<UIControl>();
            ui.LoserGame();
        }
    }
    public void MapToggle()
    //地图的调用函数
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Map.SetActive(!Map.activeSelf);
        }
    }
    public void InstancePlayer()
    //生成玩家函数
    {
        GameObject.Instantiate(Player, new Vector3(0, 0, 0), Quaternion.identity);
    }
    public void ScoreMananger(Collider2D other)
    //分数管理
    {
        if (other.name == "Gold_Big")
        {
            score += 50;
            scoreNum.text = score.ToString();
            Destroy(other.gameObject);
        }
        if (other.name == "Gold_Middle")
        {
            score += 30;
            scoreNum.text = score.ToString();
            Destroy(other.gameObject);
        }
        if (other.name == "Gold_Small")
        {
            score += 10;
            scoreNum.text = score.ToString();
            Destroy(other.gameObject);
        }
    }
    public void LifeMananger(Collision2D other, ref float life)
    //用于管理玩家的血量
    {
        switch (other.gameObject.name)
        {
            case "Cyclops_":
                {
                    life -= 20;
                    lifeNum.text = life.ToString();
                    break;
                }
        }
    }
    public void CyclopsCreat(Transform roomCreatPoint)
    //独眼巨人的生成管理
    {
        float num = Random.Range(0, 4);
        for (int i = 0; i < num; i++)
        {
            Vector2 leftBottomPoint = new Vector2(roomCreatPoint.position.x - 6, roomCreatPoint.position.y - 2);
            Vector2 rightTopPoint = new Vector2(roomCreatPoint.position.x + 6, roomCreatPoint.position.y + 2);
            Vector2 creatCyclopsPoint = new Vector2(Random.Range(leftBottomPoint.x, rightTopPoint.x), Random.Range(leftBottomPoint.y, rightTopPoint.y));
            Transform cyclops = GameObject.Instantiate(Cyclops_Prefab, creatCyclopsPoint, Quaternion.identity).transform.Find("Cyclops_");//获取到生成的巨人的游戏对象
            // Debug.Log(cyclops.name);
            Cyclops cy = cyclops.GetComponent<Cyclops>();
            cy.GenerateDetect(roomCreatPoint);
        }
    }
    public void Repel(Vector3 attacker, float speed, GameObject one)
    //用于击退
    {
        Repel _repel = one.gameObject.AddComponent<Repel>();
        _repel.speed = speed;
        _repel.attacker = attacker;
    }
    public void CreatKey(Transform pos)
    //生成demo的通关钥匙
    {
        Instantiate(Key_Prefab, pos.position, Quaternion.identity);
    }
    public void WinGame()
    {
        if(isWinDemo)
        {
            UIControl ui = canvas_UI.GetComponent<UIControl>();
            ui.WinGame();
        }
    }
}
