using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode
    {
        LookAt,
        LookAtInverted,
        CameraForWard,
        CameraForWardInverted,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                transform.LookAt(transform.position - Camera.main.transform.position);
                break;
            case Mode.CameraForWard:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForWardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
