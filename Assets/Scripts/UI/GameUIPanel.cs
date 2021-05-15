using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIPanel : MonoBehaviour
{
    private Text currentScore_Txt;
    private Text diamondCount_Txt;
    private Button pause_Btn;
    private Image startGame_Img;

    private void Awake()
    {
        EventCenter.AddListner(EventType.ShowGamePanel, ShowGameUIPanel);
        Initiation();
    }

    private void Initiation()
    {
        currentScore_Txt = transform.Find("CurrentScore_txt").GetComponent<Text>();
        diamondCount_Txt = transform.Find("DiamondIcon_img/DiamondCount_txt").GetComponent<Text>();
        pause_Btn = transform.Find("PauseGame_btn").GetComponent<Button>();
        pause_Btn.onClick.AddListener(OnPauseGameBtnClick);
        startGame_Img = transform.Find("PauseGame_btn/StartGame_img").GetComponent<Image>();
        gameObject.SetActive(false);
        startGame_Img.enabled = false;
    }

    private void ShowGameUIPanel()
    {
        gameObject.SetActive(true);
    }

    private void OnPauseGameBtnClick()
    {
        if(!startGame_Img.enabled)
        {
            //TODO PAUSE GAME
            pause_Btn.gameObject.GetComponent<Image>().enabled = false;
            startGame_Img.enabled = true;
        }else
        {
            //TODO START GAME
            pause_Btn.gameObject.GetComponent<Image>().enabled = true;
            startGame_Img.enabled = false;
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowGamePanel, ShowGameUIPanel);
    }
}
