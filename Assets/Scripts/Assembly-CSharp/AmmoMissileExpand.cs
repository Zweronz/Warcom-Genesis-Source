using UnityEngine;

public class AmmoMissileExpand : MonoBehaviour
{
	public void OnTriggerEnter(Collider collider)
	{
		base.transform.parent.GetComponent<AmmoMissile>().OnTriggerEnter(collider);
	}
}
