using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPanel : MonoBehaviour
{
    private Button startGame_Btn;
    private Button skinStore_Btn;
    private Button rank_Btn;
    private Button sound_Btn;

    private void Awake()
    {
        Initiation();
    }

    /// <summary>
    /// The initiation function of Main menu UI panel.
    /// </summary>
    private void Initiation()
    {
        startGame_Btn = transform.Find("DownPart_go/StartBg_btn").GetComponent<Button>();
        startGame_Btn.onClick.AddListener(OnStartGameBtnClick);
        skinStore_Btn = transform.Find("DownPart_go/OtherBtn_group/SkinStoreBg_btn").GetComponent<Button>();
        skinStore_Btn.onClick.AddListener(OnSkinStoreBtnClick);
        rank_Btn = transform.Find("DownPart_go/OtherBtn_group/RankBg_btn").GetComponent<Button>();
        rank_Btn.onClick.AddListener(OnRankBtnClick);
        sound_Btn = transform.Find("DownPart_go/OtherBtn_group/SoundBg_btn").GetComponent<Button>();
        sound_Btn.onClick.AddListener(OnSoundBtnClick);
    }

    /// <summary>
    /// The start game button click function.
    /// </summary>
    private void OnStartGameBtnClick()
    {
        EventCenter.BroadCast(EventType.ShowGamePanel);
        gameObject.SetActive(false);
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
