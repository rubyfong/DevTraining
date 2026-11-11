using UnityEngine;

public class NPCTalkState : NPCState
{
    public NPCTalkState(NPCContext context, NPCStateMachine.ENPCState estate) : base(context, estate)
    {
        NPCContext Context = context;
    }

    public override void EnterState()
    {
        Context.Data.CurrentState = "Talk";
        Context.Data.CooldownTimer = Random.Range(2, 4);
        Context.Agent.isStopped = true;
        UIManager.Instance.SetStatusImage(Context.Data, EStatus.Speech);

        // if no rumor, less likely to spread empty thoughts
        bool spreadrumor;
        if (Context.Data.RumorName == null)
        {
            spreadrumor = Random.value < 0.3f;
        }
        else
        {
            spreadrumor = Random.value < 0.9f;
        }

        if (Context.Data.CurrentPartner.TryGetComponent(out NPCData data))
        {
            if(spreadrumor)
            {
                data.RumorName = Context.Data.RumorName;

                if(Context.Data.RumorName == string.Empty || Context.Data.RumorName == null)
                {
                    UIManager.Instance.InstantiateDebugText(
                       $"{Context.Transform.name} distracted {data.transform.name} from their thoughts!");
                }
                else
                {
                    UIManager.Instance.InstantiateDebugText(
                        $"{Context.Transform.name} told {data.transform.name} about {Context.Data.RumorName}!");
                }                
            }
            else
            {
                UIManager.Instance.InstantiateDebugText(
                   $"{Context.Transform.name} and {data.transform.name} are chitchatting.");
            }
        }
        else
        {
            Debug.LogWarning("Cannot find NPCdata for current partner!");
        }   
        
        if(Context.Data.LiedTo > 4)
        {
            data.LiedTo += 2;
        }
        else if (Context.Data.LiedTo > 2)
        {
            data.LiedTo++;
        }
        else if (Context.Data.Affinity > 8)
        {
            data.LiedTo--;
            data.Affinity += 2;
        }
        else if (Context.Data.Affinity > 4)
        {
            data.Affinity++;
        }
    }

    public override void ExitState()
    {
        Context.Agent.isStopped = false;
        UIManager.Instance.SetStatusImage(Context.Data, EStatus.None);
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

        if (Context.Data.CanBeQuestioned || Context.Data.GoToWanderState || Context.Data.CooldownTimer <= 0)
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


