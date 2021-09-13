using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
	Rigidbody2D rb;
	public float speed;
	public float jumpForce;
	bool isGrounded = false;
	public Transform isGroundedChecker;
	public float checkGroundRadius;
	public LayerMask groundLayer;
	public LayerMask deathLayer;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	bool isDead = false;
	public float rememberGroundedFor;
	float lastTimeGrounded;
	public int defaultAdditionalJumps = 1;
	int additionalJumps;
	public AudioSource source;
	public AudioClip jumpClip;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	void Update() {
		Move();
		Jump();
		BetterJump();
		CheckIfGrounded();
		CheckIfDead();
	}

	void Move() {
		float x = Input.GetAxisRaw("Horizontal");
		float moveBy = x * speed;
		rb.velocity = new Vector2(moveBy, rb.velocity.y);
	}

	void Jump() {
		if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || Time.time - lastTimeGrounded <= rememberGroundedFor || additionalJumps > 0)) {
			source.PlayOneShot(jumpClip, 0.5f);
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			additionalJumps--;
		}
	}

	void BetterJump() {
		if (rb.velocity.y < 0) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

	void CheckIfGrounded() {
		Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
		if (collider != null) {
			isGrounded = true;
			additionalJumps = defaultAdditionalJumps;
		} else {
			if (isGrounded) {
				lastTimeGrounded = Time.time;
            }
			isGrounded = false;
		}
	}

	void CheckIfDead() {
		Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, deathLayer);
		if (collider != null) {
			isDead = true;
		} else {
			isDead = false;
		}
		if (isDead) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
