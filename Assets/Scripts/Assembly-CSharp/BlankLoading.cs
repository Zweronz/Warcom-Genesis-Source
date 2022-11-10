using System;
using UnityEngine;

public class BlankLoading : MonoBehaviour
{
	public static string nextScene;

	private int count;

	private void Start()
	{
		count = 0;
		GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	private void Update()
	{
		count++;
		if (count == 20)
		{
			Application.LoadLevel(nextScene);
		}
	}
}
