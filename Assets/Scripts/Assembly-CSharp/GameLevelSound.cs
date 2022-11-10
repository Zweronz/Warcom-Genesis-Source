using UnityEngine;

public class GameLevelSound : MonoBehaviour
{
	public ITAudioEvent bgm;

	public ITAudioEvent msc;

	public void Start()
	{
		bgm.Trigger();
		msc.Trigger();
	}
}
