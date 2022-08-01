using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 파괴할 물건 스크립트, 파괴한 만큼 점수 추가
public class Prop : MonoBehaviour
{
    public int score = 5;
    public ParticleSystem explosionParticle; // 프리펩으로 실시간으로 파티클 찍어내기

    public float hp = 10f;

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            ParticleSystem instance = Instantiate(explosionParticle, transform.position, transform.rotation);

            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();
            
            GameManager.instance.AddScore(score);
            
            Destroy(instance.gameObject, instance.main.duration);
            // Prop을 삭제하고 다시 생성하면 과부화가 올 수 있으므로 껏다가 다시 켜는 식으로 구현
            gameObject.SetActive(false);
        }
    }
}
