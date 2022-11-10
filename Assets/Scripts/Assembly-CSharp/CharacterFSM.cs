public class CharacterFSM
{
	public class State
	{
		private State m_nextState;

		private bool m_continueUpdate = true;

		private State m_childState;

		public void Enter()
		{
			m_childState = null;
			OnEnter();
		}

		public void Exit()
		{
			if (m_childState != null)
			{
				m_childState.Exit();
				m_childState = null;
			}
			OnExit();
		}

		public void Update(float deltaTime)
		{
			m_nextState = this;
			m_continueUpdate = true;
			if (m_childState != null)
			{
				m_childState.Update(deltaTime);
				State state = m_childState.NextState();
				m_continueUpdate = m_childState.ContinueUpdate();
				if (state != m_childState)
				{
					m_childState.Exit();
					m_childState = state;
					if (m_childState != null)
					{
						m_childState.Enter();
					}
				}
			}
			if (m_continueUpdate)
			{
				OnUpdate(deltaTime);
			}
		}

		public bool ContinueUpdate()
		{
			return m_continueUpdate;
		}

		public State NextState()
		{
			return m_nextState;
		}

		protected void ContinueUpdate(bool continueUpdate)
		{
			m_continueUpdate = continueUpdate;
		}

		protected void NextState(State nextState)
		{
			m_nextState = nextState;
		}

		protected void Push(State state)
		{
			if (m_childState != null)
			{
				m_childState.Exit();
				m_childState = null;
			}
			m_childState = state;
			if (m_childState != null)
			{
				m_childState.Enter();
			}
		}

		protected void Pop()
		{
			if (m_childState != null)
			{
				m_childState.Exit();
				m_childState = null;
			}
		}

		protected virtual void OnEnter()
		{
		}

		protected virtual void OnExit()
		{
		}

		protected virtual void OnUpdate(float deltaTime)
		{
		}
	}

	private State m_state;

	protected void UpdateFSM(float deltaTime)
	{
		if (m_state == null)
		{
			return;
		}
		m_state.Update(deltaTime);
		State state = m_state.NextState();
		if (state != m_state)
		{
			m_state.Exit();
			m_state = state;
			if (m_state != null)
			{
				m_state.Enter();
			}
		}
	}

	protected void SwitchFSM(State state)
	{
		if (m_state != null)
		{
			m_state.Exit();
			m_state = null;
		}
		m_state = state;
		if (m_state != null)
		{
			m_state.Enter();
		}
	}
}
