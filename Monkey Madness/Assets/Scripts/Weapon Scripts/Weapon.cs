using UnityEngine;
using System.Collections;

public interface Weapon
{
	void fire();
	void reload();

	int getDamage();
	int getMagazineCapacity();
	double getRecoilTime();
	int getPrice();
	float getUpwardAngleDrift();
	int getRange();
	int getDamageFadeFactor();
	bool getShowCrosshair();
	string getWeaponName();
	string getFireMode();
	void setFireHeldDown(bool newFireDown);
	Vector2 getMagazineLocation();
	float getReloadTime();
	bool canReload();
	bool hasMagazine();

	int getExtraAmmo();
	int getLoadedAmmo();
	void changeExtraAmmo(int change);
}
