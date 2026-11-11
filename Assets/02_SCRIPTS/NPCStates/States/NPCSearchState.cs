using UnityEngine;

public class NPCSearchState : NPCState
{
    public NPCSearchState(NPCContext context, NPCStateMachine.ENPCState estate) : base(context, estate)
    {
        NPCContext Context = context;
    }

    public override void EnterState()
    {
        Context.Data.CurrentState = "Search";
        Context.Data.CooldownTimer = Random.Range(4, 6);

        // try to find nearest NPC
        GameObject nearest = Context.Data.FindNearestNPC(Context.Data, Context.Data.SearchRange);

        if (nearest == null)
        {
            Debug.Log($"{Context.Transform.name} has nobody to talk to!");
            Context.Data.GoToWanderState = true;
            return;
        }

        Context.Target = nearest.transform;
        Debug.Log(Context.Data.gameObject.name + " targeted " + Context.Target.name);

        if (Context.Target.TryGetComponent(out NPCData data))
        {
            if (data.CurrentPartner == null)
            {
                data.GoToWaitState = true;
                data.CurrentPartner = Context.Data.gameObject;

                Context.Data.CurrentPartner = data.gameObject;               
                Context.Agent.SetDestination(Context.SetTargetPosition());
            }
            else Context.Data.GoToWanderState = true;
        }
        else
        {
            Context.Data.GoToWanderState = true;
        }
    }

    public override void ExitState()
    {
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
        if (Context.Data.GoToQuestionState)
        {
            if (Context.Data.CurrentPartner != null)
            {
                Context.Data.CurrentPartner.GetComponent<NPCData>().GoToWanderState = true;
            }
            Context.ResetPartner();
            return NPCStateMachine.ENPCState.Question;
        }

        if (Context.Data.GoToWaitState) return NPCStateMachine.ENPCState.Wait;

        if (Context.Data.CanBeQuestioned || Context.Data.GoToWanderState || (Context.Data.CooldownTimer <= 0 && !Context.CheckReachDestination()))
        {
            if (Context.Data.CurrentPartner != null)
            {
                Context.Data.CurrentPartner.GetComponent<NPCData>().GoToWanderState = true;
            }
            Context.ResetPartner();
            return NPCStateMachine.ENPCState.Wander;
        }

        if (Context.CheckReachDestination())
        {
            Context.Data.CurrentPartner.GetComponent<NPCData>().GoToListenState = true;
            return NPCStateMachine.ENPCState.Talk;
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


