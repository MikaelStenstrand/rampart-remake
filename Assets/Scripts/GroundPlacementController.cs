using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(GridLayout))]
public class GroundPlacementController : MonoBehaviour {

    [SerializeField]
    GameObject _placeableObjectPrefab;

    [SerializeField]
    string _placeableObjectFilename;

    [SerializeField]
    KeyCode _newObjectHotkey = KeyCode.N;

    [SerializeField]
    KeyCode _rotateHotkey = KeyCode.R;

    [SerializeField]
    LayerMask _environmentLayerMask;

    [SerializeField]
    PlayerSettings _playerSettings;

    const string _prefabFolderPath = "Prefabs/GroundPlaceableObjects/";
    GameObject _currentPlaceableObject;
    float _currentObjectRotation = 0;
    GridLayout _gridLayout;
    Vector3 _coordinateOfCell;

    void Awake() {
        _gridLayout = GetComponent<GridLayout>();
    }

    void Update() {
        HandleNewObjectHotkey();
        if (_currentPlaceableObject != null) {
            MoveCurrentPlaceableObjectToMouse();
            RotateCurrentPlaceableObject();
            PlaceObjectInWorld();
        }
    }

    void PlaceObjectInWorld() {
        if (Input.GetMouseButtonDown(0)) {
            // destory local GO
            Destroy(_currentPlaceableObject);
            _currentPlaceableObject = null;

            // Instantiate network GO
            GameObject placedGO = this.InstantiateNetworkGO();

            _currentObjectRotation = 0;
        }
    }

    string GetPathOfObject() {
        return _prefabFolderPath + _placeableObjectFilename;
    }

    /*
     * Expects the Game Object to have the following sturcutre
     * GAME OBJECT
     * - objects
     * -- ColorModifiedObjects (objects' color within this group will be changed)
     * -- etc.
     * -- etc.
     */
    void ChangeObjectColorToPlayerColor(GameObject GO) {
        Component[] renderers = GO.transform.GetChild(0).GetChild(0).gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers) {
            Color color = (Color)_playerSettings.GetLocalPlayerColor();
            renderer.material.color = color;
        }
    }

    GameObject InstantiateLocalGO() {
        GameObject GO = Instantiate(_placeableObjectPrefab);
        this.ChangeObjectColorToPlayerColor(GO);
        return GO;
    }

    GameObject InstantiateNetworkGO() {
        GameObject GO = PhotonNetwork.Instantiate(this.GetPathOfObject(), _coordinateOfCell, Quaternion.Euler(0, _currentObjectRotation, 0));
        this.ChangeObjectColorToPlayerColor(GO);
        return GO;
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

    void HandleNewObjectHotkey() {
        if (Input.GetKeyDown(_newObjectHotkey))  {
            if (_currentPlaceableObject == null) {
                _currentPlaceableObject = InstantiateLocalGO();
            } else {
                Destroy(_currentPlaceableObject);
            }
        }
    }
}
