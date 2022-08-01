using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public CamFollow cam;
    
    public Rigidbody ball; // 프리펩으로 가져올때 바로 리지드 바디 적용
    public Transform firePos; // 공이 생성되는 위치
    public Slider powerSlider;
    public AudioSource shootingAudio; // 오디오플레이어

    public AudioClip fireClip; // 카세트 테이프, 발사
    public AudioClip chargingClip; // 카세트 테이프, 게이지 충전
    public float minForce = 15f; // 처음 힘이자 최소힘
    public float maxForce = 30f; // 최대 힘

    public float chargingTime = 0.75f; // 0.75초 만에 minForce에서 maxForce까지 간다.

    float currentForce; // 현재 힘, minForce와 maxForce 사이 값
    float chargeSpeed; // 누르고 있는 동안 1초에 얼마의 힘이 채워질지

    bool fired; // 발사했는지 안했는지 체크

    void OnEnable() // 오브젝트 혹은 컴포넌트가 꺼져있다가 켜질때마다 실행된다. start와 비슷하지만, 켜지고 꺼질때마다 매번 실행된다.
    {
        // 초기화 작업
        currentForce = minForce;
        powerSlider.value = minForce;
        fired = false; // 발사 안함
    }

    void Start()
    {
        chargeSpeed = (maxForce - minForce) / chargingTime;  // 속도 = 거리 / 시간, 1초에 힘이 얼마만큼 채워져야하는지 나타낸다.
    }

    void Update()
    {
        if (fired)
        {
            return;
        }
        powerSlider.value = minForce;
        
        if (currentForce >= maxForce && !fired) // 현재 힘이 최대힘을 초과하는 경우 + 발사 아직 안함
        {
            currentForce = maxForce;
            // 발사처리
            Fire();
        }
        else if (Input.GetButtonDown("Fire1")) // 충전을 이제 막 시작하려고 눌렀을 때
        {
            fired = false; // 연사가 가능
            currentForce = minForce;
            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if (Input.GetButton("Fire1") && !fired) // 충전중, 발사 아직 안함.
        {
            currentForce += chargeSpeed * Time.deltaTime; // 시간 간격으로 힘이 충전될 수 있게 한다.
            powerSlider.value = currentForce; // 슬라이더의 값 동시에 적용하기
        }
        else if (Input.GetButtonUp("Fire1") && !fired) // 마우스 뗌, 발사
        {
            // 발사처리
            Fire();
        }
    }

    void Fire()
    {
        fired = true; // 발사했으니까

        Rigidbody ballInstance = Instantiate(ball, firePos.position, firePos.rotation);

        ballInstance.velocity = currentForce * firePos.forward; // 힘 * firePos의 앞방향으로 속도와 방향 지정

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentForce = minForce; // 발사했으니까
        
        cam.SetTarget(ballInstance.transform, CamFollow.State.Tracking);
    }
}
