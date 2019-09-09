using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rampart.Remake {

    public class GameManager : MonoBehaviourPunCallbacks {

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
        }

        // NETWORK
        void LeaveRoom() {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer) {
            base.OnPlayerEnteredRoom(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            base.OnPlayerLeftRoom(otherPlayer);
        }

        public override void OnLeftRoom() {
            base.OnLeftRoom();
            SceneManager.LoadScene(0);
        }

        public void OnLeaveGameButtonPressed() {
            this.LeaveRoom();
        }




        public void SetGameMode(GameMode newGameMode) {
            // change GameMode over newtork
            Hashtable roomGameMode = new Hashtable {
                { _gameModePropertyKey, newGameMode }
            };
            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomGameMode);
            } else {
                _currentGameMode = newGameMode;
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            if (propertiesThatChanged.ContainsKey(_gameModePropertyKey)) {
                GameMode newGameMode = (GameMode)propertiesThatChanged[_gameModePropertyKey];
                _currentGameMode = newGameMode;
            }
        }

        public GameMode GetGameMode() {
            return _currentGameMode;
        }
    }

}