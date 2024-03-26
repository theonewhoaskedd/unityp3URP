using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Events;

public class GameNetworkManager : MonoBehaviour
{
    public UnityEvent OnGameOver;
    public UnityEvent OnGameWin;
    [SerializeField] private GameObject allPlayerUI;
    private PhotonView _pv;

    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(!_pv.IsMine)
        {
            allPlayerUI.SetActive(false);
            return;
        }
    }

    public void OutOfBattle()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
