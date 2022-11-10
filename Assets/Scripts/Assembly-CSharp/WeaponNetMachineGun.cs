using UnityEngine;

public class WeaponNetMachineGun : WeaponNet
{
	public override void Initialize(GameObject prefab, WeaponInfo weaponInfo)
	{
		base.Initialize(prefab, weaponInfo);
	}

	protected override void OnStartFire()
	{
	}

	protected override void OnStopFire()
	{
	}

	protected override void OnEmptyFire()
	{
	}

	protected override void OnDoFire()
	{
	}
}
