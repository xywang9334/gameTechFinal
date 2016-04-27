using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

	private float TimeToDisappear = 1.0f;
	private float TimeToAppear = 1.0f;
	private bool state = true; // it appears
	private Material material;
	private bool hasCollider = true;
	
	public Color color;
	


	// Use this for initialization
	void Start () {
		material = gameObject.GetComponent<Renderer>().material;
		material.SetColor("_Color", color);
		StartCoroutine(DisappearAndReappear(TimeToDisappear, TimeToAppear));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void changeColor(Collider collide) {
		if (collide.CompareTag("ball")) {
			if(LevelManager.Instance.isred && color == Color.red) {
				MeshCollider collider = gameObject.GetComponent<MeshCollider>();
				Destroy (collider);
				hasCollider = false;
			}
			else if(!LevelManager.Instance.isred && color == Color.blue) {
				print ("delete blue");
				MeshCollider collider = gameObject.GetComponent<MeshCollider>();
				Destroy (collider);
				hasCollider = false;
			}
		}
	}
	
	void OnTriggerEnter(Collider collide) {
		changeColor(collide);
	}
	
	void OnTriggerExit(Collider collide) {
		if( !hasCollider) {
			gameObject.AddComponent<MeshCollider>();
			hasCollider = true;
		}
	}
	
	
	void OnTriggerStay(Collider collide) {
		changeColor(collide);
	}
	
	private IEnumerator DisappearAndReappear(float TimeToDisappear, float TimeToAppear) {
		while (true) {
			yield return StartCoroutine(WaitAndDisappear(TimeToDisappear));		
			yield return StartCoroutine(WaitAndAppear(TimeToAppear));
		}
	}
	
	private IEnumerator WaitAndDisappear(float TimeToDisappear) {
		yield return new WaitForSeconds(TimeToDisappear);

	}
	
	private IEnumerator WaitAndAppear(float TimeToAppear) {
		yield return new WaitForSeconds(TimeToAppear);

	}
}
