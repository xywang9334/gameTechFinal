using UnityEngine;
using System.Collections;


[RequireComponent(typeof(ParticleController))]
public class BallController : MonoBehaviour {

	// Use this for initialization
	public Rigidbody rb;
	private int doubleJump = 1;
	private Material material;
	bool hasCollider = true;
	ParticleController mParticlesController = null;
	void Start () {
		LevelManager.Instance.isred = true;
		material = gameObject.GetComponent<Renderer>().material;
		material.color = Color.red;
		Physics.gravity = new Vector3(0, -25.0f, 0);
		mParticlesController = GetComponent<ParticleController> ();
	}
		
	
	
	// Update is called once per frame
	void Update () {
		
		Vector3 position = this.transform.position;
		position.z = 0.0f;
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			rb.AddForce(Vector3.left/20, ForceMode.Impulse);
			position.x -= 0.08f;
		}
		
		if (Input.GetKey(KeyCode.RightArrow)) {
			rb.AddForce(Vector3.right/20, ForceMode.Impulse);
			position.x += 0.08f;
		}
		transform.position = position;
		this.transform.position = position;
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (doubleJump < 1) {
				doubleJump++;
				rb.AddForce(Vector3.up * 14, ForceMode.Impulse);
			}
			
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (material.color == Color.red) {
				material.SetColor("_Color", Color.blue);
				LevelManager.Instance.setColor(false);
			}
			else if (material.color == Color.blue) {
				material.SetColor("_Color", Color.red);
				LevelManager.Instance.setColor(true);
			}
			if (mParticlesController.isReadyToUse()) {
				mParticlesController.SetColor(material.color);
				Vector3 particlePos = position;
				particlePos.y += 1.0f;
//				mParticlesController.SetPosition(particlePos, 5);
			}
		}

	}
	
	
	bool checkLiveness(Vector3 position) {
		RaycastHit rayHit;
		float range = 1.0f;
		Color planeColor = Color.white;
		Ray ray = new Ray(transform.position, Vector3.down);
		
		if (Physics.Raycast(ray, out rayHit, range)) {
			if (rayHit.collider.tag == "ground") {
				Renderer renderer = rayHit.transform.gameObject.GetComponent<Renderer>();
				Material planeMaterial = renderer.material;
				planeColor = planeMaterial.color;
			}
		}
		return (planeColor == material.color);
	}
	
	void OnTriggerEnter(Collider collide) {
			doubleJump = 0;
	}

}
