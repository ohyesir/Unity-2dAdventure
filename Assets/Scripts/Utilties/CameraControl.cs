using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;
using System;

public class CameraControl : MonoBehaviour
{
    [Header("事件监听")]
    public VoidSO sceneLoadedDoneSO;
    public VoidSO cameraShakeEvent;

    [Header("控制器")]
    private CinemachineConfiner2D cinemachineConfiner2D;
    public CinemachineImpulseSource cinemachineImpulseSource;
    

    private void Start()
    {
        
    }
    private void Awake() 
    {
        cinemachineConfiner2D = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable() 
    {
        cameraShakeEvent.OnEventRaised += CameraShakeEvent;
        sceneLoadedDoneSO.OnEventRaised  += OnloadedDoneSO;
    }

    private void OnloadedDoneSO()
    {
        GetNewCameraBoundsEdge(); //切换场景后获得场景边界
    }

    private void CameraShakeEvent()
    {
        cinemachineImpulseSource.GenerateImpulse();
    }

    private void OnDisable() 
    {
        cameraShakeEvent.OnEventRaised -= CameraShakeEvent;
        sceneLoadedDoneSO.OnEventRaised -= OnloadedDoneSO;
    }

    private void GetNewCameraBoundsEdge()
    {
        var obj = GameObject.FindGameObjectWithTag("BoundsEdge");

        if(obj == null)
            return;
        
        cinemachineConfiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();//获取相机边界
        cinemachineConfiner2D.InvalidateCache(); //清除上个场景的缓存
    }
}
