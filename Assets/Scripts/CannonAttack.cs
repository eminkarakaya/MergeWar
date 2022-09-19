using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonAttack : RangeAttack
{
    public ParticleSystem fireParticle;
    Sequence sequence;
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        if(GameManager.instance.gameStage == GameStage.inGame)
        {
            if(rangeClass.enemiesInRange.Count > 0)
            {
                target = rangeClass.enemiesInRange[0];
                attackRate-= Time.deltaTime;
                if(attackRate < 0)
                {
                    StartCoroutine(AttackAnimation());
                    attackRate = attackRateTemp;
                    attack = true;
                }
                transform.DOLookAt(target.transform.position,.5f);
                unit.state = State.attack;
                
                if(TryGetComponent(out Movement movement))
                    movement.agent.speed = 0;
            }
            else
            {
                unit.state = State.walk;
                target = null;
            }   
        }
    }
    IEnumerator AttackAnimation()
    {
        sequence = DOTween.Sequence();
        yield return new WaitForSeconds(attackRate);
        Fire();
        if(fireParticle != null)
            fireParticle.Play();

        // var oldPos = transform.position;
        // Vector3 qwe = new Vector3(transform.position.x,transform.position.y,transform.position.z-2);
        // transform.DOLocalMove(qwe,1);//.OnComplete(()=>transform.DOLocalMove(oldPos,1));
        // sequence.Append(transform.DOLocalMoveZ(-2f,1));
        // Debug.Log("localz");
        // sequence.Append(transform.DOLocalMoveZ(0,1));
    }
}
