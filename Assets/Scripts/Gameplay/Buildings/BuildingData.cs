using UnityEngine;

namespace Gameplay.Buildings
{
    [CreateAssetMenu(fileName = "BuildingData", menuName = "GameSO/BuildingData")]
    public class BuildingData : ScriptableObject
    {
        public string name;
        public int Cost;
        public GameObject building;
    }
}