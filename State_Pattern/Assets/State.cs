using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE,
        PATROL,
        PURSUE,
        ATTACK,
        SLEEP,
        SAFE
    };

    public enum EVENT
    {
        ENTER,
        UPDATE,
        EXIT
    };

    public STATE name;
    public EVENT stage;
    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Animator anim;
    protected Transform player;
    protected State nextState;

    private float visDist = 10.0f;
    private float visAngle = 30.0f;
    private float shootDist = 7.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Animator _anim, Transform _player)
    {
        npc = _npc;
        agent = _agent;
        anim = _anim;
        player = _player;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    public bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (direction.magnitude < visDist && angle < visAngle)
            return true;
        return false;
    }

    public bool CanSneakBehind()
    {
        Vector3 direction = (player.position - npc.transform.position) * -1;
        if (direction.magnitude < shootDist)
            return true;
        return false;
    }

    public bool CanAttackPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        if (direction.magnitude < shootDist)
            return true;
        return false;
    }

    public void LookToPlayer(float rotationSpeed)
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);
        direction.y = 0;

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
    }

    public int FindClosest(int currentIndex, List<GameObject> list)
    {
        float lastDist = Mathf.Infinity;
        for (int i = 0; i < list.Count; i++)
        {
            var thisWp = list[i];
            float dist = Vector3.Distance(npc.transform.position, thisWp.transform.position);
            if (dist < lastDist)
            {
                currentIndex = i - 1;
                lastDist = dist;
            }
        }
        return currentIndex;
    }
}
