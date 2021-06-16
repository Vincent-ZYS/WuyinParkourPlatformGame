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
        EventCenter.AddListner<int>(EventType.UpdatePlayerUIScore, UpdatePlayerUIScore);
        Initiation();
    }

    /// <summary>
    /// The Game UI Panel initation function.
    /// </summary>
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

    /// <summary>
    /// The function to activate current UI panel object.
    /// </summary>
    private void ShowGameUIPanel()
    {
        gameObject.SetActive(true);
    }

    private void UpdatePlayerUIScore(int curScore)
    {
        currentScore_Txt.text = curScore.ToString();
    }

    /// <summary>
    /// Current panel's pause button click logic function.
    /// </summary>
    private void OnPauseGameBtnClick()
    {
        //PAUSE GAME OR NOT
        bool isGamePause = GameManager.Instance().isGamePause;
        pause_Btn.gameObject.GetComponent<Image>().enabled = isGamePause;
        startGame_Img.enabled = !isGamePause;
        GameManager.Instance().isGamePause = !isGamePause;
        Time.timeScale = isGamePause ? 1f : 0f;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.ShowGamePanel, ShowGameUIPanel);
        EventCenter.RemoveListener<int>(EventType.UpdatePlayerUIScore, UpdatePlayerUIScore);
    }
}
