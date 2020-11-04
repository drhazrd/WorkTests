using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public abstract class BaseAction<T>: IState
{
	protected T m_owner;
	public void SetOwner(T m_owner) => m_owner = _owner;
	
	public abstract void DoAction(float _dt);
	
	public virtual void OnStateEnter(){}
	public virtual void OnStateExit(){}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public abstract class BaseDescision<T>: IState
{
	protected T m_owner;
	public void SetOwner(T m_owner) => m_owner = _owner;
	public abstract bool Decision();
	public virtual void OnStateEnter(){}
	public virtual void OnStateExit(){}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public abstract class BaseState<T>: IState where T : StateMachine<T>
{
	public void InitState(T _stateMachine)
	{
		SetActions();
		SetTransitions();
		SetOwner(_stateMachine);
	}
	
	public abstract void SetActions();
	public abstract void SetTransitions();
	
	protected T m_stateMachine;
	
	protected BaseAction<T>[] m_actions;
	protected Transition<T>[] m_transitions;
	
	private void SetOwner(T _stateMachine)
	{
		m_stateMachine = _stateMachine;
		
		foreach(BaseAction<T> _action in m_actions){
			_action.SetOwner(_stateMachine);
		}
		foreach(Transition<T> _transition in m_transitions){
			_transition.SetOwner(_stateMachine);
		}
	}
	
	public virtual void OnStateEnter()
	{
		foreach(BaseAction<T> _action in m_actions){
			_action.OnStateEnter();
		}
		foreach(Transition<T> _transition in m_transitions){
			_transition.OnStateEnter();
		}
	}
	
	public virtual void OnStateUpdate(float _dt)
	{
		int _state = -1;
		
		foreach(BaseAction<T> _action in m_actions){
			_action.DoAction(_dt);
		}
		foreach(Transition<T> _transition in m_transitions){
			_state = _transition.DoTransition();
		}
		if(_state != m_stateMachine.CurrentStateIndex){
			m_stateMachine.SetNewState(_state);
		}
	}
	
	public virtual void OnStateExit()
	{
		foreach(BaseAction<T> _action in m_actions){
			_action.OnStateExit();
		}
		foreach(Transition<T> _transition in m_transitions){
			_transition.OnStateExit();
		}
	}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class Transition<T> : IState where T : MonoBehaviour
{
	private string m_name;
	
	protected int m_successState;
	protected int m_failState;
	protected BaseDescision<T> m_decision;
	
	public Transition (string _name, BaseDescision<T> _decision, int _successState, int _failState)
	{
		m_name = _name;
		m_decision = _decision;
		m_successState = _successState;
		m_failState = _failState;
	}
	public void SetOwner(T _owner) => m_decision.SetOwner(_owner);
	public int DoTransition() => m_decision.Decision() ? m_successState : m_failState;
	
	public virtual void OnStateEnter() => m_decision.OnStateEnter();
	public virtual void OnStateExit() => m_decision.OnStateExit();
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class NPCDecisionInRange: BaseDescision<NPCStateMachine>
{
	private float m_range;
	public NPCDecisionInRange(float _range) => m_range = _range;
	
	public override bool Decision()
	{
		float _distance = Vector3.Distance(m_owner.transform.position, m_owner.target.position);
		return _decision < m_range;
	}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class NPCStateIdle: BaseState<NPCStateMachine>
{
	public override void SetActions()
	{
		m_actions = new BaseAction<NPCStateMachine>[]
		{
			new NPCActionIdle(),
			new NPCActionRotate(),
		};
	}
	
	public override void SetTransitions()
	{
		m_transitions = new Transition<NPCStateMachine>[]
		{
			new Transition<NPCStateMachine>("ToChase", new NPCDecisionInRange(3f), (int)NPCStateMachine.NPCState.Chase, (int)NPCStateMachine.NPCState.Idle)
		};
	}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public class NPCStateMachine: StateMachine <NPCStateMachine>
{
	public enum NPCState
	{
		Idle,
		Chase,
		Grow,
	}
	
	public NPCState currentState;
	
	#region Data
	 public Transform target;
	
	#endregion
	
	private void OnEnable() => OnStateChanged += _OnStateChanged;
	private void OnDisable() => OnStateChanged += _OnStateChanged;
	
	public override void FillStateMachineStates()
	{
		AllStates = new Dictionary<int, BaseState<NPCStateMachine>>()
		{
			{ (int)NPCState.Idle, new NPCStateIdle() },
			{ (int)NPCState.Chase, new NPCStateChase() },
			{ (int)NPCState.Grow, new NPCStateGrow() },
		};
	}
	private void _OnStateChanged(int _stateIndex) => currentState = (NPCState) _stateIndex;
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
{
	protected event Action<int> OnStateChanged = delegate {};
	
	private int m_currentStateIndex;
	public int CurrentStateIndex => m_currentStateIndex;
	
	private BaseState<T> m_currentState;
	private Dictionary<int, BaseState<T>> m_allStates = new Dictionary<int, BaseState<T>>();
	public Dictionary<int, BaseState<T>> AllStates
	{
		get => m_allState;
		set
		{
			m_allStates = value;
			InitializeStateMachine();
		}
	}
	public virtual void Awake () => FillStateMachineStates();
	public abstract void FillStateMachineStates();
	
	public void InitializeStateMachine(BaseState<T> _currentState = null)
	{
		m_currentStateIndex = _currentState == null ? 0 : AllStates.First(_state => _state.Value == _currentState)
		m_currentState = _currentState ?? AllStates.Values.First();
		
		foreach(KeyValuePair<int, BaseState<T>> keyValuePair in AllStates)
		{ keyValuePairV.Value.InitState(this as T);}
		
		m_currentState OnStateEnter();
	}
	public virtual void Update() => m_currentState.OnStateUpdate(Time.deltaTime);
	
	public void SetNewState(int _newState)
	{
		m_currentState = _newState;
		
		m_currentState.OnStateExit();
		m_currentState = AllStates[_newState];
		m_currentState.OnStateEnter();
		
		OnStateChanged(_newState);
	}
}

/*-------------------------------------------------------------------------------Seperate Script--------------------------------------------------------------------------------*/
Draft

{
	State[] allStates
	{
		Idle,
		Chase,
		Grow,
		Other,
		Search
	}
	State
	{
		Action[] allActions
		{
			DoNothing
		}
		Action
		{
			void DoAction()
			{
				transform.Translate(Vector3.forward);
			}
		}
		Transition[] allTransitions
		{
			ToChase
		}
		Transition
		{
			Decision decision
			{
				bool DoDecision()
				{
					return PlayerInRange ? true : false;
				}
			}
			State successState;
			State failState;
		}
	}
	
}
