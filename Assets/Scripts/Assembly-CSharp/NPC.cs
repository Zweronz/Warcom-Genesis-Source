using UnityEngine;

public class NPC : Character
{
	public class AI
	{
		public virtual void Load(NPC npc)
		{
		}

		public virtual void Unload(NPC npc)
		{
		}

		public virtual void Action(NPC npc, float deltaTime)
		{
		}
	}

	public UnityEngine.AI.NavMeshAgent m_agent;

	public float m_hitStopFireInterveTime;

	protected AI m_ai;

	public override void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		base.Initialize(prefab, name, position, direction);
		CharacterController component = m_gameObject.GetComponent<CharacterController>();
		Vector3 center = component.center;
		float radius = component.radius;
		Object.Destroy(component);
		CapsuleCollider capsuleCollider = m_gameObject.AddComponent<CapsuleCollider>();
		capsuleCollider.radius = radius;
		capsuleCollider.center = center;
		m_agent = m_gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (m_ai != null && !m_dead && GameManager.Instance().GetCamera().m_cameraMode == GameCamera.CameraMode.NormalMode)
		{
			m_ai.Action(this, deltaTime);
		}
	}

	public void SetAI(AI ai)
	{
		if (m_ai != null)
		{
			m_ai.Unload(this);
			m_ai = null;
		}
		m_ai = ai;
		if (m_ai != null)
		{
			m_ai.Load(this);
		}
	}

	public override void OnDeath()
	{
		base.OnDeath();
		m_ai = null;
	}

	public override void OnEffect(ref Effect effect)
	{
	}
}
