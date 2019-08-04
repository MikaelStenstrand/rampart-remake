using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake {

    [RequireComponent(typeof(GridLayout))]
    public class GroundPlacementController : MonoBehaviour {

        [SerializeField]
        GameObject[] _placeableObjectPrefabs;

        [SerializeField]
        GameObject _canonPrefab;

        [SerializeField]
        KeyCode _newCanonHotkey = KeyCode.C;

        [SerializeField]
        KeyCode _rotateHotkey = KeyCode.R;

        [SerializeField]
        LayerMask _environmentLayerMask;

        GameObject _currentPlaceableObject;
        int _currentPrefabIndex = -1;
        float _currentObjectRotation = 0;
        GridLayout _gridLayout;
        Vector3 _coordinateOfCell;
        GameManager _gameManager;    

        void Awake() {
            _gridLayout = GetComponent<GridLayout>();
            _gameManager = GameManager.instance;
        }

        void Update() {
            if (_gameManager.GetGameMode() == GameMode.BUILD) {
                HandleNeweCanonInput();
                HandleNewWallInput();
                if (_currentPlaceableObject != null) {
                    MoveCurrentPlaceableObjectToMouse();
                    RotateCurrentPlaceableObject();
                    PlaceObjectInWorld();
                }
            }
        }

        void PlaceObjectInWorld() {
            if (Input.GetMouseButtonDown(0)) {
                GroundPlaceableObject currentGroundPlaceableObject = _currentPlaceableObject.GetComponent<GroundPlaceableObject>();
                if (currentGroundPlaceableObject != null && currentGroundPlaceableObject.isPlaceable == true) {
                    // destory local GO
                    Destroy(_currentPlaceableObject);
                    _currentPlaceableObject = null;
                    _currentPrefabIndex = -1;

                    // Instantiate network GO
                    this.InstantiateNetworkGO(currentGroundPlaceableObject.Filename);

                    _currentObjectRotation = 0;
                }
            }
        }

        GameObject InstantiateLocalGO(GameObject prefab) {
            GameObject GO = Instantiate(prefab);
            GroundPlaceableObject placedObject = GO.GetComponent<GroundPlaceableObject>();
            if (placedObject != null) {
                placedObject.ChangeObjectColorToPlayerColor();
            }
            return GO;
        }

        void InstantiateNetworkGO(string filename) {
            GameObject GO = PhotonNetwork.Instantiate(filename, _coordinateOfCell, Quaternion.Euler(0, _currentObjectRotation, 0));
            GroundPlaceableObject placedObject = GO.GetComponent<GroundPlaceableObject>();
            if (placedObject != null) {
                placedObject.ChangeObjectColorToPlayerColorOverNetwork();
            }
        }

        void RotateCurrentPlaceableObject() {
            if (Input.GetKeyDown(_rotateHotkey)) {
                AddRotation(90);
            }
        }

        void AddRotation(float degrees) {
            _currentObjectRotation += degrees;
            if (_currentObjectRotation >= 360) {
                _currentObjectRotation = 0;
            }
        }

        void MoveCurrentPlaceableObjectToMouse() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _environmentLayerMask)) {
                _coordinateOfCell = _gridLayout.CellToWorld(_gridLayout.WorldToCell(hitInfo.point));
                _currentPlaceableObject.transform.position = _coordinateOfCell;
                _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
                _currentPlaceableObject.transform.Rotate(Vector3.up, _currentObjectRotation);
            }
        }

        void HandleNewWallInput() {
            for (int i = 0; i < _placeableObjectPrefabs.Length; i++) {
                if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i)) {
                    if (PressedKeyOfCurrentPrefab(i)) {
                        Destroy(_currentPlaceableObject);
                        _currentPrefabIndex = -1;
                    } else {
                        if (_currentPlaceableObject != null)
                            Destroy(_currentPlaceableObject);
                        _currentPlaceableObject = InstantiateLocalGO(_placeableObjectPrefabs[i]);
                        _currentPrefabIndex = i;
                    }
                    break;
                }
            }
        }

        void HandleNeweCanonInput() {
            if (Input.GetKeyDown(_newCanonHotkey)) {
                if (_currentPlaceableObject == null) {
                    _currentPlaceableObject = InstantiateLocalGO(_canonPrefab);
                } else {
                    Destroy(_currentPlaceableObject);
                }

            }
        }


        bool PressedKeyOfCurrentPrefab(int index) {
            return _currentPlaceableObject != null && _currentPrefabIndex == index;
        }
    }
}
