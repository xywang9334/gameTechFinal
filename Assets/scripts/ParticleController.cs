using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour {

	public Color color = Color.white;
	public Vector3 segmentDir = new Vector3(1.0f, 0.0f, 0.0f);
	public float startingAngle = 0.0f;
	private Mesh m_Mesh;
	private int[] m_Indices;
	private Vector2[] m_UVs;
	ParticleSystem mPrSystem;
	ParticleSystem.Particle[] particles;
	int mCurrCount = 0;
	static int MaxValueToHide = 10000;

	private int mVertexCount = 0;


	void Awake () {
		mPrSystem = GetComponent<ParticleSystem>();
		mPrSystem.Play ();
	}

	// Use this for initialization
	void Start () {
	
	}

	void LateUpdate() {
		UpdateSegments ();
	}

	public void UpdateSegments () {
		if (particles == null) {
			return;
		}
		mPrSystem.SetParticles (particles, mPrSystem.particleCount);
	}


	public bool isReadyToUse () {
		return mPrSystem.particleCount > 0;
	}

	public void SetVertexCount (int aCount) {
		if (particles == null) {
			particles = new ParticleSystem.Particle[mPrSystem.particleCount];
			mPrSystem.GetParticles(particles);
		}
		if (mCurrCount > aCount) {
			for (int i = aCount; i < mPrSystem.particleCount; i ++) {
				particles[i].position = new Vector3(0, MaxValueToHide, 0);
			}
		}
		mCurrCount = aCount < mPrSystem.particleCount ? aCount : mPrSystem.particleCount;
		if (aCount > mPrSystem.particleCount) {
			Debug.LogError("vertex count > particle cache");
		}
	}


	public void SetPosition(int aIndex, Vector3 aPosition) {
		if (aIndex < 0 || aIndex >= mCurrCount) {
			return;
		}
		particles [aIndex].position = aPosition;
		if (aIndex > 0) {
			Vector3 dir = (particles[aIndex].position - particles[aIndex - 1].position);
			particles[aIndex].rotation = AngleAroundAxis(segmentDir, dir) + startingAngle;
			if (aIndex == 1) {
				dir = -(particles[0].position - particles[1].position);
				particles[0].rotation = AngleAroundAxis(segmentDir, dir) + startingAngle;
			}


		}
	}

	public void SetColor(Color acolor) {
		if (particles == null)
			return;
		for (int i = 0; i < mPrSystem.particleCount; i ++) {
			particles [i].color = color;
		}
	}


	public void SetScale(int aIndex, float scale) {
		if (aIndex < 0 || aIndex > mCurrCount) {
			return;
		}
		particles [aIndex].size = scale;
	}

	static float AngleAroundAxis(Vector3 dirA, Vector3 dirB) {
		float res = Vector3.Angle (dirA, dirB);
		res *= Vector3.Dot(Vector3.up, Vector3.Cross(dirA, dirB)) < 0.0f ? -1.0f : 1.0f;
		return res;
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}
