using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileClickAudioController : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] notFixedClip;
    [SerializeField] private AudioClip[] fixedClip;

    // Start is called before the first frame update
    void Start()
    {
        TileBaseController.OnTileClick += PlaySounds;
    }

    public void PlaySounds(bool isFixed)
    {
        if(isFixed)
            audioSource.PlayOneShot(fixedClip[Random.Range(0, fixedClip.Length)]);
        else
            audioSource.PlayOneShot(notFixedClip[Random.Range(0, notFixedClip.Length)]);
    }

    private void OnDestroy()
    {
        TileBaseController.OnTileClick -= PlaySounds;
    }
}
