using UnityEngine;

public class NPCQuestionState : NPCState
{
    public NPCQuestionState(NPCContext context, NPCStateMachine.ENPCState estate) : base(context, estate)
    {
        NPCContext Context = context;
    }
    public override void EnterState()
    {
        Context.Data.GoToQuestionState = false;
        Context.Data.CanBeQuestioned = false;
        Context.Data.CurrentState = "Question";
        UIManager.Instance.SetStatusImage(Context.Data, EStatus.None);
        Context.Agent.isStopped = true;
        Context.Data.CurrentPartner = PlayerController.Instance.gameObject;
        GameManager.Instance.EnableControls = false;

        if (Context.Data == GameManager.Instance.AskNameNPC)
        {
            UIManager.Instance.DisplayAskNameScreen();
        }
        else
        {
            // bool random value
            // one is real name
            // one is fake name

            UIManager.Instance.DisplayQuestionScreen(Context.Data, GameManager.Instance.PlayerName);
        }

        Context.Data.EnableTimer = false;
        Context.Data.CooldownTimer = 1f;
    }

    public override void ExitState()
    {

        if (GameManager.Instance.AskNameNPC == Context.Data)
        {
            GameManager.Instance.AskNameNPC = null;
        }

        if (GameManager.Instance.DialogueNPCs.Contains(Context.Data))
        {
            GameManager.Instance.DialogueNPCs.Remove(Context.Data);
        }
        Context.Agent.isStopped = false;
        Context.Data.CanBeQuestioned = false;
    }

    public override void UpdateState()
    {
        if (Context.Data.CooldownTimer > 0 && Context.Data.EnableTimer)
        {
            Context.Data.CooldownTimer -= Time.deltaTime;
        }
    }

    public override NPCStateMachine.ENPCState GetNextState()
    {
        if (Context.Data.CooldownTimer <= 0)
        {
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
