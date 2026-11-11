using UnityEngine;

public class NPCIdleState : NPCState
{
    public NPCIdleState(NPCContext context, NPCStateMachine.ENPCState estate) : base(context, estate)
    {
        NPCContext Context = context;
    }

    public override void EnterState()
    {
        Context.Data.GoToWanderState = false;
        Context.Data.CurrentState = "Idle";
        Context.Agent.isStopped = true;
        Context.Data.CooldownTimer = Random.Range(1, 2);
    }

    public override void ExitState()
    {
        Context.Agent.isStopped = false;
    }

    public override void UpdateState()
    {
        if (Context.Data.CooldownTimer > 0)
        {
            Context.Data.CooldownTimer -= Time.deltaTime;
        }     
    }

    public override NPCStateMachine.ENPCState GetNextState()
    {
        if (Context.Data.GoToQuestionState) return NPCStateMachine.ENPCState.Question;
        if (Context.Data.GoToWaitState) return NPCStateMachine.ENPCState.Wait;
        if (Context.Data.CooldownTimer <= 0) return NPCStateMachine.ENPCState.Wander;
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

