using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {
	
	[SerializeField]
	private float rotateSpeed = 0.001f; // In rotations per second
	
	// Use this for initialization
	void Start () {
		StartCoroutine (Spin ());
	}
	
	private IEnumerator Spin()
	{
		while (true)
		{
			transform.Rotate (Vector3.forward, 360 * rotateSpeed * Time.deltaTime);
			yield return 0;
		}
	}
}
