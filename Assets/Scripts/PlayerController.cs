using UnityEngine;


public class PlayerController : MonoBehaviour {

    private Transform pTransform;
	private Rigidbody2D pRigidbody;
	
	private float move;
	private float jump;

	private bool isFacingRight;
	private bool isGrounded;
	private bool playerWon;

	private Animator animator;

	public GameController gameController;

	public float velocity = 20f;
	public float jumpForce = 400f;

	public Transform SpawnPoint;
	public Camera gameCamera;

	public Transform bullet;
	public float bulletDistance = .3f;
	public float timeBetweenFires = .3f; 
	private float timeTilNextFire = 0.0f;

	[Header("SoundClips")]
	public AudioClip jumpSound;
	public AudioClip landSound;
	public AudioClip fireSound;
	public AudioClip emptySound;
	public AudioClip cockSound;
	public AudioClip deadSound;
	public AudioClip winSound;
	private AudioSource audioSource;

	void Start () {

        this.initialize();
	}


	void Update() {
			if (Input.GetKeyDown(KeyCode.Space) && !playerWon) {
				this.jump = 1f;
				audioSource.PlayOneShot(this.jumpSound);
			}
	}
	
	void FixedUpdate () {

		if (!playerWon){

			if (this.isGrounded) {
				this.move = Input.GetAxis("Horizontal");
				if (this.move > 0f) {
					this.move = 1;
					this.animator.SetInteger("HeroState", 1);
					this.isFacingRight = true;
					this.flipHorizontal();
				} else if (this.move < 0f) {
					this.move = -1;
					this.animator.SetInteger("HeroState", 1);
					this.isFacingRight = false;
					this.flipHorizontal();
				} else {
					this.move = 0;
					this.animator.SetInteger("HeroState", 0);
				}

				if (Input.GetAxis ("Fire1") == 1) this.fire();
				timeTilNextFire -= Time.deltaTime;

				this.pRigidbody.AddForce(new Vector2(
					this.move * this.velocity, 
					this.jump * this.jumpForce),
					ForceMode2D.Force);
			}
			else {
				this.move = 0f;
				this.jump = 0f;
			}

			this.gameCamera.transform.position = new Vector3(
				this.pTransform.position.x,
				this.pTransform.position.y,
				-10f);

		} else {
			this.pRigidbody.AddForce(new Vector2(0f, 
					1f * (this.jumpForce / 8f)),
					ForceMode2D.Force);
			gameController.gameOver();
		}

	}

	private void initialize(){
		this.pTransform = GetComponent<Transform>();
		this.pRigidbody = GetComponent<Rigidbody2D>();
		this.animator = GetComponent<Animator>();
		this.audioSource = GetComponent<AudioSource>();
		this.move = 0f;
		this.isFacingRight = true;
		this.isGrounded = false;
		this.playerWon = false;
	}

	private void flipHorizontal() {
		if (this.isFacingRight) {
			this.pTransform.localScale = new Vector2(1f,1f);
		}
		else {
			this.pTransform.localScale = new Vector2(-1f,1f);
		}
	}

	private void fire(){
		if (timeTilNextFire <= 0) {
			if (gameController.playerAmmo == 0) {
				audioSource.PlayOneShot(this.emptySound);
			} else {
				Vector3 bulletPos = this.pTransform.position;

				bulletPos.x = bulletPos.x + (bulletDistance * this.pTransform.localScale.x);
				bulletPos.y += 0.16f;

				bullet.transform.localScale = this.pTransform.localScale;
				Instantiate (bullet, bulletPos, this.pTransform.rotation);
				this.animator.SetInteger("HeroState", 10);
				audioSource.PlayOneShot(this.fireSound);
				gameController.updateAmmoCount(-1);
			}
			timeTilNextFire = timeBetweenFires;
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("DeathPlane") || other.gameObject.CompareTag("Enemy")) {
			audioSource.PlayOneShot(this.deadSound);
			gameController.updateLivesCount(-1);

			if (gameController.playerLives >= 0) {
				this.pTransform.position = this.SpawnPoint.position;
			} else {
				gameController.gameOver();
				Destroy(this.gameObject);
			}
		
		}
		
		if (other.gameObject.CompareTag("Platform")) {
			audioSource.PlayOneShot(this.landSound);
			this.animator.SetInteger("HeroState", 0);
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (other.gameObject.CompareTag("Platform")) {
			this.isGrounded = true;
		}
	}

	private void OnCollisionExit2D(Collision2D other) {
		this.isGrounded = false;
		this.animator.SetInteger("HeroState", 2);
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if (other.gameObject.CompareTag("AmmoPickup")) {
			other.gameObject.SendMessage("DestroyPickup");
			gameController.updateAmmoCount(+6);
			gameController.updateScore(+50);
			audioSource.PlayOneShot(this.cockSound);
		}

	}


    public void Displace(Vector3 displacement) {
		this.pTransform.position += displacement;
	}
}
