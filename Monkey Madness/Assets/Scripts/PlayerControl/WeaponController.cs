using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour 
{
	public Weapon[] weapons = new Weapon[2];
	public Sprite[] weaponSprites = new Sprite[2];
	public Sprite[] magazineSprites = new Sprite[2];
	public Sprite[] reloadArmSprites = new Sprite[2];
	public GameObject[] emptyMagazines = new GameObject[2];
	public Weapon selectedWeapon;

	public MonkeyAnimationController animationController;
	public Animation animationComponent;
	public PlayerSoundManager soundManager;
	public AimController aimController;
	
	public SpriteRenderer selectedWeaponSprite;
	public SpriteRenderer reloadArmSpriteRenderer;
	public SpriteRenderer crosshair;
	public Transform magazine;

	public int selectedWeaponIndex = 0;
	public bool fireing = false;

	public bool reloading = false;

	void Start() 
	{
		weapons [0] = new Unarmed ();
		weapons [1] = new AK47 (this, soundManager);
		selectedWeapon = weapons[0];
		reloadArmSpriteRenderer.sprite = reloadArmSprites[0];
		selectedWeaponSprite.sprite = weaponSprites[0];
		crosshair.enabled = weapons[0].getShowCrosshair() && (networkView.isMine || !(Network.isServer || Network.isClient));

		animationController.prepLayersForReload();
	}

	void Update() 
	{
		if(!transform.parent.networkView.isMine && (Network.isClient || Network.isServer))
			return;

		if(Input.GetAxis("Fire") != 0 && !fireing && !reloading && !animationComponent.animation.IsPlaying("PutAwayWeapon"))
		{
			selectedWeapon.fire();
			selectedWeapon.setFireHeldDown(true);
		}

		if(Input.GetAxis("Weapon0") != 0 && !reloading)
			switchWeapon(0);
		else if(Input.GetAxis("Weapon1") != 0 && weapons[1].getExtraAmmo() > 0 && !reloading)
			switchWeapon(1);

		if(Input.GetAxis("Fire") == 0 && !reloading)
			selectedWeapon.setFireHeldDown(false);


		if(Input.GetAxis("Reload") != 0)
			reload();
	}


	private void switchWeapon(int weaponIndex)
	{
		if(animationComponent.animation.IsPlaying("PutAwayWeapon") || (weapons[weaponIndex].getExtraAmmo() <= 0 && weaponIndex != 0) || selectedWeapon == weapons[weaponIndex])
			return;

		if( (weaponIndex != 0 && selectedWeapon.getWeaponName() == "Unarmed") || (weaponIndex == 0 && selectedWeapon.getWeaponName() != "Unarmed") )
			aimController.resetRotation();

		if (Network.isServer || Network.isClient)
			networkView.RPC ("rpc_setSelectedWeaponIndex", RPCMode.All, weaponIndex);
		else
			selectedWeaponIndex = weaponIndex;


		if (Network.isServer || Network.isClient)
			networkView.RPC ("rpc_PlayPutAwayWeaponAnimation", RPCMode.All);
		else
			animationComponent.Blend("PutAwayWeapon");
	}
	
	public void changeWeaponSprite()
	{
		selectedWeapon = weapons[selectedWeaponIndex];
		selectedWeaponSprite.sprite = weaponSprites[selectedWeaponIndex];
		magazine.GetComponent<SpriteRenderer>().sprite = magazineSprites[selectedWeaponIndex];
		magazine.GetComponent<SpriteRenderer>().enabled = selectedWeapon.hasMagazine();
		magazine.localPosition = selectedWeapon.getMagazineLocation();
		crosshair.enabled = selectedWeapon.getShowCrosshair() && (networkView.isMine || !(Network.isServer || Network.isClient));
	}


	private void reload()
	{
		if(!selectedWeapon.canReload() || reloading)
			return;

		reloading = true;
		crosshair.enabled = false;
		aimController.startLevelingGun();
	}

	public void beginReloadAnimation()
	{
		if(selectedWeapon.hasMagazine())
		{
			if(Network.isServer || Network.isClient)
				networkView.RPC("rpc_dropEmptyMagazine", RPCMode.All);
			else
				rpc_dropEmptyMagazine();
		}
		
		if(selectedWeapon.getWeaponName() == "AK47")
		{
			if(Network.isServer || Network.isClient)
				networkView.RPC("rpc_PlayAKReloadAnimation", RPCMode.All);
			else
				animationComponent.Blend("AKReload");
		}
	}

	public void doneReloadAnimation()
	{
		aimController.resumeingAimAngle = true;

	}

	public void doneReloading()
	{
		reloading = false;
		crosshair.enabled = true && (networkView.isMine || !(Network.isServer || Network.isClient));
	}

	public void letGoOfMagazine()
	{
		reloadArmSpriteRenderer.sprite = reloadArmSprites[0];
		selectedWeapon.reload();
	}

	public void grabAKMagazine()
	{
		reloadArmSpriteRenderer.sprite = reloadArmSprites[selectedWeaponIndex];
	}



	public void fire(float recoilTime)
	{
		fireing = true;

		StartCoroutine(WaitThenBeDoneFireing(recoilTime));
	}

	IEnumerator WaitThenBeDoneFireing(float time)
	{
		yield return new WaitForSeconds(time);
		
		fireing = false;
	}

	public void recoil(float recoilAngle)
	{
		aimController.recoil(recoilAngle);
	}



	[RPC]
	void rpc_PlayAKReloadAnimation()
	{
		animationComponent.Blend("AKReload");
	}

	[RPC]
	void rpc_PlayPutAwayWeaponAnimation()
	{
		animationComponent.Blend("PutAwayWeapon");
	}

	[RPC]
	void rpc_setSelectedWeaponIndex(int newIndex)
	{
		selectedWeaponIndex = newIndex;
	}

	[RPC]
	void rpc_dropEmptyMagazine()
	{
		GameObject newMagazine = (GameObject.Instantiate(emptyMagazines[selectedWeaponIndex], magazine.position, new Quaternion(magazine.rotation.x,magazine.rotation.y,magazine.rotation.z * this.transform.localScale.x,magazine.rotation.w)) as GameObject);
		newMagazine.transform.localScale = new Vector3(newMagazine.transform.localScale.x * this.transform.localScale.x,newMagazine.transform.localScale.y,newMagazine.transform.localScale.z);
	}
}
