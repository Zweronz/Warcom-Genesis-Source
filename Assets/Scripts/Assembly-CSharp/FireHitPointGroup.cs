using UnityEngine;

public class FireHitPointGroup : MonoBehaviour
{
	public int fireHitPointIndex;

	public void SetFireHitPoint(Vector3 position, float lifeTime)
	{
		Transform transform = base.transform.Find("FireHitPoint" + string.Format("{0:D2}", fireHitPointIndex));
		transform.position = position;
		transform.GetComponent<FireHitPoint>().m_lifeTime = lifeTime;
		transform.gameObject.SetActiveRecursively(true);
		transform.Find("hit").GetComponent<SpecialEffectParticleEmit>().Emit();
		fireHitPointIndex++;
		if (fireHitPointIndex > 9)
		{
			fireHitPointIndex = 0;
		}
	}
}
