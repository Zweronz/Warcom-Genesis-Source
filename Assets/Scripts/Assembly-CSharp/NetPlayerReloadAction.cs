public class NetPlayerReloadAction
{
	public double timeStamp;

	public int currentAmmoTotal;

	public WeaponType currentWeapon;

	public NetPlayerReloadAction(double timeStamp, int currentTotal, WeaponType currentWeapon)
	{
		this.timeStamp = timeStamp;
		currentAmmoTotal = currentTotal;
		this.currentWeapon = currentWeapon;
	}

	public NetPlayerReloadAction()
	{
	}
}
