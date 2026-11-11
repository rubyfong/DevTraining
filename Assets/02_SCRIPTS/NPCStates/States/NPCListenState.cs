using UnityEngine;

public class NPCListenState : NPCState
{
    public NPCListenState(NPCContext context, NPCStateMachine.ENPCState estate) : base(context, estate)
    {
        NPCContext Context = context;
    }
    public override void EnterState()
    {
        Context.Data.GoToListenState = false;
        Context.Data.CurrentState = "Listen";
        Context.Agent.isStopped = true;
        UIManager.Instance.SetStatusImage(Context.Data, EStatus.Speech);
    }

    public override void ExitState()
    {
        Context.Agent.isStopped = false;
        UIManager.Instance.SetStatusImage(Context.Data, EStatus.None);
    }

    public override void UpdateState()
    {

    }

    public override NPCStateMachine.ENPCState GetNextState()
    {
        if (Context.Data.GoToQuestionState)
        {
            if (Context.Data.CurrentPartner != null)
            {
                Context.Data.CurrentPartner.GetComponent<NPCData>().GoToWanderState = true;
            }
            Context.ResetPartner();
            return NPCStateMachine.ENPCState.Question;
        }

        if (Context.Data.CanBeQuestioned || Context.Data.GoToWanderState)
        {
            if (Context.Data.CurrentPartner != null)
            {
                Context.Data.CurrentPartner.GetComponent<NPCData>().GoToWanderState = true;
            }
            Context.ResetPartner();
            return NPCStateMachine.ENPCState.Wander;
        }
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
