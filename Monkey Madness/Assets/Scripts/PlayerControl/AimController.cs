using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour 
{
	public float aimAngle = 0.0f;

	public Transform crosshair;
	public Transform weaponArm;
	public Transform head;

	public float rotateSpeed = 90.0f;
	public float fineRotateSpeed = 30.0f;
	public float maxUpAngle = 80.0f;
	public float maxDownAngle = 80.0f;
	public float crosshairDistance = 3.0f;

	void Update () 
	{
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
	}

	public void recoil(float recoildAngle)
	{
		float angle = weaponArm.transform.eulerAngles.z;

		if(angle > 360-maxUpAngle || angle < 180)
			weaponArm.transform.Rotate( new Vector3(0.0f, 0.0f, -recoildAngle));
	}
}
