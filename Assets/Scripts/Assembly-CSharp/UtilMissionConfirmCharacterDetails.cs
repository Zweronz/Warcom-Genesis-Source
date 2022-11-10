using UnityEngine;

public class UtilMissionConfirmCharacterDetails : MonoBehaviour
{
	public Transform m_hp;

	public Transform m_hpRecover;

	public Transform m_speed;

	public GameObject m_leftBtn;

	public GameObject m_rightBtn;

	private float m_HpMax = 2000f;

	private float m_HPRecover = 200f;

	private float m_Speed = 6f;

	public void RefreshCharacterDetails(Player player)
	{
		float num = 1f;
		int num2 = Mathf.Max(DataCenter.Save().GetWeek(), 0);
		num = ((num2 > 3) ? Mathf.Max(2f - (float)(num2 - 1) * 0.33f, 1f) : 2f);
		UpdateBar(m_hp, player.m_HPMax * player.m_factorHpMax / num, m_HpMax);
		UpdateBar(m_hpRecover, player.m_HPRecoverSpeed * player.m_factorHpRecover / num, m_HPRecover);
		UpdateBar(m_speed, player.m_moveSpeed * player.m_factorMoveSpeed, m_Speed);
	}

	private void UpdateBar(Transform barTransform, float a, float b)
	{
		Transform transform = barTransform.Find("Bar");
		Transform transform2 = transform.Find("Clip");
		TUIRect component = transform2.GetComponent<TUIRect>();
		component.rect.width = (0f - component.rect.x) * 2f * a / b;
		component.UpdateRect();
		TUIMeshSprite component2 = transform.Find("BarMiddle").GetComponent<TUIMeshSprite>();
		component2.UpdateMesh();
		TUIMeshSprite component3 = transform.Find("BarLeft").GetComponent<TUIMeshSprite>();
		component3.gameObject.SetActiveRecursively(component.rect.width != 0f);
		TUIMeshSprite component4 = transform.Find("BarRight").GetComponent<TUIMeshSprite>();
		component4.gameObject.SetActiveRecursively(component.rect.width >= 46f);
	}

	public void NextOk()
	{
		if (m_leftBtn != null)
		{
			m_leftBtn.SetActiveRecursively(true);
			m_leftBtn.GetComponent<TUIButtonClick>().Reset();
		}
		if (m_rightBtn != null)
		{
			m_rightBtn.SetActiveRecursively(true);
			m_rightBtn.GetComponent<TUIButtonClick>().Reset();
		}
	}
}
