using UnityEngine;

public class UtilDropItemNet : MonoBehaviour
{
	private float m_triggerRadius = 2f;

	private bool m_isSendLock;

	private float m_lifeTime = 0.7f;

	public void Update()
	{
		DropItemNet dropItemNet = (DropItemNet)DropItemStub.GetDropItem(base.transform.gameObject);
		Effect itemEffect = dropItemNet.GetItemEffect();
		Collider[] array = Physics.OverlapSphere(base.transform.position, m_triggerRadius, 512);
		if (array.Length != 0 && !m_isSendLock)
		{
			UtilsNet.SendLock(dropItemNet.m_netId);
			m_isSendLock = true;
		}
		if (dropItemNet.m_isEffect)
		{
			m_lifeTime -= Time.deltaTime;
			if (m_lifeTime < 0f)
			{
				ScenePointManager.GetScenePointTransform(dropItemNet.m_point).GetComponent<ScenePointDropItemRefresh>().m_isOccupied = false;
				Object.Destroy(base.transform.gameObject);
			}
		}
	}

	public void SetTriggerRadius(float triggerRadius)
	{
		m_triggerRadius = triggerRadius;
	}
}
