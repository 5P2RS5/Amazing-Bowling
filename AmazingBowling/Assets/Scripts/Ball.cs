using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공
public class Ball : MonoBehaviour
{
    public LayerMask whatIsProb;
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;

    public float maxDamage = 100f;
    // 자기 반경으로 1000의 힘을 줘서 반경내에 있으면 튕겨낸다.
    public float explosionForce = 1000f;
    // 포탄 생명주기
    public float lifeTime = 10f;
    public float explosionRadius = 20f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // 10초 뒤 파괴
    }

    void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProb);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            
            // AddExplosionForce() : 어떤 폭발의 원점을 지정해주면 해당 원점으로부터 자신이 얼마만큼 떨어져있는지 계산한 후
            //내가 스스로 얼마만큼 튕겨져나가야 할지 계산해주는 함수
            targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            // 어떤 위치에서 어떤 반경을 가지고 어떤 힘을 가지고 폭발할지 계산해 내가 얼마만큼 떨어져 나갈지 스스로 계산해서 튕겨져 나간다.
            Prop targetProp = colliders[i].GetComponent<Prop>();

            float damage = CalculateDamage(colliders[i].transform.position);
            
            targetProp.TakeDamage(damage);
        }
        
        // 다른 오브젝트에 공이 부딪히면 파괴된다. 파티클과 오디오도 바로 파괴되어 재생이 되지 않을 수 있으므로
        // 부모로부터 빠져나오도록 설정한다.
        explosionParticle.transform.parent = null; // 부모로부터 빠져나오기

        explosionParticle.Play();
        explosionAudio.Play();
        
        GameManager.instance.OnBallDestroy();
        
        Destroy(explosionParticle.gameObject, explosionParticle.main.duration);
        Destroy(gameObject);
    }

    // 폭발지점으로부터 멀리 떨어질수록 데미지를 차등 지급하는 함수
    float CalculateDamage(Vector3 targetPosition) // targetPosition : 상대방 위치
    {
        Vector3 explosionToTarget = targetPosition - transform.position;

        float distance = explosionToTarget.magnitude; 
        // magnitude : 실제 거리를 리턴하는 함수, 상대지점과 자신사이의 거리를 피타고라스 법칙으로 계산하여 리턴

        float edgeToCenterDistance = explosionRadius - distance; // 가장자리로 부터 거리

        float percentage = edgeToCenterDistance / explosionRadius; // 얼마만큼 들어갔는지 퍼센트

        float damage = maxDamage * percentage;

        damage = MathF.Max(0, damage);
        // Max 두 값중 큰값을 리턴한다. 만약 damage가 걸쳐 있는 경우 +가 될 수 있는데 이렇게되면 체력이 오히려 회복을한다.
        // 이를 방지하기 위해 사용
        return damage;

    }
}
