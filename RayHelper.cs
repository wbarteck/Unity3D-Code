using UnityEngine;
using System.Collections;

public class RayHelper {

	public static Vector3 RaycastHitToTag(Camera c, string tagName) {
		Ray ray = c.ScreenPointToRay (Input.mousePosition);
		RaycastHit[] hits;
		Vector3 pos = Vector3.zero + Vector3.down * 10f;
		// Get all hits
		hits = Physics.RaycastAll (ray, Mathf.Infinity);
		for (int i = 0; i < hits.Length; i++) {
			RaycastHit hit = hits [i];
			// Get closts hit to surface
			if (hit.collider.tag == tagName || hit.collider.name == "Terrain") {
				if (hit.point.y > pos.y)
					pos =  hit.point;
			}
		}
		return pos;
	}
}
