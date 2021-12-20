using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    private int currentIndex = -1;
    public Patrol(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
        : base(_npc, _agent, _anim, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }
    public override void Enter()
    {
        currentIndex = FindClosest(currentIndex, GameEnvironment.Singleton.CheckpointsList);
        anim.SetTrigger("isWalking");
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.Singleton.CheckpointsList.Count - 1)
                currentIndex = 0;
            else
                currentIndex++;

            agent.SetDestination(GameEnvironment.Singleton.CheckpointsList[currentIndex].transform.position);
        }

        if (CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }

        if (CanSneakBehind())
        {
            nextState = new Safe(npc, agent, anim, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        anim.ResetTrigger("isWalking");
        base.Exit();
    }
}
