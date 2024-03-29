﻿using Photon.Pun;
using UnityEngine;

namespace Rampart.Remake {

    [RequireComponent(typeof(GridLayout))]
    [RequireComponent(typeof(CannonController))]
    public class GroundPlacementController : MonoBehaviour {

        [SerializeField]
        GameObject[] _placeableObjectPrefabs;

        [SerializeField]
        GameObject _cannonPrefab;

        [SerializeField]
        KeyCode _newCannonHotkey = KeyCode.C;

        [SerializeField]
        KeyCode _rotateHotkey = KeyCode.R;

        [SerializeField]
        LayerMask _environmentLayerMask;

        [SerializeField]
        bool _randomizePieces = true;


        CannonController _cannonController;
        GameObject _currentPlaceableObject;
        int _currentPrefabIndex = -1;
        float _currentObjectRotation = 0;
        GridLayout _gridLayout;
        Vector3 _coordinateOfCell;
        GameManager _gameManager;
        bool _debugDestroyGOInPlaceCannonMode = false;

        void Awake() {
            _gridLayout = GetComponent<GridLayout>();
            _gameManager = GameManager.instance;
            _cannonController = gameObject.GetComponent<CannonController>();
        }

        void Update() {
            if (_gameManager.GetGameMode() == GameMode.BUILD) {
                _debugDestroyGOInPlaceCannonMode = true;
                if (_randomizePieces) {
                    if (_currentPlaceableObject == null) {
                        this.RandomNewWall();
                    }
                } else {
                    this.HandleNewWallInput();
                }
            } else if (_gameManager.GetGameMode() == GameMode.PLACE_CANNON) {
                if (_debugDestroyGOInPlaceCannonMode) {
                    this.DestroyCurrentPlaceableObject();
                    _debugDestroyGOInPlaceCannonMode = false;
                }
                this.NewCannon();
            } else if (_currentPlaceableObject != null){
                this.DestroyCurrentPlaceableObject();
            }

            this.MoveRotatePlaceObject();
        }

        void DestroyCurrentPlaceableObject() {
            if (_currentPlaceableObject != null) {
                Destroy(_currentPlaceableObject);
                _currentPlaceableObject = null;
                _currentPrefabIndex = -1;
            }
        }

        void MoveRotatePlaceObject() {
            if (_currentPlaceableObject != null) {
                this.MoveCurrentPlaceableObjectToMouse();
                this.RotateCurrentPlaceableObject();
                this.PlaceObjectInWorld();
            }
        }

        void PlaceObjectInWorld() {
            if (Input.GetMouseButtonDown(0)) {
                GroundPlaceableObject currentGroundPlaceableObject = _currentPlaceableObject.GetComponent<GroundPlaceableObject>();
                if (currentGroundPlaceableObject != null && currentGroundPlaceableObject.isPlaceable == true) {
                    // destory local GO
                    this.DestroyCurrentPlaceableObject();

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
                placedObject.ChangeObjectColorToLocalPlayerColor();
            }
            return GO;
        }

        void InstantiateNetworkGO(string filename) {
            GameObject GO = PhotonNetwork.Instantiate(filename, _coordinateOfCell, Quaternion.Euler(0, _currentObjectRotation, 0));
            GroundPlaceableObject placedObject = GO.GetComponent<GroundPlaceableObject>();
            if (placedObject != null) {
                placedObject.ChangeObjectColorToPlayerColorOverNetwork();
            }

            if (_gameManager.GetGameMode() == GameMode.PLACE_CANNON) {
                _cannonController.AddCannon(GO.GetComponent<Cannon>());
            }

        }

        void InstantiateObjectFollowMouse(GameObject GO) {
            if (_currentPlaceableObject != null)
                Destroy(_currentPlaceableObject);
            _currentPlaceableObject = InstantiateLocalGO(GO);
        }

        void RotateCurrentPlaceableObject() {
            if (Input.GetKeyDown(_rotateHotkey) || Input.GetMouseButtonDown(1)) {
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

        void RandomNewWall() {
            int prefabIndex = (int) Mathf.Ceil(Random.Range(0f, _placeableObjectPrefabs.Length - 1));
            this.InstantiateObjectFollowMouse(_placeableObjectPrefabs[prefabIndex]);
        }


        void HandleNewWallInput() {
            for (int i = 0; i < _placeableObjectPrefabs.Length; i++) {
                if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i)) {
                    if (PressedKeyOfCurrentPrefab(i)) {
                        Destroy(_currentPlaceableObject);
                        _currentPrefabIndex = -1;
                    } else {
                        _currentPrefabIndex = i;
                        this.InstantiateObjectFollowMouse(_placeableObjectPrefabs[_currentPrefabIndex]);
                    }
                    break;
                }
            }
        }

        void NewCannon() {
            if (_currentPlaceableObject == null) {
                _currentPlaceableObject = InstantiateLocalGO(_cannonPrefab);
            }
        }

        bool PressedKeyOfCurrentPrefab(int index) {
            return _currentPlaceableObject != null && _currentPrefabIndex == index;
        }
    }
}
