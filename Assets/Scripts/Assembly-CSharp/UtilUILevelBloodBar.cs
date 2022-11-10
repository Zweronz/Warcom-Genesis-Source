using UnityEngine;

public class UtilUILevelBloodBar : MonoBehaviour
{
	public TUIRect m_clip;

	public TUIMeshSprite m_middle;

	public TUIMeshSprite m_left;

	public TUIMeshSprite m_right;

	public TUIMeshSprite m_charactorIcon;

	public TUIMeshText m_charactorName;

	public TUITexture m_TUITextureDynamic;

	public GameObject m_onHitCover;

	public bool isWeb;

	private float m_onHitCoverLoopTime;

	private float m_hp;

	private bool m_sfx;

	public void Start()
	{
		string text = (isWeb ? DataCenter.Conf().GetCharacterInfo(DataCenter.StateMulti().GetCurrentCharactor()).name : DataCenter.Conf().GetCharacterInfo(DataCenter.StateSingle().GetCurrentCharactor()).name);
		m_charactorName.text = text;
		m_charactorName.UpdateMesh();
		string frameName = (isWeb ? DataCenter.Conf().GetCharacterInfo(DataCenter.StateMulti().GetCurrentCharactor()).model : DataCenter.Conf().GetCharacterInfo(DataCenter.StateSingle().GetCurrentCharactor()).model);
		m_TUITextureDynamic.LoadFrame(frameName);
		m_charactorIcon.frameName = frameName;
		m_charactorIcon.UpdateMesh();
		m_onHitCoverLoopTime = m_onHitCover.GetComponent<Animation>()["bloodstain"].length;
	}

	public void Update()
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player == null)
		{
			return;
		}
		if (player.m_HPCurrent != m_hp)
		{
			if (m_hp > player.m_HPCurrent)
			{
				m_onHitCover.GetComponent<Animation>().Play("bloodstain");
			}
			m_clip.rect.width = (0f - m_clip.rect.x) * 2f * player.m_HPCurrent / (player.m_HPMax * player.m_factorHpMax);
			m_clip.UpdateRect();
			m_left.gameObject.SetActiveRecursively(m_clip.rect.width > 0f);
			m_right.transform.localPosition = new Vector3(m_clip.rect.width - 35.5f, 0f, -2f);
			m_right.gameObject.SetActiveRecursively(m_clip.rect.width > 0f);
			m_hp = player.m_HPCurrent;
			m_middle.UpdateMesh();
		}
		if (player.m_HPCurrent / (player.m_HPMax * player.m_factorHpMax) < 0.2f)
		{
			if (m_middle.frameName != "BloodBarMid02")
			{
				m_middle.frameName = "BloodBarMid02";
				m_left.frameName = "BloodBarLeft02";
				m_right.frameName = "BloodBarRight02";
				m_middle.UpdateMesh();
				m_left.UpdateMesh();
				m_right.UpdateMesh();
			}
			if (!m_sfx)
			{
				m_sfx = true;
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().PlayAudio("Fx_heart_loop");
			}
			else if (player.m_HPCurrent <= 0f)
			{
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().StopAudio("Fx_heart_loop");
			}
			m_onHitCoverLoopTime -= Time.deltaTime;
			if (m_onHitCoverLoopTime < 0f)
			{
				m_onHitCover.GetComponent<Animation>().Play("bloodstain");
				m_onHitCoverLoopTime = m_onHitCover.GetComponent<Animation>()["bloodstain"].length;
			}
		}
		else
		{
			if (m_middle.frameName != "BloodBarMid")
			{
				m_middle.frameName = "BloodBarMid";
				m_left.frameName = "BloodBarLeft";
				m_right.frameName = "BloodBarRight";
				m_middle.UpdateMesh();
				m_left.UpdateMesh();
				m_right.UpdateMesh();
			}
			if (m_sfx)
			{
				m_sfx = false;
				DataCenter.StateCommon().audio.GetComponent<TAudioController>().StopAudio("Fx_heart_loop");
			}
			m_onHitCoverLoopTime = m_onHitCover.GetComponent<Animation>()["bloodstain"].length;
		}
	}

	public void OnDestroy()
	{
		if (m_sfx)
		{
			m_sfx = false;
			DataCenter.StateCommon().audio.GetComponent<TAudioController>().StopAudio("Fx_heart_loop");
		}
	}
}
