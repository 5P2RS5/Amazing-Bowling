using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    // 1. 라운드 대기 상태 카메라
    // 2. 포탄 발사 전 대기 카메라
    // 3. 포탄을 추적하는 카메라

    public enum State
    {
        Idle, Ready, Tracking
    }

    State state // 프로퍼티
    {
        set // 바깥에서 이 state을 equal로 받을 때 set안에 코드가 실행된다.
        {
            switch (value) 
            {
                case State.Idle :
                    targetZoomSize = roundReadyZoomSize;
                    break;
                
                case State.Ready :
                    targetZoomSize = readyShotZoomSize;
                    break;
                
                case State.Tracking :
                    targetZoomSize = trackingZoomSize;
                    break;
            }
        }
    }

    Transform target;
    
    public float smoothTime = 0.2f; // 지연시간, 내가 원하는 위치로 카메라가 이동하기까지 걸리는 시간.

    Vector3 lastMovingVelocity; // 마지막 순간에 원하는 위치까지 어느 속도로 움직였는지
    Vector3 targetPosition; // 목표 위치, target으로 부터 가져온다.

    public Camera cam; // 카메라, 줌인 줌아웃을 지원하기 위해서
    float targetZoomSize = 5f;

    const float roundReadyZoomSize = 14.5f; // 라운드 대기 
    const float readyShotZoomSize = 5f; // 쏘기 전
    const float trackingZoomSize = 10f; // 쏜 후 추적

    float lastZoomSpeed; // 

    void Awake()
    {
        state = State.Idle; // State.Idle을 state프로퍼티의 value로 전달한다.
                            // value는 프로퍼티의 외부로부터 들어오는 값을 나타낸다.
                            // 시스템이 알아서 지정해주는 것
                            // 프로퍼티는 간결히 사용하기 위해 함수 대신해서 사용할 수 있다.
        cam = GetComponentInChildren<Camera>();
    }

    void Move() // 카메라가 타겟의 위치에 맞춰 움직이는 함수
    {
        targetPosition = target.transform.position;

        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, target.position, 
            ref lastMovingVelocity, smoothTime);
        // SmoothDamp()
        transform.position = smoothPosition;
    }

    void Zoom()
    {
        float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomSize, 
            ref lastZoomSpeed, smoothTime);

        cam.orthographicSize = smoothZoomSize;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Move();
            Zoom();
        }
    }

    public void Reset()
    {
        state = State.Idle;
    }

    public void SetTarget(Transform newTarget, State newState)
    {
        target = newTarget;
        state = newState;
    }
}
