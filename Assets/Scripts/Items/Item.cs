using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public virtual void OnPickup() {
		//Destory the Item on pick up.
		Destroy(this.gameObject);
	}
}