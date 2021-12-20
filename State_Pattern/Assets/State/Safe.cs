using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.AI;

public class Safe : State
{
    private int currentIndex = -1;
    public Safe(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.SAFE;
        agent.speed = 5;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        FindClosest(currentIndex, GameEnvironment.Singleton.SafepointsList);
        agent.SetDestination(GameEnvironment.Singleton.SafepointsList[currentIndex + 1].transform.position);
        anim.SetTrigger("isRunning");
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            nextState = new Idle(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        //if (CanSeePlayer())
        //{
        //    nextState = new Pursue(npc, agent, anim, player);
        //    stage = EVENT.EXIT;
        //}

        //if (CanSneakBehind())
        //{
        //    nextState = new Safe(npc, agent, anim, player);
        //    stage = EVENT.EXIT;
        //}
    }

    public override void Exit()
    {
        anim.ResetTrigger("isRunning");
        base.Exit();
    }
}
