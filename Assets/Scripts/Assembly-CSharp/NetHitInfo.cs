public class NetHitInfo
{
	public WeaponType weaponType;

	public float damage;

	public bool headShot;

	public int netId;

	public int byNetId;

	public NetHitInfo(WeaponType weaponType, float damage, bool headShot, int netId, int byNetId)
	{
		this.weaponType = weaponType;
		this.damage = damage;
		this.headShot = headShot;
		this.netId = netId;
		this.byNetId = byNetId;
	}

	public NetHitInfo()
	{
	}
}
