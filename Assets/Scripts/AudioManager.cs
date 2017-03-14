using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioSource _oneShotAudioSourcePrefab;

    private static AudioManager _instance;

    private static Dictionary<string, AudioClip> _audioClips;

    void Awake() {
        AudioManager._instance = this;

        AudioManager._audioClips = new Dictionary<string, AudioClip>();

        AudioManager._audioClips.Add(
            "Keyboard_Click_01",
            Resources.Load("SFX/Keyboard_Click_01") as AudioClip
        );
        AudioManager._audioClips.Add(
            "Keyboard_Click_02",
            Resources.Load("SFX/Keyboard_Click_02") as AudioClip
        );
        AudioManager._audioClips.Add(
            "Keyboard_Click_03",
            Resources.Load("SFX/Keyboard_Click_03") as AudioClip
        );
        AudioManager._audioClips.Add(
            "Message_Recieve",
            Resources.Load("SFX/Message_Recieve") as AudioClip
        );
        AudioManager._audioClips.Add(
            "Message_Send",
            Resources.Load("SFX/Message_Send") as AudioClip
        );
        AudioManager._audioClips.Add(
            "Typing",
            Resources.Load("SFX/Typing") as AudioClip
        );
        AudioManager._audioClips.Add(
            "Vibrate",
            Resources.Load("SFX/Vibrate") as AudioClip
        );
    }

    public static void PlayOneShot(string key, float volume = 1f) {
        if (!AudioManager._audioClips.ContainsKey(key)) {
            Debug.LogError("AudioClip with key " + key + " not found");

            return;
        }

        AudioClip clip = AudioManager._audioClips[key];

        AudioSource source = Instantiate(
            AudioManager._instance._oneShotAudioSourcePrefab,
            Camera.main.transform.position,
            Quaternion.identity
        ) as AudioSource;

        source.clip   = clip;
        source.volume = volume;
        source.loop   = false;

        source.Play();

        Destroy(source.gameObject, clip.length);
    }

}
