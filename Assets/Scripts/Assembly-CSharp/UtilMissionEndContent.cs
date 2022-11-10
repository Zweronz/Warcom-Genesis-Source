using UnityEngine;

public class UtilMissionEndContent : MonoBehaviour
{
	public TUIMeshText m_bonusCreditsText;

	public TUIMeshText m_bonusExpText;

	public TUIMeshText m_missionCreditsText;

	public TUIMeshText m_missionExpText;

	private void DoubleShow()
	{
		m_bonusCreditsText.UpdateMesh();
		m_bonusExpText.UpdateMesh();
		m_missionCreditsText.UpdateMesh();
		m_missionExpText.UpdateMesh();
	}
}
