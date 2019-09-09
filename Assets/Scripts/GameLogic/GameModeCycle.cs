using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake { 

    public class GameModeCycle : MonoBehaviour {

        [SerializeField]
        KeyCode _debugChangeGameMode = KeyCode.G;

        [SerializeField]
        GameModeProperty _gameModeProperty;

        GameManager _gameManager;
        float _nextTimeToChangeGameMode;

        void Start() {
            _gameManager = GameManager.instance;
            _nextTimeToChangeGameMode = Time.time + _gameModeProperty.BuildTime;
        }

        void Update() {
            this.DebugCheckGameModeChangeInput();
            if (PhotonNetwork.IsConnected && PhotonNetwork.IsMasterClient) {
                this.HandleGameModeIntervalUpdates();
            }
        }

        public float GetTimeToNextGameMode() {
            return _nextTimeToChangeGameMode - Time.time;
        }

        // TODO: RPC to update timer to all clients
        // wall pieces bug into the floor...
        void HandleGameModeIntervalUpdates() {
            if (_gameManager.GetGameMode() == GameMode.BUILD) {
                if (Time.time >= _nextTimeToChangeGameMode) {
                    _gameManager.SetGameMode(GameMode.PLACE_CANNON);
                    _nextTimeToChangeGameMode = Time.time + _gameModeProperty.GetGameModeTime(GameMode.PLACE_CANNON);
                }
            } else if (_gameManager.GetGameMode() == GameMode.PLACE_CANNON) {
                if (Time.time >= _nextTimeToChangeGameMode) {
                    _gameManager.SetGameMode(GameMode.ATTACK);
                    _nextTimeToChangeGameMode = Time.time + _gameModeProperty.GetGameModeTime(GameMode.ATTACK);
                }
            } else if (_gameManager.GetGameMode() == GameMode.ATTACK) {
                if (Time.time >= _nextTimeToChangeGameMode) {
                    _gameManager.SetGameMode(GameMode.BUILD);
                    _nextTimeToChangeGameMode = Time.time + _gameModeProperty.GetGameModeTime(GameMode.BUILD);
                }
            }
        }







        void DebugCheckGameModeChangeInput() {
            if (Input.GetKeyDown(_debugChangeGameMode)) {
                if (_gameManager.GetGameMode() == GameMode.BUILD)
                    _gameManager.SetGameMode(GameMode.PLACE_CANNON);
                else if (_gameManager.GetGameMode() == GameMode.PLACE_CANNON)
                    _gameManager.SetGameMode(GameMode.ATTACK);
                else if (_gameManager.GetGameMode() == GameMode.ATTACK)
                    _gameManager.SetGameMode(GameMode.BUILD);
            }
        }


    }
}