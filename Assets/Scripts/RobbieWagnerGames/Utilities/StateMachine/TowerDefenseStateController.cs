using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RobbieWagnerGames;

public enum TowerDefenseStateEnum
{
    None = 0,
    Prep = 1,
    Battle = 2,
    Resolve = 3
}

public class TowerDefenseStateController : StateController<TowerDefenseStateEnum>
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void TransitionState(TowerDefenseStateEnum stateKey)
    {
        base.TransitionState(stateKey);
    }
}
