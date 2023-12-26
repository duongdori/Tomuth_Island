using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCurrent
{
    //animation
    const string IDLE = "_Idle";
    const string WALK_FORWARD = "_Walk_Forward";
    const string ATTACK = "_Attack";
    const string RUN_FORWARD = "_Run_Forward";
    const string HIT_FRONT = "_Hit_Front";
    const string HIT_BACK = "_Hit_Back";
    const string DEATH = "_Death";

    //call animation
    public string GetIdle(string enemy)
    {
        return enemy + IDLE;
    }

    public string GetWalkForward(string enemy)
    {
        return enemy + WALK_FORWARD;
    }

    public string GetRunForward(string enemy)
    {
        return enemy + RUN_FORWARD;
    }

    public string GetAttack(string enemy)
    {
        return enemy + ATTACK;
    }

    public string GetHitFront(string enemy)
    {
        return enemy + HIT_FRONT;
    }

    public string GetHitBack(string enemy)
    {
        return enemy + HIT_BACK;
    }

    public string GetDeath(string enemy)
    {
        return enemy + DEATH;
    }
}
public enum AnimalPositions
{
    None,
    Lead,
    Boss
}
public enum AnimalStatus
{
    Dangerous,
    Peace,
    Attack
}
