using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshRenderer))]
public class RandomMaterial : MonoBehaviour {

	public Material[] materials;
	public MeshRenderer mr;

	// Use this for initialization
	void Start () {
		if (mr == null)
			mr = GetComponent<MeshRenderer> ();
		AssignRandomMaterial ();
	}
	
	void AssignRandomMaterial() {
		int a = Random.Range(0, materials.Length);
		mr.material = materials[a];
	}
}
