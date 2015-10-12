using UnityEngine;
using System.Collections;

public class Drink : Item {
	public Alcohol alcohol = new Alcohol();
	public float ounces;

	public void ConsumeAlochol() {
		if(alcohol.ounces <= this.ounces) {
			GameController.Instance.AddDrink(alcohol);
		}
		else if(this.ounces == 0) {
			Destroy(this);
		}
	}	
}