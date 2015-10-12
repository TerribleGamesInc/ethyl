using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class RandomizeList {
	public static void Shuffle<T>(this IList<T> list) {
		for(var index = 0; index < list.Count; index++)
			list.Swap(index, Random.Range(index, list.Count));
	}
	public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
		var temp = list[indexA];
		list[indexA] = list[indexB];
		list[indexB] = temp;
	}
}