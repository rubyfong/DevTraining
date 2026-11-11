using UnityEngine;

public class NPCSpawnState : NPCState
{
    //float _timer = 2;
    //bool _toWander, _toSchool;

    public NPCSpawnState(NPCContext context, NPCStateMachine.ENPCState estate) : base(context, estate)
    {
        NPCContext Context = context;
    }

    public override void EnterState()
    {
        Context.Data.CurrentState = "Spawn";
    }

    public override void ExitState()
    {
        //_toWander = false;
        //_toSchool = false;
        //_timer = 2;
    }

    public override void UpdateState()
    {
    }

    public override NPCStateMachine.ENPCState GetNextState()
    {
        //if (_toSchool) return FireflyStateMachine.EFireflyState.School;
        //if (_toWander) return FireflyStateMachine.EFireflyState.Wander;
        return StateKey;
    }
    public override void FixedUpdateState()
    {

    }

    #region On Collision
    public override void OnCollisionEnter(Collision collision)
    {

    }

    public override void OnCollisionExit(Collision collision)
    {

    }

    public override void OnCollisionStay(Collision collision)
    {

    }
    #endregion

    #region On Trigger
    public override void OnTriggerEnter(Collider other)
    {

    }

    public override void OnTriggerExit(Collider other)
    {

    }

    public override void OnTriggerStay(Collider other)
    {

    }
    #endregion 
}

