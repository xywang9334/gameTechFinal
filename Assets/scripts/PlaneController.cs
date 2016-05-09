using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

	private Material material;
	private bool hasCollider = true;
	
	public Color color;
	


	// Use this for initialization
	void Start () {
		material = gameObject.GetComponent<Renderer>().material;
		material.SetColor("_Color", color);
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
}
