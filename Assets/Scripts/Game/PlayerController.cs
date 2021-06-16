using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    private Transform rayCastDwTf;
    private Transform rayCastFwTf;

    public LayerMask platformLayerMask;

    public LayerMask spikeLayerMask;

    private void Awake()
    {
        varsContainer = ManagerVars.GetManagerVarsContainer();
        rayCastDwTf = transform.Find("RayCastDwGo");
        rayCastFwTf = transform.Find("RayCastFwGo");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance().isGameStart || 
            GameManager.Instance().isGameOver || 
            GameManager.Instance().isGamePause || EventSystem.current.IsPointerOverGameObject()) { return; }
        ClickLeftRightScreenToMove();
    }

    private void ClickLeftRightScreenToMove()
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
        PlayerGameOverOrNot();
    }

    private void PlayerGameOverOrNot()
    {
        if(GetComponent<Rigidbody2D>().velocity.y < 0f && GameManager.Instance().isGameOver == false)
        {
            if (!IsPlayerJumpCastDwRay())
            {
                GetComponent<SpriteRenderer>().sortingLayerName = "Default";
                GetComponent<BoxCollider2D>().enabled = false;
                GameManager.Instance().isGameOver = true;
                Debug.Log("GameOver");
            }
            else if(IsPlayerJumpCastFwRay())
            {
                GameObject deadEffect = Instantiate(varsContainer.playerDeadEffectGo);
                deadEffect.transform.position = transform.position;
                GameManager.Instance().isGameOver = true;
                Debug.Log("GameOver");
                Destroy(gameObject);
            }
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

    private bool IsPlayerJumpCastDwRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayCastDwTf.position, Vector2.down, 1f, platformLayerMask);
        if(hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                return true;
            }
        }
        return false;
    }

    private bool IsPlayerJumpCastFwRay()
    {
        float isLeftOrNot = isMoveLeft ? -0.3f : 0.3f;
        RaycastHit2D hit = Physics2D.Raycast(rayCastFwTf.position, Vector2.right, isLeftOrNot, spikeLayerMask);
        Debug.DrawRay(rayCastFwTf.position, isLeftOrNot * Vector2.right, Color.red);
        if(hit.collider != null)
        {
            if(hit.collider.tag == "Obstacle")
            {
                Debug.LogError("TOUCH!!");
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
