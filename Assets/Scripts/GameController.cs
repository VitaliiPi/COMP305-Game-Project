using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {


	public int initialPlayerLives = 5;
	public int initialPlayerAmmo = 0;
	public int maxPlayerAmmo = 6;

	public int playerLives;
	public int playerAmmo;
	public int playerScore = 0;

	public Canvas canvas;
	public Text scoreText;
	public Text gameOverText;
    public GameObject button;

    [Header("SoundClips")]
	public AudioClip enemyDeadSound;
	public AudioClip gameOverSound;

	private AudioSource audioSource;

	private GameObject[] ammo;
	private GameObject[] lives;

	void Start () {

		gameOverText.gameObject.SetActive(false);

        button = GameObject.Find("Button");

        audioSource = GetComponent<AudioSource>();

		ammo = new GameObject[maxPlayerAmmo];
		for (int i = 0; i < maxPlayerAmmo; i++){
			ammo[i] = GameObject.Find("Ammo" + (i+1));
		}

		lives = new GameObject[initialPlayerLives];
		for (int i = 0; i < initialPlayerLives; i++){
			lives[i] = GameObject.Find("Life" + (i+1));
		}
	
		playerLives = initialPlayerLives;
		this.updateAmmoCount(initialPlayerAmmo);
	}

	void Update () {
		updateUI();
	}

	private void updateUI(){
		scoreText.text = "Score: " + playerScore;
	}

	public int updateLivesCount(int delta) {
		playerLives += delta;
		for (int i = 0; i < initialPlayerLives; i++){
			lives[i].SetActive(i < playerLives); 
		}
		return this.playerLives;
	}

	public int updateAmmoCount(int delta) {
		playerAmmo += delta;
		if (playerAmmo > maxPlayerAmmo) playerAmmo = maxPlayerAmmo; 
		for (int i = 0; i < maxPlayerAmmo; i++){
			ammo[i].SetActive(i < playerAmmo); 
		}
		return this.playerAmmo;
	}

	public void updateScore(int delta) {
		playerScore += delta;
	}

	public void enemyDied() {
		audioSource.PlayOneShot(this.enemyDeadSound);
	}

	public void gameOver() {
		if (playerLives < 0) {
			gameOverText.text = "GAME OVER\n";
			audioSource.PlayOneShot(this.gameOverSound);
		} else {
			gameOverText.text = "YOU WON!\n";
		}
		gameOverText.text += "SCORE: " + playerScore;
		gameOverText.gameObject.SetActive(true);
	}
}
