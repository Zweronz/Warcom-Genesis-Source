using UnityEngine;

public class GameMain : MonoBehaviour
{
	public void Awake()
	{
		GameManager.Instance().StartGame(base.gameObject);
	}

	public void OnDestroy()
	{
		GameManager.Instance().StopGame();
	}

	public void Update()
	{
		GameManager.Instance().UpdateGame();
	}
}
