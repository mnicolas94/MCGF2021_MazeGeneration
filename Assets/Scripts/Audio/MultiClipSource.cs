using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    public class MultiClipSource : MonoBehaviour
    {
        public AudioSource source;
        public List<AudioClip> clips;

        public void PlayRandomClip()
        {
            var clip = GetRandomClip();
            source.PlayOneShot(clip);
        }

        private AudioClip GetRandomClip()
        {
            int randIndex = Random.Range(0, clips.Count);
            return clips[randIndex];
        }
    }
}
