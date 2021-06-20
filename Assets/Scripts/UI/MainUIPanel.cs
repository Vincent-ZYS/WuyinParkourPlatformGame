using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainUIPanel : MonoBehaviour
{
    private Button startGame_Btn;
    private Button skinStore_Btn;
    private Button rank_Btn;
    private Button sound_Btn;

    private RectTransform panelFstPartRctf, panelSecPartRectf;

    private void Awake()
    {
        EventCenter.AddListner<bool>(EventType.ShowMainPanel, IsShowMainUIPanelOrNot);
        Initiation();
        if(GameManager.isReStartGame)
        {
            OnStartGameBtnClick();
            GameManager.isReStartGame = false;
            return;
        }
        GameManager.Instance().isGameStart = false;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventType.ShowMainPanel, IsShowMainUIPanelOrNot);
    }

    /// <summary>
    /// The initiation function of Main menu UI panel.
    /// </summary>
    private void Initiation()
    {
        startGame_Btn = transform.Find("DownPart_go/Start_btn").GetComponent<Button>();
        startGame_Btn.onClick.AddListener(OnStartGameBtnClick);
        skinStore_Btn = transform.Find("DownPart_go/OtherBtn_group/SkinStore_btn").GetComponent<Button>();
        skinStore_Btn.onClick.AddListener(OnSkinStoreBtnClick);
        rank_Btn = transform.Find("DownPart_go/OtherBtn_group/Rank_btn").GetComponent<Button>();
        rank_Btn.onClick.AddListener(OnRankBtnClick);
        sound_Btn = transform.Find("DownPart_go/OtherBtn_group/Sound_btn").GetComponent<Button>();
        sound_Btn.onClick.AddListener(OnSoundBtnClick);
        panelFstPartRctf = transform.Find("AbovePart_go").GetComponent<RectTransform>();
        panelSecPartRectf = transform.Find("DownPart_go").GetComponent<RectTransform>();
    }

    public void IsShowMainUIPanelOrNot(bool isShow)
    {
        if (isShow)
        {
            panelFstPartRctf.DOAnchorPos(new Vector2(0f, -245f), 0.5f);
            panelSecPartRectf.DOAnchorPos(new Vector2(0f, 216f), 0.5f);
            GetComponent<Image>().DOFade(1f, 0.8f);
        }
        else
        {
            panelFstPartRctf.DOAnchorPos(new Vector2(480f, -245f), 0.5f);
            panelSecPartRectf.DOAnchorPos(new Vector2(-480f, 216f), 0.5f);
            GetComponent<Image>().DOFade(0f, 0.8f);
        }
    }

    /// <summary>
    /// The start game button click function.
    /// </summary>
    private void OnStartGameBtnClick()
    {
        EventCenter.BroadCast(EventType.ShowGamePanel,true);
        EventCenter.BroadCast(EventType.ShowMainPanel, false);
        GameManager.Instance().isGameStart = true;
    }

    private void OnSkinStoreBtnClick()
    {

    }

    private void OnRankBtnClick()
    {

    }

    private void OnSoundBtnClick()
    {

    }
}
