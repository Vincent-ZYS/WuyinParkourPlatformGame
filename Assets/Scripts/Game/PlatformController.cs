﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public SpriteRenderer[] PlatformSpriteRenderer;
    private float curFallTimer;
    private float fallTime;
    private bool isReadyToFall = false;

    public void SinglePlatformInitation(Sprite sprite, float fallTime)
    {
        this.fallTime = fallTime;
        isReadyToFall = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        for (int i=0; i < PlatformSpriteRenderer.Length; i++)
        {
            PlatformSpriteRenderer[i].sprite = sprite;
        }
    }

    private void OnEnable()
    {
        EventCenter.AddListner(EventType.PlatformReadyToFall, ReadyToFall);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener(EventType.PlatformReadyToFall, ReadyToFall);
    }

    private void Update()
    {
        if (fallTime > 0f && isReadyToFall)
        {
            curFallTimer += Time.deltaTime;
            if(curFallTimer >= fallTime)
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                StartCoroutine(DealyHide());
            }
        }
    }

    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public void ReadyToFall()
    {
        isReadyToFall = true;
    }
}
