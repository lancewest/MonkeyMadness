using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour 
{
	public WeaponController weaponController;

	public float aimAngle = 0.0f;

	public Transform crosshair;
	public Transform weaponArm;
	public Transform head;

	public float rotateSpeed = 90.0f;
	public float fineRotateSpeed = 30.0f;
	public float maxUpAngle = 80.0f;
	public float maxDownAngle = 80.0f;
	public float crosshairDistance = 3.0f;

	public bool levelingOutGun = false;
	public bool resumeingAimAngle = false;
	public float aimAngleBeforeReload = 0.0f;

	void Update () 
	{
		if(!transform.parent.networkView.isMine && (Network.isClient || Network.isServer))
			return;

		if(levelingOutGun)
			levelOutGunForReload();
		else if(resumeingAimAngle)
			resumeAimAngleAfterReload();
		else
			aim();
	}


	void aim()
	{
		float angle = weaponArm.transform.eulerAngles.z;

		if( (angle > maxDownAngle && angle < 180 && Input.GetAxis("Vertical") <= 0) || (angle < 360-maxUpAngle && angle > 180 && Input.GetAxis("Vertical") >=0))
			return;

		if(Input.GetAxis("Shift") != 0)
			weaponArm.transform.Rotate( new Vector3(0.0f, 0.0f, -Input.GetAxis("Vertical") * fineRotateSpeed * Time.deltaTime));
		else
			weaponArm.transform.Rotate( new Vector3(0.0f, 0.0f, -Input.GetAxis("Vertical") * rotateSpeed * Time.deltaTime));

		aimAngle = weaponArm.transform.rotation.eulerAngles.z;
	}

	public void recoil(float recoildAngle)
	{
		float angle = weaponArm.transform.eulerAngles.z;

		if(angle > 360-maxUpAngle || angle < 180)
			weaponArm.transform.Rotate( new Vector3(0.0f, 0.0f, -recoildAngle));
	}

	public void resetRotation()
	{
		weaponArm.transform.rotation = Quaternion.identity;
	}

	public void resetSize()
	{
		weaponArm.transform.localScale.Set(1.0f,1.0f,1.0f);
	}


	public void startLevelingGun()
	{
		aimAngleBeforeReload = aimAngle;
		levelingOutGun = true;
	}

	public void levelOutGunForReload()
	{
		float directionModifier = -1.0f;
		if(aimAngleBeforeReload > 200)
			directionModifier = 1.0f;

		weaponArm.transform.Rotate( new Vector3(0.0f, 0.0f, directionModifier * 2.5f * rotateSpeed * Time.deltaTime));

		if(Mathf.Abs(weaponArm.transform.rotation.eulerAngles.z) < 10)
		{
			levelingOutGun = false;
			weaponController.beginReloadAnimation();
		}
	}

	public void resumeAimAngleAfterReload()
	{
		float directionModifier = 1.0f;
		if(aimAngleBeforeReload > 200)
			directionModifier = -1.0f;

		weaponArm.transform.Rotate( new Vector3(0.0f, 0.0f, directionModifier * 2.5f * rotateSpeed * Time.deltaTime));

		if(Mathf.Abs(weaponArm.transform.eulerAngles.z - aimAngleBeforeReload) < 10)
		{
			resumeingAimAngle = false;
			weaponController.doneReloading();
		}
	}
}
