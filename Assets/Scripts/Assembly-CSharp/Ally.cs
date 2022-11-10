using UnityEngine;

public class Ally : NPC
{
	public override void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		base.Initialize(prefab, name, position, direction);
		m_gameObject.layer = 10;
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
	}
}
