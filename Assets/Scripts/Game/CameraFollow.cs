using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 offest;
    private Vector2 velocity;

    // Update is called once per frame
    void Update()
    {
        InitiateTargetPos();
    }

    private void FixedUpdate()
    {
        CameraMovement();
    }

    private void InitiateTargetPos()
    {
        if(target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            offest = target.position - transform.position;
        }
    }

    private void CameraMovement()
    {
        if (target != null)
        {
            float movePosX = Mathf.SmoothDamp(transform.position.x,
                target.position.x - offest.x, ref velocity.x, 0.05f);
            float movePosY = Mathf.SmoothDamp(transform.position.y,
                target.position.y - offest.y, ref velocity.y, 0.05f);

            //when target player's y position over the current camera position then implement
            if (movePosY > transform.position.y)
            {
                transform.position = new Vector3(movePosX, movePosY, transform.position.z);
            }
        }
    }
}
