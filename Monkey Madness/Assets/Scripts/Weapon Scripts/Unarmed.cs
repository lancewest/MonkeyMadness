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
	
	public int ammo = 0;
	
	
	public void fire()
	{
		//Debug.Log ("Hit!");
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
