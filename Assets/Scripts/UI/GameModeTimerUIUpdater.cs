using UnityEngine;
using UnityEngine.UI;

namespace Rampart.Remake {

    public class GameModeTimerUIUpdater : MonoBehaviour {

        [SerializeField]
        Text _gameModeTimerTextUI;

        [SerializeField]
        GameModeCycle _gameModeCycle;

        void Start() {
            _gameModeTimerTextUI.text = "";
        }

        void Update() {
            _gameModeTimerTextUI.text = (_gameModeCycle.GetTimeToNextGameMode()).ToString("0");
        }
    }
}
