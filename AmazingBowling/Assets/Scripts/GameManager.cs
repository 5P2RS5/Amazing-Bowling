using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onReset;
    public static GameManager instance;

    public GameObject readyPanel; // 게임 준비 상태 판넬

    public Text scoreText;

    public Text bestScoreText;

    public Text messageText;

    public bool isRoundActive = false; // 게임 중인지 아닌지

    int score = 0;

    public ShooterRotator shooterRotator; // 대기 중일 때 슈팅 관련 스크립트 끄기 위해서

    public CamFollow cam; // 대기 중일 때 카메라 제어

    void Awake()
    {
        instance = this;
        UpdateUI();
    }

    void Start()
    {
        StartCoroutine("RoundRoutine");
    }

    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }

    void UpdateBestScore()
    {
        if (GetBestScore() < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }

    int GetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore");
        return bestScore;
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "Best Score: " + GetBestScore();
    }

    public void OnBallDestroy() // 볼이 폭발했을때
    {
        UpdateUI();
        isRoundActive = false; // 라운드 대기 상태로 넘어가기
    }

    public void Reset()
    {
        score = 0;
        UpdateUI();
        
        // 라운드를 다시 처음부터 시작하는 코드
        StartCoroutine("RoundRoutine");
    }

    IEnumerator RoundRoutine()
    {
        // Ready
        onReset.Invoke();
        readyPanel.SetActive(true);
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Idle);
        shooterRotator.enabled = false;

        isRoundActive = false;

        messageText.text = "Ready...";
        
        yield return new WaitForSeconds(3f);
        // Play
        isRoundActive = true;
        readyPanel.SetActive(false);
        shooterRotator.enabled = true;
        
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Ready);
        while (isRoundActive == true)
        {
            yield return null; // 조건 만족안될 때 까지 무한 루프
        }
        // END
        readyPanel.SetActive(true);
        shooterRotator.enabled = false;

        messageText.text = "Wait For Next Round...";
        
        yield return new WaitForSeconds(3f);
        
        Reset();
    }
}
