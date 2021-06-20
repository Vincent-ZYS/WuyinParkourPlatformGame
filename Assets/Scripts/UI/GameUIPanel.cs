using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameUIPanel : MonoBehaviour
{
    private Text currentScore_Txt;
    private Text diamondCount_Txt;
    private Button pause_Btn;
    private Image startGame_Img;

    private RectTransform panelFstPartRctf;

    private void Awake()
    {
        EventCenter.AddListner<bool>(EventType.ShowGamePanel, ShowGameUIPanel);
        EventCenter.AddListner<int>(EventType.UpdatePlayerUIScore, UpdatePlayerUIScore);
        EventCenter.AddListner<int>(EventType.UpdatePlayerDiamondUICount, UpdateDiamondUICount);
        Initiation();
    }

    /// <summary>
    /// The Game UI Panel initation function.
    /// </summary>
    private void Initiation()
    {
        currentScore_Txt = transform.Find("AbovePart_go/CurrentScore_txt").GetComponent<Text>();
        diamondCount_Txt = transform.Find("AbovePart_go/DiamondIcon_img/DiamondCount_txt").GetComponent<Text>();
        pause_Btn = transform.Find("AbovePart_go/PauseGame_btn").GetComponent<Button>();
        pause_Btn.onClick.AddListener(OnPauseGameBtnClick);
        startGame_Img = transform.Find("AbovePart_go/PauseGame_btn/StartGame_img").GetComponent<Image>();
        currentScore_Txt.text = "0";
        diamondCount_Txt.text = "0";
        startGame_Img.enabled = false;
        panelFstPartRctf = transform.Find("AbovePart_go").GetComponent<RectTransform>();
    }

    /// <summary>
    /// The function to activate current UI panel object.
    /// </summary>
    private void ShowGameUIPanel(bool isShow)
    {
        if (isShow)
        {
            panelFstPartRctf.DOAnchorPos(new Vector2(0f, -50f), 0.5f);
        }
        else
        {
            panelFstPartRctf.DOAnchorPos(new Vector2(480f, -50f), 0.5f);
        }
    }

    private void UpdatePlayerUIScore(int curScore)
    {
        currentScore_Txt.text = curScore.ToString();
    }

    private void UpdateDiamondUICount(int curCount)
    {
        diamondCount_Txt.text = curCount.ToString();
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
        EventCenter.RemoveListener<bool>(EventType.ShowGamePanel, ShowGameUIPanel);
        EventCenter.RemoveListener<int>(EventType.UpdatePlayerUIScore, UpdatePlayerUIScore);
        EventCenter.RemoveListener<int>(EventType.UpdatePlayerDiamondUICount, UpdateDiamondUICount);
    }
}
