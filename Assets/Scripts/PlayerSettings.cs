using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon; ///raiseEvent

public class PlayerSettings : MonoBehaviourPunCallbacks
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthBar;
    private PhotonView _pv;
    private GameNetworkManager _gameManager;
    private const byte GAME_IS_WON = 0;

    private void Awake()
    {
        _pv = GetComponentInParent<PhotonView>();
        _gameManager = gameObject.GetComponentInParent<GameNetworkManager>();
    }

    private void Start()
    {
        health = maxHealth;
        healthBar.value = health;
    }

    public void TakeDamage(int value)
    {
        _pv.RPC("UpdateHealth",RpcTarget.All, value);
    }

    [PunRPC]
    public void UpdateHealth(int value)
    {
        health -= value;
        if(health<=0)
        {
            if(!_pv.IsMine) return;
            SendWinEvent();
            _gameManager.OnGameOver.Invoke();
        }
        healthBar.value = health;
    }

    public override void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEventCome;
    }

    public override void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEventCome;
    }

    private void OnNetworkEventCome(EventData obj)
    {
        if(obj.Code == GAME_IS_WON)
        {
            if(!_pv.IsMine)
            {
                return;
            }
            _gameManager.OnGameWin.Invoke();
        }
    }

    private void SendWinEvent()
    {
        object[] datas = null;
        PhotonNetwork.RaiseEvent(GAME_IS_WON, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
}
