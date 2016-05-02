using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(ParticleController))]
public class BallController : MonoBehaviour {

	// Use this for initialization
	public Rigidbody rb;
	public Text text;
	private int doubleJump = 1;
	private Material material;
	private GameObject arrowUI;
	private GameObject spaceUI;
//	bool hasCollider = true;
	private bool arrowUIdone = false;
	private bool spaceUIdone = false;
	ParticleController mParticlesController = null;
	void Start () {
		if (LevelManager.Instance.levelNum == 0 && arrowUI != null && spaceUI != null) {
			arrowUI = GameObject.Find ("ArrowKeys");
			spaceUI = GameObject.Find ("SpaceKey");
			spaceUI.SetActive (false);
		}
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
			if (LevelManager.Instance.levelNum == 0 && !arrowUIdone) {
				arrowUIdone = true;
				StartCoroutine (UICoroutine(0));

			}
		}
		
		if (Input.GetKey(KeyCode.RightArrow)) {
			rb.AddForce(Vector3.right/20, ForceMode.Impulse);
			position.x += 0.08f;
			if (LevelManager.Instance.levelNum == 0 && !arrowUIdone) {
				arrowUIdone = true;
				StartCoroutine (UICoroutine(0));
			}
		}
		transform.position = position;
		this.transform.position = position;
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if (doubleJump < 1) {
				doubleJump++;
				rb.AddForce(Vector3.up * 14, ForceMode.Impulse);
			}
			if (LevelManager.Instance.levelNum == 0&& !arrowUIdone) {
				arrowUIdone = true;
				StartCoroutine (UICoroutine(0));
			}
			
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			//spaceUIdone = true;
			if (LevelManager.Instance.levelNum == 0) {
				if (arrowUIdone && !spaceUIdone) {
					spaceUIdone = true;
					StartCoroutine (UICoroutine(1));
				}
				else
					spaceUIdone = true;

			}

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



	IEnumerator UICoroutine(int i){
		if(i == 0){
			yield return new WaitForSeconds (1f);
			text = GameObject.Find ("ArrowText").GetComponent<Text>();
			float duration = 0.5f; //0.5 secs
			float currentTime = 0f;
			while(currentTime < duration)
			{
				float alpha = Mathf.Lerp(1f, 0f, currentTime/duration);
				text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
				currentTime += Time.deltaTime;
				yield return null;
			}
			arrowUI.SetActive (false);
			if (!spaceUIdone) {
				spaceUI.SetActive (true);
				text = GameObject.Find ("SpaceText").GetComponent<Text>();
				duration = 0.5f; //0.5 secs
				currentTime = 0f;
				while(currentTime < duration)
				{
					float alpha = Mathf.Lerp(1f, 0f, currentTime/duration);
					text.color = new Color(text.color.r, text.color.g, text.color.b, 1f - alpha);
					currentTime += Time.deltaTime;
					yield return null;
				}

			}
		}
		if (i == 1) {
			yield return new WaitForSeconds (1f);
			try{
				text = GameObject.Find ("SpaceText").GetComponent<Text>();
			}
			catch{
			}
			if (text != null) {
				float duration = 0.5f; //0.5 secs
				float currentTime = 0f;
				while (currentTime < duration) {
					float alpha = Mathf.Lerp (1f, 0f, currentTime / duration);
					text.color = new Color (text.color.r, text.color.g, text.color.b, alpha);
					currentTime += Time.deltaTime;
					yield return null;
				}
				yield break;
//				spaceUI.SetActive (false);
			}
		}
	}

}