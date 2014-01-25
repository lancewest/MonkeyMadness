using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = false;
	
	public float moveForce;
	public float maxGroundSpeed;
	public float speedBeforeForce;
	public float jumpForce;
	public float maxAngleClimb;
	public float timeTillNotGrounded;
	public float timeTillFalling;
	public float timeForJumpToTake;

	public bool isBlasted = false;
	public bool isFalling = false;
	public bool isGrounded = true;
	public bool isJumping = false;
	public bool isCollided = false;
	public int state = 0; // 0=idle, 1=walk, 2=jumping, 3=falling, 4=blasted, 5=walk-backwards

	public float lastCollidedTime;
	public float lastJumpTime;

	public Vector2 terrainDirection;

	public MonkeyAnimationController animationController;

	
	void Awake()
	{
		if(!transform.parent.networkView.isMine && (Network.isClient || Network.isServer))
		{
			Destroy(GetComponent<Rigidbody2D>());
			GetComponent<BoxCollider2D>().isTrigger = true;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		OnCollision(collision);
	}

	void OnCollisionStay2D(Collision2D collision)
	{
		OnCollision(collision);
	}

	void OnCollision(Collision2D collision)
	{
		if(isJumping && Time.time < lastJumpTime + timeForJumpToTake)
			return;


		//string log = collision.contacts.Length + ": ";
		//foreach(ContactPoint2D cp in collision.contacts)
		//{
		//	Vector2 v = new Vector2(collision.contacts[0].normal.y, -collision.contacts[0].normal.x);
		//	log = log + v.ToString();
		//}
		//Debug.Log (log);

		terrainDirection.Set(Mathf.Abs(collision.contacts[0].normal.y), -collision.contacts[0].normal.x);

		isCollided = true;
		lastCollidedTime = Time.time;
		
		if(Vector2.Angle(terrainDirection, Vector2.right) < maxAngleClimb)
		{
			isJumping = false;
			isFalling = false;
			isGrounded = true;
		}
		else
		{
			terrainDirection.Set(1, 0);
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		terrainDirection.Set(collision.contacts[0].normal.y, -collision.contacts[0].normal.x);

		if(Vector2.Angle(terrainDirection, Vector2.right) >= maxAngleClimb)
			terrainDirection.Set(1, 0);

		lastCollidedTime = Time.time;

		isCollided = false;

	}

	void Update()
	{
		if(!transform.parent.networkView.isMine && (Network.isClient || Network.isServer))
			return;

		CheckForFallingAndGrounded();
		ApplyMovement();
		ApplyJump();
	}

	void CheckForFallingAndGrounded()
	{

		if(Time.time > lastCollidedTime + timeTillNotGrounded)
		{
			isGrounded = false;
		}

		if(Time.time > lastCollidedTime + timeTillFalling)
		{
			isFalling = true;
			state = 3;
		}
	}

	void ApplyMovement()
	{
		float horizontalInput = Input.GetAxis("Horizontal");

		if(horizontalInput != 0)
		{
			rigidbody2D.isKinematic = false;
		}

		if(!isBlasted && !isFalling && !isJumping && isGrounded)
		{

			if(horizontalInput != 0 && Vector2.Angle(terrainDirection, Vector2.right) < maxAngleClimb)
			{
				state = 1;
				
				if(rigidbody2D.velocity.magnitude > maxGroundSpeed)
					rigidbody2D.velocity = rigidbody2D.velocity.normalized * maxGroundSpeed;
				else if(isCollided && rigidbody2D.velocity.magnitude < speedBeforeForce)
					rigidbody2D.AddForce(terrainDirection * horizontalInput * moveForce);
			}
			else if(horizontalInput == 0)
			{
				state = 0;
				rigidbody2D.isKinematic = true;
			}
		}


		if(horizontalInput > 0 && !facingRight)
		{
			if(Input.GetAxis("Shift") == 0)
				Flip();
			else
				state = 5;
		}
		else if(horizontalInput < 0 && facingRight)
		{
			if(Input.GetAxis("Shift") == 0)
				Flip();
			else
				state = 5;
		}
	}
	
	void ApplyJump()
	{
		if(Input.GetButtonDown("Jump") && isGrounded)
		{
			state = 2;

			rigidbody2D.isKinematic = false;
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			lastJumpTime = Time.time;

			isJumping = true;
			isGrounded = false;
		}
	}
	
	
	public void Flip()
	{
		facingRight = !facingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
}