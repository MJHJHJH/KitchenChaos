using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        AllEventHander.onSelectedCounterChangedEvent += Interaction;
    }

    private void OnDestroy()
    {
        AllEventHander.onSelectedCounterChangedEvent -= Interaction;
    }

    protected virtual void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {

    }
}
