using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour {

	public Color color = Color.red;
	public Vector3 segmentDir = new Vector3(1.0f, 0.0f, 0.0f);
	public float startingAngle = 0.0f;
	private Mesh m_Mesh;
	ParticleSystem mPrSystem;
	ParticleSystem.Particle[] particles;
	int mCurrCount = 0;
	static int MaxValueToHide = 10000;

	public float timeToDie = 5.0f;

	private int mVertexCount = 0;


	// Use this for initialization
	void Awake () {
		initializeParticleSystem();
	}

	void initializeParticleSystem() {
		mPrSystem = GetComponent<ParticleSystem>();
		mPrSystem.startColor = color;
		mPrSystem.startSize = 3;
		if (particles == null) {
			particles = new ParticleSystem.Particle[mPrSystem.particleCount];
			mPrSystem.GetParticles(particles);
		}
		for (int i = 0; i < mPrSystem.particleCount; i ++) {
			particles[i].color = color;
			particles[i].rotation = 90;
		}
		mPrSystem.SetParticles (particles, mPrSystem.particleCount);
		mPrSystem.Play ();
	}

	public void UpdateSegments () {
		
		mPrSystem.GetParticles(particles);
		clearOldParticles();
		mPrSystem.SetParticles (particles, mPrSystem.particleCount);
	}

	public void clearOldParticles() {
		for (int i = 0; i < particles.Length; i ++) {
			float timeAlive = particles[i].startLifetime - particles[i].lifetime;
			if (timeAlive > timeToDie) {
				particles[i].lifetime = -1;
			}
		}
	}


	public bool isReadyToUse () {
		return mPrSystem.particleCount > 0;
	}

	public void SetColor(Color acolor) {
		for (int i = 0; i < particles.Length; i ++) {
			particles [i].color = color;
		}
		color = acolor;
		mPrSystem.startColor = acolor;
		mPrSystem.SetParticles (particles, mPrSystem.particleCount);
	}

	static float AngleAroundAxis(Vector3 dirA, Vector3 dirB) {
		float res = Vector3.Angle (dirA, dirB);
		res *= Vector3.Dot(Vector3.up, Vector3.Cross(dirA, dirB)) < 0.0f ? -1.0f : 1.0f;
		return res;
	}

	// Update is called once per frame
	void LateUpdate () {
		if (particles == null) {
			initializeParticleSystem();
		}
		UpdateSegments ();
	}
}