using UnityEngine;

public class UtilHalo : MonoBehaviour
{
	public TUIButtonClick btn;

	public GameObject followObj;

	private void Update()
	{
		if (btn.pressed)
		{
			followObj.SetActiveRecursively(true);
		}
		else
		{
			followObj.SetActiveRecursively(false);
		}
	}
}
