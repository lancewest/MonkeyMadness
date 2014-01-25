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
	
	public int extraAmmo = 0;
	public int loadedAmmo = 0;

	public bool fireHeldDown = false;
	public bool playNoAmmoSoundAtEndOfMagazine = true;

	public Vector2 magazineLocation = new Vector2(-4.2723f, -2.5983f);


	public AK47(WeaponController wc, PlayerSoundManager sm)
	{
		weaponController = wc;
		soundManager = sm;

		extraAmmo = 1000;
		loadedAmmo = 30;
	}


	public void fire()
	{
		if(loadedAmmo <= 0)
		{
			if(!fireHeldDown || playNoAmmoSoundAtEndOfMagazine)
			{
				soundManager.playNoAmmo();
				playNoAmmoSoundAtEndOfMagazine = false;
			}
			return;
		}

		soundManager.playAKFire();
		loadedAmmo--;
		playNoAmmoSoundAtEndOfMagazine = true;
		weaponController.fire(recoilTime);
		weaponController.recoil(upwardAngleDrift);
	}

	public void reload()
	{
		if(loadedAmmo >= magazineCapacity || extraAmmo <= 0)
			return;
		else if(extraAmmo + loadedAmmo <= magazineCapacity)
		{
			loadedAmmo += extraAmmo;
			extraAmmo = 0;
		}
		else
		{
			extraAmmo -= magazineCapacity - loadedAmmo;
			loadedAmmo = magazineCapacity;
		}
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

	public string getWeaponName()
	{
		return "AK47";
	}

	public string getFireMode()
	{
		return "Auto";
	}

	public void setFireHeldDown(bool newFireHeldDown)
	{
		fireHeldDown = newFireHeldDown;
	}

	public Vector2 getMagazineLocation()
	{
		return magazineLocation;
	}

	public float getReloadTime()
	{
		return 1.0f;
	}

	public bool canReload()
	{
		return !(loadedAmmo >= magazineCapacity || extraAmmo <= 0);
	}

	public bool hasMagazine()
	{
		return true;
	}

	
	public int getExtraAmmo()
	{
		return extraAmmo;
	}

	public int getLoadedAmmo()
	{
		return loadedAmmo;
	}

	public void changeExtraAmmo(int change)
	{
		extraAmmo += change;
	}

}
