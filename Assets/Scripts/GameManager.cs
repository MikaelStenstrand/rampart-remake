using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rampart.Remake {

    public class GameManager : MonoBehaviourPunCallbacks {

        [SerializeField]
        KeyCode _debugChangeGameMode = KeyCode.G;

        GameMode _currentGameMode = GameMode.BUILD;

        #region Singelton
        public static GameManager instance;

        void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Debug.LogWarning("More than one instance of GameManager exists!");
                return;
            }
        }
        #endregion Singelton

        const string _gameModePropertyKey = "GameMode";

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                LeaveRoom();
            }
            this.DebugCheckGameModeChangeInput();
        }

        void DebugCheckGameModeChangeInput() {
            if (Input.GetKeyDown(_debugChangeGameMode)) {
                if (this.GetGameMode() == GameMode.BUILD)
                    this.SetGameMode(GameMode.ATTACK);
                else
                    this.SetGameMode(GameMode.BUILD);
            }
        }

        void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }


        public override void OnPlayerEnteredRoom(Player newPlayer) {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.LogFormat("OnPlayerEnteredRoom: Player entered: {0}", newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.LogFormat("OnPlayerLeftRoom: Player left: {0}", otherPlayer);
        }


        public override void OnLeftRoom() {
            base.OnLeftRoom();
            Debug.Log("NETWORK: OnLeftRoom()");
            SceneManager.LoadScene(0);
        }

        public void OnLeaveGameButtonPressed() {
            Debug.Log("OnLeaveGameButtonPressed");
            this.LeaveRoom();
        }

        public void SetGameMode(GameMode newGameMode) {
            // change over newtork
            Hashtable roomGameMode = new Hashtable {
                { _gameModePropertyKey, newGameMode }
            };
            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomGameMode);
            } else {
                Debug.LogWarning("Not connected! Game mode changed locally only!");
                _currentGameMode = newGameMode;
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            Debug.Log("OnRoomPropertiesUpdate:" + propertiesThatChanged);
            if (propertiesThatChanged.ContainsKey(_gameModePropertyKey)) {
                GameMode newGameMode = (GameMode)propertiesThatChanged[_gameModePropertyKey];
                _currentGameMode = newGameMode;
                Debug.Log("GameMode Set to: " + _currentGameMode);
            }
        }

        public GameMode GetGameMode() {
            return _currentGameMode;
        }
    }

}