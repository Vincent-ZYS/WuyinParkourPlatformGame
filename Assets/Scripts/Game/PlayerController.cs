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

    private Transform rayCastTf;

    public LayerMask playerLayerMask;

    private void Awake()
    {
        varsContainer = ManagerVars.GetManagerVarsContainer();
        rayCastTf = transform.Find("RayCastGo");
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
        if(GetComponent<Rigidbody2D>().velocity.y < 0f && !IsPlayerJumpCastRay() && GameManager.Instance().isGameOver == false)
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance().isGameOver = true;
            //TODO UI GameOver
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

    private bool IsPlayerJumpCastRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayCastTf.position, Vector2.down, 1f, playerLayerMask);
        if(hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Platform")
            {
                return true;
            }
        }
        return false;
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
