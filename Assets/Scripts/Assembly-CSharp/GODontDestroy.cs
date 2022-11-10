using UnityEngine;

public class GODontDestroy : MonoBehaviour
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.F1))
		{
			Cursor.visible = !Cursor.visible;
			Screen.lockCursor = !Screen.lockCursor;
		}
	}
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
