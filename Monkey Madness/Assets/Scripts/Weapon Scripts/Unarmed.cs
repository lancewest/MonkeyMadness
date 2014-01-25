using UnityEngine;
using System.Collections;

public class Unarmed : Weapon 
{
	public readonly int damage = 0;
	public readonly int magazineCapacity = 0;
	public readonly double recoilTime = 0.0;
	public readonly bool showCrosshair = false;
	public readonly int price;
	public readonly float upwardAngleDrift;
	public readonly int range;
	public readonly int damageFadeFactor;
	
	public int extraAmmo = 0;
	public int loadedAmmo = 0;

	public bool fireDown = false;

	
	public void fire()
	{
		//Debug.Log ("Hit!");
	}

	public void reload()
	{
		
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
		return "Unarmed";
	}

	public string getFireMode()
	{
		return "Semi";
	}

	public void setFireHeldDown(bool newFireDown)
	{
		fireDown = newFireDown;
	}

	public Vector2 getMagazineLocation()
	{
		return new Vector2(0.0f,0.0f);
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
