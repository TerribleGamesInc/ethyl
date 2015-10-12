using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Soundtrack : Singleton<Soundtrack> {
	/*DATA MEMEMBERS*/
	public List<AudioClip> soundtrack;	//A List of AudioClips that comprise the games soundtrack.

	public bool isLoaded;

	// Use this for initialization
	public override void Awake () {
		//Invoke the inherited method.
		base.Awake();
		//Initialize the soundtrack List.
		soundtrack = new List<AudioClip>();
		//Set isLoaded to false.
		isLoaded = false;
		//Load the audio tracks from the directory.
		StartCoroutine(LoadTracks());
	}
	/**
	 * Method Name: Enqueue
	 * Description: Method returns the current track to be played.
	 */ 
	public AudioClip Enqueue() {
		//Return the current track.
		return soundtrack[0];
	}
	/**
	 * Method Name: QueueNext
	 * Description: Method queues the next track.
	 */
	public AudioClip QueueNext() {
		//Add the current track to the end of the soundtrack list.
		soundtrack.Add(soundtrack[0]);
		//Remove the current track.
		soundtrack.RemoveAt(0);
		//Return the new current track.
		return soundtrack[0];
	}
	/**
	 * Method Name: QueuePrevious
	 * Description: Method queues the previous track.
	 */ 
	public AudioClip QueuePrevious() {
		soundtrack.Insert (0, soundtrack[soundtrack.Count - 1]);
		soundtrack.RemoveAt(soundtrack.Count - 1);
		//Return the new current track.
		return soundtrack[0];
	}
	/**
	 * Method Name: Shuffle
	 * Description: Method shuffles the soundtrack.
	 */ 
	public void Shuffle() {
		//Shuffle the soundtrack.
		soundtrack.Shuffle();
	}
	/**
	 * Coroutine Name: LoadTracks
	 * Description   : Coroutine will load any audio files from the specified directory into the game as audio clips.
	 */ 
	private IEnumerator LoadTracks() {
		string[] clips = System.IO.Directory.GetFiles(Application.dataPath + "/Audio/Soundtrack/", "*.wav");
		//For each file (track) in the directory...
		foreach(string clip in clips) {
			//..Load the file.
			WWW file = new WWW("file://" + clip);
			//...Extract the audio clip from the file load into an audio clip object. Parameter is true because the sound is 3D.
			AudioClip track = file.GetAudioClip(true);
			//...While the audio clip is not loaded...
			while(track.loadState != AudioDataLoadState.Loaded) {
				//...Yield the file (track).
				yield return file;
			}
			//...Assign the file name to the audio clip name.
			track.name = System.IO.Path.GetFileName(clip);
			//...Add the track to the soundtrack.
			soundtrack.Add(track);
		}
		//The soundtrack is completely loaded.
		isLoaded = true;
	}
}