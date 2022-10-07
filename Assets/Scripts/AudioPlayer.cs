using System;
using poetools;
using UnityEngine;

namespace DefaultNamespace
{
    public class AudioPlayer : LazySingleton<AudioPlayer>
    {
        private AudioSource _audioSource;
        
        private void Start()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = Resources.Load<AudioClip>("Grapple Song");
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void Play(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}