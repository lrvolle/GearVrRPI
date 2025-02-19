﻿using UnityEngine;
using System.Collections;

public class EchoSpawner : MonoBehaviour {
    //initialize vars
    public float force = 5f;
    private float buttonPressTime = 0;
    private float minWarmUp = .2f;
    public float maxWarmUp =              2f;
    public GameObject echo;
    public AudioClip Chirp1;
    public AudioClip Chirp2;
    private AudioSource chirpSource;
    public AudioClip Hum;
    private AudioSource humSource;


    // Use this for initialization
    void Start () {
        chirpSource = gameObject.AddComponent<AudioSource>();
        chirpSource.volume = 0.4f;
        humSource = gameObject.AddComponent<AudioSource>();
        humSource.volume = 0.1f;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("AButton"))
        {
            buttonPressTime = Time.time;
            if (!humSource.isPlaying)
            {
                humSource.clip = Hum;
                humSource.Play();
            }
        }
        if (Input.GetButtonUp("AButton") && Time.time > buttonPressTime + minWarmUp)
        {
            buttonPressTime = Time.time - buttonPressTime;
            if (buttonPressTime > maxWarmUp)
                buttonPressTime = maxWarmUp;
            int echoSize = (int)Mathf.Round(buttonPressTime * 50);

            //distribute echoSize points in circle, using a sunflower seed arrangement 
            int boundPts = (int)Mathf.Round(2 * Mathf.Sqrt((float)echoSize));
            float phi = (Mathf.Sqrt(5) + 1) / 2;
            for (float i = 0; i < echoSize; i++)
            {
                float radius = 10* Mathf.Sqrt(i - 0.5f) / Mathf.Sqrt(echoSize - (boundPts + 1) / 2);
                float theta = 2 * Mathf.PI * i / (phi * phi);
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                Vector3 circleCenter = transform.position + transform.forward * (force - 5f);
                Vector3 target = transform.rotation * new Vector3(x,y,0) + circleCenter;
                Vector3 newForce = (target - transform.position).normalized * force;
                GameObject newEcho = Instantiate(echo,
                    transform.position, transform.rotation) as GameObject;
                Rigidbody echoBody = newEcho.GetComponent<Rigidbody>();
                echoBody.AddForce(newForce, ForceMode.Impulse);
                Debug.DrawRay(transform.position, newForce, Color.red);
            }

            //play chirping sound
            chirpSource.pitch = Random.Range(0.5f, 1.5f);
            if (Random.value > .5)
            {
                chirpSource.clip = Chirp1;
            }
            else
            {
                chirpSource.clip = Chirp2;
            }
            humSource.Stop();
            chirpSource.Play();
            
        }

	}
}
