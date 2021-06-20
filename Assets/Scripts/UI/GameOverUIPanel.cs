using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameOverUIPanel : MonoBehaviour
{
    private RectTransform panelFstPartRctf, panelSecPartRectf;
    private Text curScoreTxt, highestScoreTxt, addDiamondCountTxt, restartTxt;
    private Button rankBtn, homeBtn, restartBtn;

    private void Awake()
    {
        EventCenter.AddListner<bool>(EventType.ShowGameOverPanel, IsShowGameOverUIPanelOrNot);
        Initiation();
    }

    private void Initiation()
    {
        panelFstPartRctf = transform.Find("FirstPart_go").GetComponent<RectTransform>();
        panelSecPartRectf = transform.Find("SecondPart_go").GetComponent<RectTransform>();
        curScoreTxt = transform.Find("FirstPart_go/FinalScore_txt").GetComponent<Text>();
        highestScoreTxt = transform.Find("FirstPart_go/HightScore_txt").GetComponent<Text>();
        addDiamondCountTxt = transform.Find("FirstPart_go/DiamondRecord_go/DiamondCount_txt").GetComponent<Text>();
        restartTxt = transform.Find("SecondPart_go/restart_btn").GetComponent<Text>();
        rankBtn = transform.Find("SecondPart_go/Rank_btn").GetComponent<Button>();
        homeBtn = transform.Find("SecondPart_go/Home_btn").GetComponent<Button>();
        restartBtn = transform.Find("SecondPart_go/restart_btn").GetComponent<Button>();
        rankBtn.onClick.AddListener(OnRankBtnClick);
        homeBtn.onClick.AddListener(OnHomeBtnClick);
        restartBtn.onClick.AddListener(OnRestartBtnClick);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool>(EventType.ShowGameOverPanel, IsShowGameOverUIPanelOrNot);
    }

    public void IsShowGameOverUIPanelOrNot(bool isShow)
    {
        StartCoroutine(IsDelayShowGameOverUIPanelOrNot(isShow));
        curScoreTxt.text = GameManager.Instance().playerScore.ToString();
        //TODO HIGH SCORE
        highestScoreTxt.text = "0";
        addDiamondCountTxt.text = "+" + GameManager.Instance().playerDiamondCount.ToString();
    }

    IEnumerator IsDelayShowGameOverUIPanelOrNot(bool isShow)
    {
        yield return new WaitForSeconds(0.5f);
        if(isShow)
        {
            panelFstPartRctf.DOAnchorPos(new Vector2(0f, -210f), 0.5f);
            panelSecPartRectf.DOAnchorPos(new Vector2(0f, 144f), 0.5f);
            GetComponent<Image>().DOFade(1f, 0.8f);
        }else
        {
            panelFstPartRctf.DOAnchorPos(new Vector2(0f, 210f), 0.5f);
            panelSecPartRectf.DOAnchorPos(new Vector2(0f, -200f), 0.5f);
            GetComponent<Image>().DOFade(0f, 0.8f);
        }
    }

    public void OnRankBtnClick()
    {

    }

    public void OnHomeBtnClick()
    {
        ResetGameStatusData(false, false);
    }

    public void OnRestartBtnClick()
    {
        //TODO Some bugs happened with DOTween
        //DOTween.Clear(true);
        GameManager.isReStartGame = true;
        ResetGameStatusData(false, true);
        //EventCenter.BroadCast(EventType.ShowGamePanel, true);
        //EventCenter.BroadCast(EventType.ShowMainPanel, false);
    }

    private void ResetGameStatusData(bool isGameOver, bool isGameStart)
    {
        GameManager.Instance().isGameOver = isGameOver;
        GameManager.Instance().isGameStart = isGameStart;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
