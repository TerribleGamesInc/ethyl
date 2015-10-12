using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : Singleton<GameController> {

	/*DATA MEMEMBERS*/
	GameObject player;
	Movement   movement;

	//Declare private class level constant variables.
	private const float DEFAULT_FADE = 5.0f;	//The default fade time for the screen and audio.
	private float _bac;							//The blood aclohol content of the player.
	private float _vomitAmount;
	private float _vomitTimeRemaining;			//The amount of time in seconds before the player vomits.
	private List<Alcohol> drinks;

	public float BAC {
		get {return _bac;}
	}

	public float VomitAmount {
		get {return _vomitAmount;}
		set {_vomitAmount = value;}
	}

	public float VomitTimeRemaining {
		get {return _vomitTimeRemaining;}
		set {_vomitTimeRemaining = value;}
	}


	
	//private string mainMenu = "000 - Main Menu";	//The name of the main menu scene.
		
	/**
	 * Method Name: Start
	 * Description: Method executes once after the Awake method.
	 */ 
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		movement = player.GetComponent<Movement>();
		//Start audio fade in.
		AudioController.Instance.AudioFadeIn(DEFAULT_FADE);
		//Shuffle the soundtrack.
		Soundtrack.Instance.soundtrack.Shuffle();
		//Start scene fade in.
		//FadeController.Instance.SceneFadeIn(DEFAULT_FADE);
	}
	void Update() {
		if (VomitTimeRemaining <= 0) {
			Vomit();
		}
	}
	/**
	 * Method Name: OnLevelWasLoaded
	 * Description: Method handles behavior when a new level is loaded.
	 * @param index.
	 */ 
	void OnLevelWasLoaded(int index) {
		//Print that the level was loaded.
		Debug.Log ("Loaded");
		//If the index of the current level is the loaded level...
		if(index == Application.loadedLevel) {
			//...Stop all the active coroutines.
			StopAllCoroutines();
			//...Start audio fade in.
			AudioController.Instance.AudioFadeIn(DEFAULT_FADE);
			//...Start scene fade in.
			//FadeController.Instance.SceneFadeIn(DEFAULT_FADE);
		}
	}
	public void AddDrink(Alcohol drink) {
		drinks.Add(drink);
	}
	private void CalculateBloodAlcohol() {
		float volume = 0;
		foreach (Alcohol drink in drinks) {
			volume += drink.abv * drink.ounces;
		}
		_bac = volume; 
	}
	private void Vomit() {
		//Disable the character movement.
		movement.enabled = false;
		//send message
		//object.sendmessage(AddVomit(_vomit));
	}
	/**
	 * Method Name: LoadLevel
	 * Description: Method loads a specified level.
	 * @param level, duration
	 */ 
	public void LoadLevel(string level, float duration) {
		//Start the LoadLevel coroutine.
		StartCoroutine(LoadLevelCoroutine (level, duration));
	}
	/**
	 * Method Name: RestartLevel
	 * Description: Method reloads the currently loaded level.
	 */ 
	public void RestartLevel() {
		//Start the LoadLevel coroutine.
		StartCoroutine(LoadLevelCoroutine(Application.loadedLevelName, DEFAULT_FADE));
	}
	/**
	 * Method Name: ResetGame
	 * Description: Method reloads the entire game.
	 */ 
	public void ResetGame() {
		//Start the LoadLevel coroutine.
		//StartCoroutine(LoadLevelCoroutine (mainMenu, DEFAULT_FADE));
	}
	/**
	 * Coroutine Name: LoadLevelCoroutine
	 * Description   : Coroutine loads the specified level.
	 */ 
	private IEnumerator LoadLevelCoroutine(string level, float duration) {
		//Print that the level is loading.
		Debug.Log ("Loading level...");
		//Fade out the scene.
		//FadeController.Instance.SceneFadeOut(duration);
		//Fade out the audio.
		AudioController.Instance.AudioFadeOut(duration);
		//Wait for a period of time.
		yield return new WaitForSeconds(duration + 1.0f);
		//Load the level specified.
		Application.LoadLevel(level);
	}
}