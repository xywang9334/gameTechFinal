using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour {

	public Color color = Color.red;
	ParticleSystem mPrSystem;
	ParticleSystem.Particle[] particles;

	public float timeToDie = 5.0f;



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
		mPrSystem.startColor = color;
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