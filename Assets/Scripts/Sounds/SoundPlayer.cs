using UnityEngine;

namespace Assets.Scripts.Sounds
{
    public static class SoundPlayer
    {
        public static void Play(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, Vector3.zero);
        }
    }
}
