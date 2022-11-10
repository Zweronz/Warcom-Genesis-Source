using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UtilMedalPrompt : MonoBehaviour
{
	public TUITexture m_tuiTextureDynamic;

	public bool isGame;

	public bool isWeb;

	public TUIMeshSprite m_icon;

	public TUIMeshText m_name;

	public TUIMeshText m_details;

	private bool m_show;

	private Queue<MedalInfo> m_unlockMedalQueue = new Queue<MedalInfo>();

	private int m_count;

	private void Update()
	{
		m_count++;
		if (m_count > 4)
		{
			m_count = 0;
			CheckMedal();
			if (m_unlockMedalQueue.Count != 0 && !m_show)
			{
				m_show = true;
				ShowMedalPrompt();
			}
		}
	}

	private void CheckMedal()
	{
		for (int i = 1; i <= DataCenter.Conf().GetMedalCount(); i++)
		{
			MedalInfo medalInfoByIndex = DataCenter.Conf().GetMedalInfoByIndex(i);
			if (isGame)
			{
				if (isWeb)
				{
					continue;
				}
				if (medalInfoByIndex.conditionName == "TimeCount")
				{
					if ((float)DataCenter.Save().GetTotalTime() + DataCenter.StateSingle().m_missionTempData.time >= (float)medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "KillCount")
				{
					if (DataCenter.Save().GetEnemyKill() + DataCenter.StateSingle().m_missionTempData.emenyKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "HeadshotCount")
				{
					if (DataCenter.Save().GetEnemyHeadShot() + DataCenter.StateSingle().m_missionTempData.emenyKillHeadShot >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "DeathCount")
				{
					if (DataCenter.Save().GetDead() + DataCenter.StateSingle().m_missionTempData.dead >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "BeHeadshotCount")
				{
					if (DataCenter.Save().GetDeadHeadShot() + DataCenter.StateSingle().m_missionTempData.deadHeadShot >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "UseItemCount")
				{
					if (DataCenter.Save().GetItemUseCount() + DataCenter.StateSingle().m_missionTempData.itemUseCount >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "BulletCount")
				{
					if (DataCenter.Save().GetFireBullet() + DataCenter.StateSingle().m_missionTempData.fireBullet >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "MeleeKillCount")
				{
					if (DataCenter.Save().GetWeaponKnifeKill() + DataCenter.StateSingle().m_missionTempData.weaponKnifeKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "PistolKillCount")
				{
					if (DataCenter.Save().GetWeaponHandgunKill() + DataCenter.StateSingle().m_missionTempData.weaponHandgunKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "RifleKillCount")
				{
					if (DataCenter.Save().GetWeaponAssaultRifleKill() + DataCenter.StateSingle().m_missionTempData.weaponAssaultRifleKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "RocketKillCount")
				{
					if (DataCenter.Save().GetWeaponRPGKill() + DataCenter.StateSingle().m_missionTempData.weaponRPGKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "ShotgunKillCount")
				{
					if (DataCenter.Save().GetWeaponShotgunGunKill() + DataCenter.StateSingle().m_missionTempData.weaponShotgunKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "SMGKillCount")
				{
					if (DataCenter.Save().GetWeaponSubmachineGunKill() + DataCenter.StateSingle().m_missionTempData.weaponSubmachineGunKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "SniperKillCount")
				{
					if (DataCenter.Save().GetWeaponSniperKill() + DataCenter.StateSingle().m_missionTempData.weaponSniperKill >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "KillScoutCount")
				{
					if (DataCenter.Save().GetKillScoutCount() + DataCenter.StateSingle().m_missionTempData.killScoutCount >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "KillCommandoCount")
				{
					if (DataCenter.Save().GetKillCommandoCount() + DataCenter.StateSingle().m_missionTempData.killCommandoCount >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "KillSniperCount")
				{
					if (DataCenter.Save().GetKillSniperCount() + DataCenter.StateSingle().m_missionTempData.killSniperCount >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
					{
						DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
						m_unlockMedalQueue.Enqueue(medalInfoByIndex);
					}
				}
				else if (medalInfoByIndex.conditionName == "KillRaiderCount" && DataCenter.Save().GetKillRaiderCount() + DataCenter.StateSingle().m_missionTempData.killRaiderCount >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i) && !DataCenter.StateSingle().m_missionTempData.medalList.Contains(i))
				{
					DataCenter.StateSingle().m_missionTempData.medalList.Add(i);
					m_unlockMedalQueue.Enqueue(medalInfoByIndex);
				}
			}
			else if (medalInfoByIndex.conditionName == "BattlesCount")
			{
				if (DataCenter.Save().GetPlayTimes() >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i))
				{
					DataCenter.Save().UnlockMedal(i, medalInfoByIndex.conditionName);
					m_unlockMedalQueue.Enqueue(medalInfoByIndex);
				}
			}
			else if (medalInfoByIndex.conditionName == "TaskCount")
			{
				if (DataCenter.Save().GetMainMissionComplete() + DataCenter.Save().GetAuxiliaryMissionComplete() >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i))
				{
					DataCenter.Save().UnlockMedal(i, medalInfoByIndex.conditionName);
					m_unlockMedalQueue.Enqueue(medalInfoByIndex);
				}
			}
			else if (medalInfoByIndex.conditionName == "BuyWeaponCount")
			{
				if (DataCenter.Save().GetUnlockWeaponCount() - 10 >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i))
				{
					DataCenter.Save().UnlockMedal(i, medalInfoByIndex.conditionName);
					m_unlockMedalQueue.Enqueue(medalInfoByIndex);
				}
			}
			else if (medalInfoByIndex.conditionName == "BuyArmorCount")
			{
				if (DataCenter.Save().GetUnlockArmorCount() >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i))
				{
					DataCenter.Save().UnlockMedal(i, medalInfoByIndex.conditionName);
					m_unlockMedalQueue.Enqueue(medalInfoByIndex);
				}
			}
			else if (medalInfoByIndex.conditionName == "BuyItemCount" && DataCenter.Save().GetUnlockItemCount() >= medalInfoByIndex.conditionParam && !DataCenter.Save().IsMedalUnlock(i))
			{
				DataCenter.Save().UnlockMedal(i, medalInfoByIndex.conditionName);
				m_unlockMedalQueue.Enqueue(medalInfoByIndex);
			}
		}
	}

	private void ShowMedalPrompt()
	{
		MedalInfo medalInfo = m_unlockMedalQueue.Dequeue();
		base.gameObject.GetComponent<Animation>().Play("MedalPromptIn");
		m_icon.frameName = medalInfo.iconName;
		m_tuiTextureDynamic.LoadFrame(m_icon.frameName);
		m_icon.UpdateMesh();
		m_name.text = medalInfo.name;
		m_name.UpdateMesh();
		Regex regex = new Regex("^.*?\\n\\n(.*)$");
		m_details.text = regex.Match(medalInfo.desc).Groups[1].Value;
		m_details.UpdateMesh();
		if (medalInfo.rewardExp != -1)
		{
			if (!isGame)
			{
				DataCenter.Save().SetExp(DataCenter.Save().GetExp() + medalInfo.rewardExp);
			}
			else if (!isWeb)
			{
				DataCenter.StateSingle().m_missionTempData.expMedal += medalInfo.rewardExp;
			}
		}
		else if (medalInfo.rewardMoney != -1)
		{
			if (!isGame)
			{
				DataCenter.Save().SetMoney(DataCenter.Save().GetMoney() + medalInfo.rewardMoney);
			}
			else if (!isWeb)
			{
				DataCenter.StateSingle().m_missionTempData.moneyMedal += medalInfo.rewardMoney;
			}
		}
	}

	private void SetShowFalse()
	{
		m_show = false;
		if (!isGame)
		{
			DataCenter.Save().Save();
		}
	}
}
