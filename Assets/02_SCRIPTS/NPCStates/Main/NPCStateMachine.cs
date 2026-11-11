using UnityEngine;

public class NPCStateMachine : StateMachine<NPCStateMachine.ENPCState>
{
    public enum ENPCState
    {
       // Spawn,
        Idle,
        Wander,
        Search,
        Wait,
        Talk,
        Listen,
        Question,
    }

    private NPCContext _context;
    private Transform _transform => transform;

    private void Awake()
    {
        _context = new NPCContext(
            transform: _transform
            );

        InitializeStates();
    }

    void InitializeStates()
    {
        //States.Add(ENPCState.Spawn, new NPCSpawnState(_context, ENPCState.Spawn));
        States.Add(ENPCState.Idle, new NPCIdleState(_context, ENPCState.Idle));
        States.Add(ENPCState.Wander, new NPCWanderState(_context, ENPCState.Wander));
        States.Add(ENPCState.Search, new NPCSearchState(_context, ENPCState.Search));
        States.Add(ENPCState.Wait, new NPCWaitState(_context, ENPCState.Wait));
        States.Add(ENPCState.Talk, new NPCTalkState(_context, ENPCState.Talk));
        States.Add(ENPCState.Listen, new NPCListenState(_context, ENPCState.Listen));
        States.Add(ENPCState.Question, new NPCQuestionState(_context, ENPCState.Question));

        CurrentState = States[ENPCState.Idle];
    }
}
