using UnityEngine;

public class GameManager
{
	private static GameManager m_instance;

	private GameCamera m_camera;

	private UILevel m_ui;

	private UILevelWeb m_uiWeb;

	private GameLevel m_level;

	public static GameManager Instance()
	{
		if (m_instance == null)
		{
			m_instance = new GameManager();
		}
		return m_instance;
	}

	public void StartGame(GameObject main)
	{
		m_camera = main.GetComponent<GameCamera>();
		m_ui = main.GetComponent<UILevel>();
		m_uiWeb = main.GetComponent<UILevelWeb>();
		m_level = new GameLevel();
		m_level.StartGame();
	}

	public void StopGame()
	{
		m_level.StopGame();
		m_level = null;
		m_ui = null;
		m_uiWeb = null;
		m_camera = null;
	}

	public void UpdateGame()
	{
		m_level.UpdateGame(Time.deltaTime);
	}

	public GameCamera GetCamera()
	{
		return m_camera;
	}

	public UILevel GetUI()
	{
		return m_ui;
	}

	public UILevelWeb GetUIWeb()
	{
		return m_uiWeb;
	}

	public GameLevel GetLevel()
	{
		return m_level;
	}
}
