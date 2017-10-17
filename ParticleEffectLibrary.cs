using UnityEngine;
using System.Collections;

public class ParticleEffectLibrary : MonoBehaviour {

	public static ParticleEffectLibrary instance;

	public Effect[] effects;

	void Start() {
        if (instance == null)
		    instance = this;
	}
}
[System.Serializable]
public class Effect {
    public string effectName;
	public EffectType type;
	public enum EffectType
	{
		blood,
		metal,
		wood,
		smoke,
		spark,
        explosion
	}
	public ParticleSystem particlePrefab;

}