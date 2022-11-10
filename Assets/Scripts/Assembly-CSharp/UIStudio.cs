using UnityEngine;

public class UIStudio : MonoBehaviour
{
	private bool sceneChange;

	private float time;

	private bool changeSence;

	public void Start()
	{
		XAdManagerWrapper.SetVideoFile("WCGE_OP.mp4");
		XAdManagerWrapper.ShowVideoAdLocal();
	}

	public void Update()
	{
		time += Time.deltaTime;
		if (time > 0.5f && !changeSence)
		{
			changeSence = true;
			SwitchScene("Entry");
			UtilUIUpdateAttribute.SetNextScene("Entry");
		}
	}

	private void SwitchScene(string scene)
	{
		Application.LoadLevel("Entry");
	}
}
