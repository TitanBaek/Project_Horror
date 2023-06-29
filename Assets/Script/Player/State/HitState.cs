using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class HitState : StateBase<Player>
    {
        private Coroutine hit_Coroutine;
        private bool hit_Finished;

        public HitState(Player owner) : base(owner) { }

        public override void Enter()
        {
            hit_Finished = false;

            hit_Coroutine = owner.StartCoroutine(Hit());
        }

        public override void Exit()
        {
            owner.StopCoroutine(hit_Coroutine);
        }

        public override void LateUpdate()
        {
        }

        public override void Setup()
        {
        }

        public override void Transition()
        {
            if(hit_Finished)
            {
                owner.ChangeState(P_State.Chase);
            }
        }

        public override void Update()
        {
        }

        public IEnumerator Hit() // 히트 상태에서 실행되게
        {
            int index = Random.Range(0, owner.HitPoints.Length);
            PlayHitSound();
            Debug.Log("사운드가 재생되었다...");
            yield return new WaitForSeconds(0.3f);
            SwitchParticle(index);
            yield return new WaitForSeconds(0.3f);
            owner.Anim.SetBool("Hit", true);
            CameraShake.Instance.ShakeCamera(3f, .5f);
            yield return new WaitForSeconds(0.15f);
            SwitchParticle(index);
            owner.Anim.SetBool("Hit", false);
            Debug.Log("셋불해주세요");
            owner.StartMoving();
            yield return new WaitForSeconds(0.3f);
            Debug.Log("피니시드 트루로 해주세요");
            hit_Finished = true;
            //owner.TakeHit(15);
        }

        public void PlayHitSound() // 히트 상태에서 실행되게
        {
            owner.AudioSources[2].clip = owner.HitAudios[Random.Range(0, owner.HitAudios.Length)];
            if (!owner.AudioSources[2].isPlaying)
            {
                owner.AudioSources[2].Play();
            }
        }
        public void SwitchParticle(int index) // 히트 상태에서 실행되게
        {
            if (owner.HitPoints[index].activeSelf)
            {
                owner.HitPoints[index].SetActive(false);
            }
            else
            {
                owner.HitPoints[index].SetActive(true);

            }
        }
    }
}