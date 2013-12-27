using UnityEngine;
using System.Collections;

public class AK47 : Weapon 
{
	public readonly int damage = 15;
	public readonly int magazineCapacity = 30;
	public readonly float recoilTime = 0.10f;
	public readonly bool showCrosshair = true;
	public readonly int price;
	public readonly float upwardAngleDrift = 0.85f;
	public readonly int range;
	public readonly int damageFadeFactor;

	public WeaponController weaponController;
	public PlayerSoundManager soundManager;
	
	public int ammo = 0;


	public AK47(WeaponController wc, PlayerSoundManager sm)
	{
		weaponController = wc;
		soundManager = sm;
	}


	public void fire()
	{
		if(ammo <= 0)
			return;

		soundManager.playAKFire();
		ammo--;
		weaponController.fire(recoilTime);
		weaponController.recoil(upwardAngleDrift);
	}
	

	public int getDamage()
	{
		return damage;
	}

	public int getMagazineCapacity()
	{
		return magazineCapacity;
	}

	public double getRecoilTime()
	{
		return recoilTime;
	}

	public int getPrice()
	{
		return price;
	}

	public float getUpwardAngleDrift()
	{
		return upwardAngleDrift;
	}

	public int getRange()
	{
		return range;
	}

	public int getDamageFadeFactor()
	{
		return damageFadeFactor;
	}

	public bool getShowCrosshair()
	{
		return showCrosshair;
	}

	
	public int getAmmo()
	{
		return ammo;
	}

	public void changeAmmo(int change)
	{
		ammo += change;
	}

}
