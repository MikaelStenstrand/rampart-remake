using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Rampart.Remake {

    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour {

        const string _playerNamePrefKey = "PlayerName";

        private InputField _inputField;

        void Start() {
            string defaultName = string.Empty;
            _inputField = this.GetComponent<InputField>();

            if (_inputField != null) {
                if (PlayerPrefs.HasKey(_playerNamePrefKey)) {
                    defaultName = PlayerPrefs.GetString(_playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }
            this.SetPlayerName(defaultName);
        }

        public void SetPlayerName(string playerName) {
            if (string.IsNullOrEmpty(playerName))
                return;
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(_playerNamePrefKey, playerName);
        }

    }
}