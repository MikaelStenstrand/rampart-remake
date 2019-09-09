using UnityEngine;

namespace Rampart.Remake {

    [CreateAssetMenu(fileName = "New Game Mode Property", menuName = "Game Logic/Game Mode Property")]
    public class GameModeProperty : ScriptableObject {

        public float BuildTime;
        public float PlaceCannonTime;
        public float AttackTime;


        public float GetGameModeTime(GameMode gameMode) {
            switch (gameMode) {
                case GameMode.BUILD:
                    return BuildTime;
                case GameMode.PLACE_CANNON:
                    return PlaceCannonTime;
                case GameMode.ATTACK:
                    return AttackTime;
                default:
                    return 0f;
            }
        }



    }
}