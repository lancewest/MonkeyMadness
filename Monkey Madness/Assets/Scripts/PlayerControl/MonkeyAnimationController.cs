using UnityEngine;
using System.Collections;

public class MonkeyAnimationController : MonoBehaviour 
{
	private Animation myAnimation;
	private PlayerController playerController;

	public float animationSpeed;

	public int animationState = 0; // 0=Idle, 1=Walk, 



	void Start()
	{
		playerController = transform.parent.GetComponent<PlayerController>();
		myAnimation = GetComponent<Animation>();
		myAnimation.Play("Idle", PlayMode.StopAll);
	}

	void Update()
	{
		if(playerController.state == animationState)
			return;

		if(playerController.state == 0)
		{
			animationState = 0;
			myAnimation.CrossFade("Idle", 0.2f);
		}
		else if(playerController.state == 1)
		{
			animationState = 1;
			myAnimation["Walk"].speed = 1.0f;
			myAnimation.CrossFade("Walk", 0.2f);
		}
		else if(playerController.state == 5)
		{
			animationState = 5;
			myAnimation["Walk"].speed = -1.0f;
			myAnimation.CrossFade("Walk", 0.2f);
		}
	}
}
