using UnityEngine;

public class UtilUILevelWeaponsChange : MonoBehaviour
{
	public TUIButtonSelect m_weapon01Btn;

	public TUIButtonSelect m_weapon02Btn;

	public TUIButtonSelect m_weapon03Btn;

	public TUIMeshText m_weaponAmmoText;

	public TUIMeshSprite m_weaponIcon;

	public TUITexture m_TUITextureDynamic;

	public GameObject m_buyBullet;

	public GameObject m_notUseBullet;

	public TUIMeshText m_bulletMoneyText;

	public TUIMeshText m_bulletNumText;

	public TUIRect m_sniperAmmoRect;

	public TUIMeshSprite m_snipeAmmo;

	public GameObject m_notUseSniperBullet;

	public TUIMeshText m_snipeAmmoText;

	public GameObject m_sniperModeBtn;

	public bool isWeb;

	private int m_currentIndex = 1;

	private int m_count;

	private string m_weapon01IconName;

	private string m_weapon02IconName;

	private string m_weapon03IconName;

	private bool m_buyBulletReset;

	private bool m_bulletEmpty;

	public void Awake()
	{
		CharacterEquipInfo characterEquipInfo = (isWeb ? DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateMulti().GetCurrentCharactor()) : DataCenter.Save().GetCharacterEquipInfo(DataCenter.StateSingle().GetCurrentCharactor()));
		m_weapon01IconName = DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon01).icon;
		m_TUITextureDynamic.LoadFrame(m_weapon01IconName);
		m_weapon02IconName = DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon02).icon;
		m_TUITextureDynamic.LoadFrame(m_weapon02IconName);
		m_weapon03IconName = DataCenter.Conf().GetWeaponInfoByName(characterEquipInfo.weapon03).icon;
		m_TUITextureDynamic.LoadFrame(m_weapon03IconName);
	}

	public void Start()
	{
		m_weapon01Btn.SetSelected(true);
		m_weapon02Btn.SetSelected(false);
		m_weapon03Btn.SetSelected(false);
		m_weaponIcon.frameName = m_weapon01IconName;
		m_weaponIcon.UpdateMesh();
	}

	public void Update()
	{
		m_count++;
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (m_count <= 6)
		{
			return;
		}
		if (player != null)
		{
			if (player.m_armorNotUseBullet || player.GetWeapon().GetWeaponType() == WeaponType.Knife)
			{
				m_weaponAmmoText.gameObject.SetActiveRecursively(false);
				m_notUseBullet.gameObject.SetActiveRecursively(true);
			}
			else
			{
				m_weaponAmmoText.gameObject.SetActiveRecursively(true);
				m_notUseBullet.gameObject.SetActiveRecursively(false);
			}
			m_weaponAmmoText.text = player.GetWeapon().GetAmmoCurrentAmount() + "/" + player.GetWeapon().GetAmmoTotal();
			m_weaponAmmoText.UpdateMesh();
			m_bulletMoneyText.text = DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName()).buyAmmoPrice.ToString();
			m_bulletMoneyText.UpdateMesh();
			m_bulletNumText.text = "x" + player.GetWeapon().GetAmmoCapacity() * 2;
			m_bulletNumText.UpdateMesh();
			if (player.GetWeapon().GetWeaponType() == WeaponType.SniperRifle)
			{
				if (player.GetCharacterStateFire().m_time != 0f)
				{
					float num = 50f * player.GetCharacterStateFire().m_time / player.GetCharacterStateFire().m_totalTime;
					m_sniperAmmoRect.transform.localPosition = new Vector3(50f - num, 0f, 0f);
					m_sniperAmmoRect.UpdateRect();
					m_snipeAmmo.UpdateMesh();
				}
				if (player.m_armorNotUseBullet)
				{
					m_notUseSniperBullet.SetActiveRecursively(true);
					m_snipeAmmoText.gameObject.SetActiveRecursively(false);
				}
				else
				{
					m_notUseSniperBullet.SetActiveRecursively(false);
					m_snipeAmmoText.gameObject.SetActiveRecursively(true);
				}
				m_snipeAmmoText.text = player.GetWeapon().GetAmmoCurrentAmount() + "/" + player.GetWeapon().GetAmmoTotal();
				m_snipeAmmoText.UpdateMesh();
			}
			if (player.GetWeapon().GetAmmoTotal() < player.GetWeapon().GetAmmoCapacity() * 2 && player.GetWeapon().GetWeaponType() != 0)
			{
				if (DataCenter.Save().GetMoney() >= DataCenter.Conf().GetWeaponInfoByName(player.GetWeapon().GetName()).buyAmmoPrice)
				{
					if (!player.m_armorNotUseBullet)
					{
						if (!m_buyBulletReset)
						{
							BuyBulletReset(true);
							m_buyBulletReset = true;
							if (player.GetWeapon().GetAmmoTotal() + player.GetWeapon().GetAmmoCurrentAmount() != 0)
							{
								GameObject.FindGameObjectWithTag("TipsPool").GetComponent<UtilUILevelTips>().PushTipsInStack("Ammo low!");
							}
						}
					}
					else
					{
						m_buyBulletReset = false;
						BuyBulletReset(false);
					}
				}
				else
				{
					m_buyBulletReset = false;
					BuyBulletReset(false);
				}
			}
			else
			{
				m_buyBulletReset = false;
				BuyBulletReset(false);
			}
		}
		m_count = 0;
	}

	public void ChangeWeapon()
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		m_currentIndex++;
		if (m_currentIndex == 4)
		{
			m_currentIndex = 1;
		}
		if (player != null)
		{
			player.ChangeWeapon(m_currentIndex);
			GameManager.Instance().GetCamera().SetCurrentCrosshairByWeaponInfo(DataCenter.Conf().GetWeaponInfoByName(player.GetChangeWeapon().GetName()));
		}
		if (m_currentIndex == 1)
		{
			m_weapon01Btn.SetSelected(true);
			m_weapon02Btn.SetSelected(false);
			m_weapon03Btn.SetSelected(false);
			m_weaponIcon.frameName = m_weapon01IconName;
			m_weaponIcon.UpdateMesh();
		}
		else if (m_currentIndex == 2)
		{
			m_weapon01Btn.SetSelected(false);
			m_weapon02Btn.SetSelected(true);
			m_weapon03Btn.SetSelected(false);
			m_weaponIcon.frameName = m_weapon02IconName;
			m_weaponIcon.UpdateMesh();
		}
		else if (m_currentIndex == 3)
		{
			m_weapon01Btn.SetSelected(false);
			m_weapon02Btn.SetSelected(false);
			m_weapon03Btn.SetSelected(true);
			m_weaponIcon.frameName = m_weapon03IconName;
			m_weaponIcon.UpdateMesh();
		}
		if (player.GetChangeWeapon().GetWeaponType() == WeaponType.SniperRifle)
		{
			m_sniperModeBtn.SetActiveRecursively(true);
		}
		else
		{
			m_sniperModeBtn.SetActiveRecursively(false);
		}
	}

	public void ChangeWeapon2()
	{
		Player player = GameManager.Instance().GetLevel().GetPlayer();
		if (player != null)
		{
			m_currentIndex--;
			if (m_currentIndex == 0)
			{
				m_currentIndex = 3;
			}
			player.ChangeWeapon(m_currentIndex);
			GameManager.Instance().GetCamera().SetCurrentCrosshairByWeaponInfo(DataCenter.Conf().GetWeaponInfoByName(player.GetChangeWeapon().GetName()));
		}
		if (m_currentIndex == 1)
		{
			m_weapon01Btn.SetSelected(true);
			m_weapon02Btn.SetSelected(false);
			m_weapon03Btn.SetSelected(false);
			m_weaponIcon.frameName = m_weapon01IconName;
			m_weaponIcon.UpdateMesh();
		}
		else if (m_currentIndex == 2)
		{
			m_weapon01Btn.SetSelected(false);
			m_weapon02Btn.SetSelected(true);
			m_weapon03Btn.SetSelected(false);
			m_weaponIcon.frameName = m_weapon02IconName;
			m_weaponIcon.UpdateMesh();
		}
		else if (m_currentIndex == 3)
		{
			m_weapon01Btn.SetSelected(false);
			m_weapon02Btn.SetSelected(false);
			m_weapon03Btn.SetSelected(true);
			m_weaponIcon.frameName = m_weapon03IconName;
			m_weaponIcon.UpdateMesh();
		}
		if (player.GetChangeWeapon().GetWeaponType() == WeaponType.SniperRifle)
		{
			m_sniperModeBtn.SetActiveRecursively(true);
		}
		else
		{
			m_sniperModeBtn.SetActiveRecursively(false);
		}
	}

	private void BuyBulletReset(bool active)
	{
		m_buyBullet.SetActiveRecursively(active);
		if (active)
		{
			m_buyBullet.transform.Find("BuyBullet").GetComponent<TUIButtonClick>().Reset();
		}
	}
}
