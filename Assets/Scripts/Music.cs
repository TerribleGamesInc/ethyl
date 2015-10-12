using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Music : MonoBehaviour {

	/*DATA MEMBERS*/	
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource>();
		StartCoroutine(Enqueue());
	}

	void Update() {
		if (!source.isPlaying && Soundtrack.Instance.isLoaded) {
			Next();
		}
	}

	public void Next() {
		source.clip = Soundtrack.Instance.QueueNext();
		source.time = 0;
		source.Play();
	}

	private void RandomTrackPosition() {
		source.time = Mathf.FloorToInt(Random.Range(0, source.clip.length));
	}

	private IEnumerator Enqueue() {
		while (!Soundtrack.Instance.isLoaded) {
			yield return 0;
		}
		Soundtrack.Instance.Shuffle();
		source.clip = Soundtrack.Instance.Enqueue();
		RandomTrackPosition();
		source.Play();
	}
}