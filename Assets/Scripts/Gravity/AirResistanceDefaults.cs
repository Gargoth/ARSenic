using UnityEngine;

// NOTE: This script is from the following source
// https://thearchitect4855.itch.io/unity-air-resistance
// Placed in the Assets\Scripts\Gravity folder to be included in the assembly definition

namespace AirResistance2
{
	[System.Serializable]
	public class AirResistanceDefaults : ScriptableObject
	{
		public float defaultAirDensity = 1.225f;
		public float defaultDensityVariation = 0.5f;

		private void OnValidate()
		{
			defaultAirDensity = Mathf.Max(0, defaultAirDensity);
			defaultDensityVariation = Mathf.Clamp(defaultDensityVariation, 0, 1);
		}
	}
}