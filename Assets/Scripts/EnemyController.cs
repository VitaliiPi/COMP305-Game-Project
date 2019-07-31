using UnityEngine;
using System.Text.RegularExpressions;


public class EnemyController : MonoBehaviour {

	private Transform eTransform;
	private GameObject gameController;

	public float speed = 2f;
	public int enemyPoints = 200;
	
	public Transform movementStart;
	public Transform movementEnd;
	private bool reverseMovement = false;
	

	void Start () {
		this.eTransform = GetComponent<Transform>();
		gameController = GameObject.Find("GameController");

		string instanceNumber = this.getInstanceNumber(gameObject.ToString());

		GameObject found;
		if (!this.movementStart) {
			found = GameObject.Find("E" + instanceNumber + "Start");
			if (found) this.movementStart = found.transform;
		}

		if (!this.movementEnd) {
			found = GameObject.Find("E" + instanceNumber + "End");
			if (found) this.movementEnd = found.transform;
		}
	}
	
	void FixedUpdate () {
		
		if (!this.movementStart || !this.movementEnd) return;
		
		Debug.DrawLine(this.movementStart.position, 
				this.movementEnd.position);
		
		Vector2 movementDestination;
		if (reverseMovement == false) {
			movementDestination = new Vector2(
				this.movementEnd.position.x,
				this.movementEnd.position.y);
		} else {
			movementDestination = new Vector2(
				this.movementStart.position.x,
				this.movementStart.position.y);
		}
		
		this.eTransform.position = Vector2.MoveTowards(
			new Vector2(this.eTransform.position.x, this.eTransform.position.y),
			movementDestination,
			this.speed * Time.deltaTime
		);

		if (this.eTransform.position.x == movementDestination.x && this.eTransform.position.y == movementDestination.y) {
			reverseMovement = !reverseMovement;
		}

	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Bullet")) {
			other.gameObject.SendMessage("DestroyBullet");
			gameController.SendMessage("enemyDied");
			gameController.SendMessage("updateScore", this.enemyPoints);
			Destroy(this.gameObject);
		}
	}

	private string getInstanceNumber(string objName) {
		Regex rx = new Regex(@"([0-9]+)");
		MatchCollection matches = rx.Matches(objName);
		return matches[0].ToString();
	}

}
