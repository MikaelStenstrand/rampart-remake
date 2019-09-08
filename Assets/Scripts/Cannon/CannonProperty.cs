using UnityEngine;

namespace Rampart.Remake { 

    [CreateAssetMenu(fileName = "New Cannon Property", menuName = "Cannon/Cannon Property")]
    public class CannonProperty : ScriptableObject {

        public string CannonName;
        public GameObject CannonPrefab;

        // rotate speed
        // Rate of fire
        // description
        // damange
        // accuracy
        // hit effect

        public GameObject ProjectilePrefab;
        public float ProjectileTravelTime;


        // not currently




    }
}