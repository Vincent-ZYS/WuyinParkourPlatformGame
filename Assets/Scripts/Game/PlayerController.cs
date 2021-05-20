using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// The flag to judge player move left or not.
    /// </summary>
    private bool isMoveLeft = false;
    /// <summary>
    /// The flag to judge player is jumping or not.
    /// </summary>
    private bool isJumping = false;
    /// <summary>
    /// The next left Spawned Platform Position.
    /// </summary>
    private Vector3 nextLeftPlatformPos;
    /// <summary>
    /// The next right Spawned Platform Position.
    /// </summary>
    private Vector3 nextRightPlatformPos;
    /// <summary>
    /// The configuration parameter management container.
    /// </summary>
    private ManagerVars varsContainer;

    private void Awake()
    {
        varsContainer = ManagerVars.GetManagerVarsContainer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance().isGameStart || GameManager.Instance().isGameOver) { return; }
        ClickLeftScreenToMove();
    }

    private void ClickLeftScreenToMove()
    {
        if(Input.GetMouseButtonDown(0) && !isJumping)
        {
            if(Input.mousePosition.x <= Screen.width/2)
            {
                isMoveLeft = true;
            }else if(Input.mousePosition.x > Screen.width/2)
            {
                isMoveLeft = false;
            }
            PlayerJump();
        }
    }

    private void PlayerJump()
    {
        isJumping = true;
        if (isMoveLeft)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            DoTweenPlayerJumpAnimation(nextLeftPlatformPos);
        } else
        {
            transform.localScale = Vector3.one;
            DoTweenPlayerJumpAnimation(nextRightPlatformPos);
        }
        EventCenter.BroadCast(EventType.DecidePath);
    }

    private void DoTweenPlayerJumpAnimation(Vector3 jumpDirection)
    {
        transform.DOMoveX(jumpDirection.x, 0.2f);
        transform.DOMoveY(jumpDirection.y + 0.8f, 0.15f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Platform")
        {
            Vector3 currentPlatformPos = collision.transform.position;
            nextLeftPlatformPos = new Vector3(currentPlatformPos.x - varsContainer.nextPosX, 
                currentPlatformPos.y + varsContainer.nextPosX,0f);
            nextRightPlatformPos = new Vector3(currentPlatformPos.x + varsContainer.nextPosX,
                currentPlatformPos.y + varsContainer.nextPosX,0f);
            isJumping = false;
        }
    }
}
