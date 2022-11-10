using UnityEngine;

public class UtilDropItem : MonoBehaviour
{
	private float m_triggerRadius = 2f;

	private float m_lifeTime = 0.7f;

	public void Update()
	{
		DropItem dropItem = DropItemStub.GetDropItem(base.transform.gameObject);
		Effect effect = dropItem.GetItemEffect();
		Collider[] array = Physics.OverlapSphere(base.transform.position, m_triggerRadius, 512);
		if (array.Length != 0 && !dropItem.m_isEffect)
		{
			Character character = CharacterStub.GetCharacter(array[0].gameObject);
			character.OnEffect(ref effect);
			dropItem.m_isEffect = true;
		}
		if (dropItem.m_isEffect)
		{
			m_lifeTime -= Time.deltaTime;
			if (m_lifeTime < 0f)
			{
				Object.Destroy(base.transform.gameObject);
			}
		}
	}

	public void SetTriggerRadius(float triggerRadius)
	{
		m_triggerRadius = triggerRadius;
	}
}
