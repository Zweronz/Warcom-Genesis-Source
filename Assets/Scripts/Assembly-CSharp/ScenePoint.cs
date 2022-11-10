using UnityEngine;

public class ScenePoint : MonoBehaviour
{
	public void OnDrawGizmos()
	{
		Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.2f);
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.4f);
		Gizmos.DrawSphere(base.transform.position, 0.5f);
	}
}
