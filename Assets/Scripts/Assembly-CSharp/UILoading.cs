using System;
using UnityEngine;

public class UILoading : MonoBehaviour, TUIHandler
{
	public GameObject m_tutorial;

	public GameObject m_tips;

	public TUIMeshSprite m_bk;

	public GameObject btn;

	private int m_count;

	private TUI m_tui;

	private int m_step;

	public void Awake()
	{
		m_count = 0;
	}

	public void Start()
	{
		m_tui = TUI.Instance("TUI");
		m_tui.SetHandler(this);
		GC.Collect();
		Resources.UnloadUnusedAssets();
		btn.GetComponent<Animation>()["BtnFlash"].wrapMode = WrapMode.Loop;
		if (UtilUIUpdateAttribute.GetNextScene() == "Shop")
		{
			btn.SetActiveRecursively(false);
			m_bk.frameName = "LoadingShop";
			m_bk.UpdateMesh();
			m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
			OpenClikPlugin.Show(false);
			return;
		}
		if (UtilUIUpdateAttribute.GetNextScene().Contains("MissionConfirm") || UtilUIUpdateAttribute.GetNextScene() == "SuitUp")
		{
			btn.SetActiveRecursively(false);
			m_bk.frameName = "LoadingMissionConfirm";
			m_bk.UpdateMesh();
			m_tui.transform.Find("TUIControl").Find("IpadMask").gameObject.SetActiveRecursively(true);
			OpenClikPlugin.Show(false);
			return;
		}
		if (DataCenter.Save().GetWeek() == -2)
		{
			m_bk.frameName = "Tutorial01";
			m_bk.UpdateMesh();
			m_step = 3;
			return;
		}
		if (DataCenter.Save().GetWeek() == -1)
		{
			m_bk.frameName = "Tutorial04";
			m_bk.UpdateMesh();
			m_step = 1;
			return;
		}
		if (DataCenter.Save().GetWeek() == 0)
		{
			m_bk.frameName = "Tutorial05";
			m_bk.UpdateMesh();
			m_step = 1;
			return;
		}
		m_tutorial.SetActiveRecursively(false);
		m_tips.SetActiveRecursively(true);
		TUIMeshSprite component = m_tips.transform.Find("Character").GetComponent<TUIMeshSprite>();
		component.frameName = "Character0" + (UnityEngine.Random.Range(0, 12) % 4 + 1);
		component.UpdateMesh();
		TUIMeshText component2 = m_tips.transform.Find("TipsText").GetComponent<TUIMeshText>();
		int id = UnityEngine.Random.Range(0, DataCenter.Conf().GetTipsCount() * 3) % DataCenter.Conf().GetTipsCount() + 1;
		component2.text = DataCenter.Conf().GetTipsById(id);
		component2.UpdateMesh();
		m_step = 0;
	}

	public void OnDestroy()
	{
		OpenClikPlugin.Hide();
	}

	public void Update()
	{
		TUIInput[] input = TUIInputManager.GetInput();
		for (int i = 0; i < input.Length; i++)
		{
			m_tui.HandleInput(input[i]);
		}
	}

	public void LateUpdate()
	{
		if (UtilUIUpdateAttribute.GetNextScene().Contains("Level") && DataCenter.Save().GetWeek() < 1)
		{
			if (m_step == 0)
			{
				Application.LoadLevel(UtilUIUpdateAttribute.GetNextScene());
				TAudioController component = DataCenter.StateCommon().audio.GetComponent<TAudioController>();
				if (component != null)
				{
					component.StopAudio("music_Meu");
				}
				UnityEngine.Object.Destroy(DataCenter.StateCommon().audio.transform.Find("Audio").gameObject);
			}
			return;
		}
		m_count++;
		if (m_count != 120)
		{
			return;
		}
		Application.LoadLevel(UtilUIUpdateAttribute.GetNextScene());
		if (UtilUIUpdateAttribute.GetNextScene().Contains("Level"))
		{
			TAudioController component2 = DataCenter.StateCommon().audio.GetComponent<TAudioController>();
			if (component2 != null)
			{
				component2.StopAudio("music_Meu");
			}
			UnityEngine.Object.Destroy(DataCenter.StateCommon().audio.transform.Find("Audio").gameObject);
		}
	}

	public void HandleEvent(TUIControl control, int eventType, float wparam, float lparam, object data)
	{
		if (control.name == "Begin" && eventType == 3)
		{
			m_step--;
			if (m_step == 2)
			{
				m_bk.frameName = "Tutorial02";
			}
			else if (m_step == 1)
			{
				m_bk.frameName = "Tutorial03";
			}
			m_bk.UpdateMesh();
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("UI_MeuButton_Click");
		}
	}
}
