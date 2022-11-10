using UnityEngine;

public class FireLineGroup : MonoBehaviour
{
	public int fireLineIndex;

	public void SetFireLine(Vector3 position, Vector3 dir, float lifeTime)
	{
		Transform transform = base.transform.Find("FireLine" + string.Format("{0:D2}", fireLineIndex));
		transform.position = position;
		transform.right = -dir;
		transform.GetComponent<FireLine>().m_lifeTime = lifeTime;
		transform.gameObject.SetActiveRecursively(true);
		fireLineIndex++;
		if (fireLineIndex > 9)
		{
			fireLineIndex = 0;
		}
	}
}
