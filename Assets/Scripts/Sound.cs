using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioClip put, rotate, select;
    [SerializeField] private AudioSource audioSource;
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(string clip)
    {
        if (MenuManager.Instance.IsSoundOn)
        {
            switch (clip)
            {
                case "select":
                    audioSource.PlayOneShot(select);
                    break;
                case "put":
                    audioSource.PlayOneShot(put);
                    break;
                case "rotate":
                    audioSource.PlayOneShot(rotate);
                    break;
            }
        }
    }
}
