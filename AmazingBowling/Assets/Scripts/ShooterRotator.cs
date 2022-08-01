using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

// 포신 회전 관련 스크립트
public class ShooterRotator : MonoBehaviour
{
    enum RotateState
    {
        Idle, // 대기 상태
        Vertical, // 수직 회전
        Horizontal, // 수평 회전
        Ready // 회전 끝 쏠 준비
    }

    RotateState state = RotateState.Idle;

    // 1초에 360도 회전
    public float verticalRotateSpeed = 360f;
    public float horizontalRotateSpeed = 360f;

    public BallShooter ballShooter;
    
    void Update()
    {
        switch (state)
        {
            case RotateState.Idle:
                if (Input.GetButtonDown("Fire1")) // project setting -> InputManager에서 확인이 가능
                    state = RotateState.Horizontal;
                break;
            case RotateState.Horizontal:
                if (Input.GetButton("Fire1"))
                    transform.Rotate(new Vector3(0, horizontalRotateSpeed * Time.deltaTime,0));
                else if(Input.GetButtonUp("Fire1")) 
                    state = RotateState.Vertical;
                break;
            case RotateState.Vertical:
                if (Input.GetButton("Fire1"))
                    transform.Rotate(new Vector3(-1 * verticalRotateSpeed * Time.deltaTime, 0, 0));
                else if (Input.GetButtonUp("Fire1"))
                {
                    state = RotateState.Ready;
                    ballShooter.enabled = true;
                }
                break;
            case RotateState.Ready:
                break;
        }
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.identity; // x = 0, y = 0, z = 0으로 초기화한다.
        state = RotateState.Idle;
        ballShooter.enabled = false;
    }
}
