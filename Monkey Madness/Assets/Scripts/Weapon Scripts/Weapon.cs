using UnityEngine;
using System.Collections;

public interface Weapon
{
	void fire();

	int getDamage();
	int getMagazineCapacity();
	double getRecoilTime();
	int getPrice();
	float getUpwardAngleDrift();
	int getRange();
	int getDamageFadeFactor();
	bool getShowCrosshair();

	int getAmmo();
	void changeAmmo(int change);
}
