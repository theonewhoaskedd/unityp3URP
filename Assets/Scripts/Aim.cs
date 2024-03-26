using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using System.IO;

public class Aim : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private List<GameObject> allTarget;
    [SerializeField] private GameObject targetCylinder;
    [SerializeField] private float range;
    private PlayerInput _inputs;
    private PhotonView _pv;
    private CharacterController _controller;
    private GameObject _targetObj;
    private bool _canSearch = true;
    private int _targetCount;

    private void Awake()
    {
        _inputs = new PlayerInput();
        _controller = GetComponent<CharacterController>();
        _pv = GetComponentInParent<PhotonView>();
    }

    private void Start()
    {
        if(!_pv.IsMine) return;
        targetCylinder.SetActive(false);
        _inputs.CharacterControls.Fire.started+=OnFire;
        _inputs.CharacterControls.ChangeTarget.started+=SelectNewTarget;
    }

    private void OnEnable()
    {
        _inputs.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _inputs.CharacterControls.Disable(); 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void SetTargetStatus(bool isTarget)
    {
        targetCylinder.SetActive(isTarget);
    }

    private void SelectTarget()
    {
        if(_controller.velocity == Vector3.zero)
        {
            if(_canSearch)
            {
                InvokeRepeating("Calculate",0f,0.5f);
            }
        }
        else
        {
            if(_targetObj != null)
            {
                _targetObj.GetComponent<Aim>().SetTargetStatus(false);
                _targetObj = null;
            }

            _canSearch=true;
            CancelInvoke();
        }
    }

    private void Calculate()
    {
        _canSearch=false;
        allTarget.Clear();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.position, range);
        foreach(RaycastHit hit in hits)
        {
            GameObject tempObj = hit.collider.gameObject;
            if(tempObj.GetComponent<CharacterController>() && !tempObj.GetComponentInParent<PhotonView>().IsMine)
            {
                allTarget.Add(tempObj);
            }else continue;
        }
        if(allTarget.Count==0) return;
        SelectNewTarget();
    }

    private void SelectNewTarget()
    {
        foreach(GameObject obj in allTarget)
        {
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }
        if(_targetCount>=allTarget.Count)
        {
            _targetCount=0;
        }

        _targetObj = allTarget[_targetCount];
        allTarget[_targetCount].GetComponent<Aim>().SetTargetStatus(true);
    }

    private void SelectNewTarget(InputAction.CallbackContext context)
    {
        _targetCount++;
        foreach(GameObject obj in allTarget)
        {
            obj.GetComponent<Aim>().SetTargetStatus(false);
        }
        if(_targetCount>=allTarget.Count)
        {
            _targetCount=0;
        }

        _targetObj = allTarget[_targetCount];
        allTarget[_targetCount].GetComponent<Aim>().SetTargetStatus(true);
    }

    private void FixedUpdate()
    {
        if(!_pv.IsMine) return;
        SelectTarget();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if(_targetObj != null)
        {
            Vector3 dir = (_targetObj.transform.position - transform.position).normalized;
            GameObject temp = PhotonNetwork.Instantiate(Path.Combine("Fireball"),spawnPosition.position, Quaternion.identity);
            temp.GetComponent<Bullet>().StartMove(dir);
            Physics.IgnoreCollision(temp.GetComponent<Collider>(), transform.GetComponent<Collider>());
        }
    }
}
