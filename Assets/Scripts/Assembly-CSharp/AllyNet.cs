using UnityEngine;

public class AllyNet : Character
{
	public override void Initialize(GameObject prefab, string name, Vector3 position, float direction)
	{
		base.Initialize(prefab, name, position, direction);
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
	}

	public override void OnDeath()
	{
		base.OnDeath();
	}

	public override void OnEffect(ref Effect effect)
	{
	}
}
