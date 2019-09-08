using Photon.Pun;
using UnityEngine;

    namespace Rampart.Remake { 
    public class ColorChanger : MonoBehaviour {

        /*
         * Expects the Game Object to have the following sturcutre
         * GAME OBJECT
         * - objects
         * -- ColorModifiedObjects (objects' color within this group will be changed)
         * -- etc.
         * -- etc.
        */

        [SerializeField]
        PlayerSettings _playerSettings;
        Color _defaultColor;

        void Awake() {
            _defaultColor = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponentsInChildren<MeshRenderer>()[0].material.color;
        }

        public void ChangeObjectColorToLocalPlayerColor() {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponentsInChildren<MeshRenderer>();
            Color color = (Color)_playerSettings.GetLocalPlayerColor();
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = color;
            }
        }

        public void ChangeObjectColorToPlayerColor(Photon.Realtime.Player player) {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponentsInChildren<MeshRenderer>();
            Color color = _playerSettings.GetPlayerColor(player);
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = color;
            }
        }

        public void ChangeToNotPlaceableColor() {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = Color.red;
            }
        }

        public void ChangeToDefaultColor() {
            Component[] renderers = gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) {
                renderer.material.color = _defaultColor;
            }
        }


    }
}