using Photon.Pun;
using UnityEngine;

public class WallDestroyController : MonoBehaviourPun {


    public void DestroyWallPiece(int childIndex) {
        Debug.Log("parent");
        DestroyChildGO(childIndex);
        gameObject.GetComponent<PhotonView>().RPC("DestroyWallPieceRPC", RpcTarget.Others, childIndex);
    }

    [PunRPC]
    void DestroyWallPieceRPC(int childIndex) {
        Debug.Log("-RPC-");
        Debug.Log("RPC: child index: " + childIndex.ToString());

        DestroyChildGO(childIndex);
        Debug.Log("RPC: destroyed!");

    }



    void DestroyChildGO(int childIndex) {
        Destroy(gameObject.transform.GetChild(0).GetChild(1).GetChild(childIndex).gameObject);
    }


}
