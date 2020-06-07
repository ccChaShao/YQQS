using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UIControl : MonoBehaviour
{
    public AudioMixer audioMixer_BGM, audioMixer_Other;
    public GameObject panel_Pause, panel_Win, panel_Lose;
    public void SetVolume_BGM(float value)
    {
        audioMixer_BGM.SetFloat("Volume_BGM", value);
    }
    public void SetVolume_Other(float value)
    {
        audioMixer_Other.SetFloat("Volume_Other", value);
    }
    public void PauseGame()
    //暂停游戏
    {
        GameMananger.instance_GameMananger.isPause = true;
        panel_Pause.SetActive(true);
        Pause();
    }
    public void BackGame()
    //返回游戏
    {
        GameMananger.instance_GameMananger.isPause = false;
        panel_Pause.SetActive(false);
        Going();
    }
    public void WinGame()
    //游戏胜利
    {
        panel_Win.SetActive(true);
    }
    public void QuitGame()
    //退出游戏
    {
        Application.Quit();
    }
    public void LoserGame()
    {
        panel_Lose.SetActive(true);
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Going()
    {
        Time.timeScale = 1;
    }
}
