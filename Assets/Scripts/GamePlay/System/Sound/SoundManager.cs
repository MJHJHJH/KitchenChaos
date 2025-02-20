using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField] public AudioClipRefsSo audioClipRefsSo;
    // Start is called before the first frame update

    public void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volume);
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }


}
