﻿using System.Collections;
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

    public static GameManager Instance()
    {
        if(_instance==null)
        {
            _instance = FindObjectOfType<GameManager>();
        }
        return _instance;
    }
}