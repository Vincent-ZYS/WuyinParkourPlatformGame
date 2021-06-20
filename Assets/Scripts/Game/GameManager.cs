using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The instance object to control this singleton instance.
    /// </summary>
    private static GameManager _instance;
    /// <summary>
    /// The Flag to judge current game is start or not.
    /// </summary>
    public bool isGameStart { get; set; }
    /// <summary>
    /// The Flag to judge current game is over or not.
    /// </summary>
    public bool isGameOver { get; set; }
    /// <summary>
    /// The Flag to judge current gasme is paused or not.
    /// </summary>
    public bool isGamePause { get; set; }
    /// <summary>
    /// To store current player's score.
    /// </summary>
    public int playerScore { get; set; }
    /// <summary>
    /// To store current player's diamond count.
    /// </summary>
    public int playerDiamondCount { get; set; }

    private void Awake()
    {
        EventCenter.AddListner(EventType.AddPlayerScore, AddCurrentPlayerScore);
        EventCenter.AddListner(EventType.AddDiamondCount, AddCurrentPlayerDiamondCount); 
        GameDataInitiate();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.AddPlayerScore, AddCurrentPlayerScore);
        EventCenter.RemoveListener(EventType.AddDiamondCount, AddCurrentPlayerDiamondCount);
    }
    public static GameManager Instance()
    {
        if(_instance==null)
        {
            _instance = FindObjectOfType<GameManager>();
        }
        return _instance;
    }

    private void GameDataInitiate()
    {
        //Data initiate
        playerScore = 0;
        playerDiamondCount = 0;
    }

    public void AddCurrentPlayerScore()
    {
        //TODO more efficienct way?
        playerScore += 5;
    }

    public void AddCurrentPlayerDiamondCount()
    {
        //TODO more efficienct way?
        playerDiamondCount++;
    }
}
