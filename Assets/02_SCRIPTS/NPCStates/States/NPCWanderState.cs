using UnityEngine;

public class NPCWanderState : NPCState
{
    float _maxWanderRangeFromSpawn = 10;

    public NPCWanderState(NPCContext context, NPCStateMachine.ENPCState estate) : base(context, estate)
    {
        NPCContext Context = context;
    }

    public override void EnterState()
    {
        Context.Data.GoToWanderState = false;
        Context.Data.CurrentState = "Wander";
        Context.Data.CooldownTimer = Random.Range(1, 3);

        if (Context.Agent.isOnNavMesh)
        {
            float distancefromSpawn = Vector3.Distance(Context.Transform.position, Context.Data.StartingPosition);

            if(distancefromSpawn > _maxWanderRangeFromSpawn)
            {
                Context.Agent.SetDestination(Context.Data.StartingPosition);
            }
            else
            {
                Context.Agent.SetDestination(Context.Data.GenerateRandomPosition());
            }
        }
        else
        {
            Context.Agent.transform.position = Context.Data.StartingPosition;
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

            if (Context.CheckReachDestination())
            {
                Context.Agent.SetDestination(Context.Data.GenerateRandomPosition());
            }
        }    
    }

    public override NPCStateMachine.ENPCState GetNextState()
    {
        if (Context.Data.GoToQuestionState) return NPCStateMachine.ENPCState.Question;
        if (Context.Data.GoToWaitState) return NPCStateMachine.ENPCState.Wait;
        if (Context.Data.CooldownTimer <= 0)
        {
            bool random = Random.value < 0.3f;

            if (random)
            {
                return NPCStateMachine.ENPCState.Idle;
            }
            else
            {
                return NPCStateMachine.ENPCState.Search;
            }          
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


