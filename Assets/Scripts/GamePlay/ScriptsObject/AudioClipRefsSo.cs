using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipRefsSo", menuName = "ScriptableObject/AudioClipRefsSo")]
public class AudioClipRefsSo : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip stoverSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;
}

