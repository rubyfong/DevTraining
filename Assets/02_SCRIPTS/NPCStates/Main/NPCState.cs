public abstract class NPCState : BaseState<NPCStateMachine.ENPCState>
{
    protected NPCContext Context;

    public NPCState(NPCContext context, NPCStateMachine.ENPCState stateKey) : base(stateKey)
    {
        Context = context;
    }
}