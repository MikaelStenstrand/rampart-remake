using UnityEngine;

namespace Rampart.Remake { 

    [CreateAssetMenu(fileName = "New Cannon Property", menuName = "Cannon/Cannon Property")]
    public class CannonProperty : ScriptableObject {

        public string CannonName;
        public float CoolDown;

        // Projectile
        public GameObject ProjectilePrefab;
        public float ProjectileTravelTime;

    }
}