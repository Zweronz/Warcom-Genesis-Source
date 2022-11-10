using UnityEngine;

public class UtilMissionEndEffectLight : MonoBehaviour
{
	public Vector3 startPos;

	public Vector3 endPos;

	public float time;

	private void Start()
	{
		time = Time.time;
	}

	private void Update()
	{
		float num = Vector3.Magnitude(base.transform.position - endPos - base.transform.parent.position);
		if (num > 0.1f)
		{
			base.transform.position = Vector3.Lerp(startPos + base.transform.parent.position, endPos + base.transform.parent.position, Time.time - time);
			return;
		}
		base.transform.position = startPos + base.transform.parent.position;
		time = Time.time;
		base.gameObject.SetActiveRecursively(false);
	}
}
