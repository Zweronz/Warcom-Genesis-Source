using UnityEngine;

public class SpecialEffectAnimationContinuous : MonoBehaviour
{
	public void Awake()
	{
		base.gameObject.GetComponent<Animation>().playAutomatically = false;
		base.gameObject.GetComponent<Animation>().clip.wrapMode = WrapMode.Loop;
		base.gameObject.SetActiveRecursively(false);
	}

	public void Play()
	{
		base.gameObject.SetActiveRecursively(true);
		base.gameObject.GetComponent<Animation>().Play();
	}

	public void Stop()
	{
		base.gameObject.SetActiveRecursively(false);
		base.gameObject.GetComponent<Animation>().Stop();
	}
}
