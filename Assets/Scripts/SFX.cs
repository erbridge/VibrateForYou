using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SFX : MonoBehaviour
{
    public AudioSource OneShotAudioPrefab;

    public static SFX Instance;

    void Awake()
    {
        Instance = this;
    }

    public static AudioSource PlayAt(AudioClip clip, Vector2 position, float volume, bool loop = false)
    {
        var audio = Instantiate(Instance.OneShotAudioPrefab, position, Quaternion.identity) as AudioSource;
        
        audio.clip = clip;
        audio.volume = volume;
        audio.loop = loop;
        audio.Play();
        Destroy(audio.gameObject, clip.length * 2f);
        return audio;
    }

    public static AudioSource PlayAt(AudioClip clip, Vector2 position, float volume, float pitch)
    {
        var audio = Instantiate(Instance.OneShotAudioPrefab, position, Quaternion.identity) as AudioSource;

        audio.clip = clip;
        audio.volume = volume;
        audio.pitch = pitch;

        audio.Play();
        Destroy(audio.gameObject, clip.length * 2f);
        return audio;
    }

    public static AudioSource PlayAt(AudioClip clip, Vector2 position, float volume, float pitch, float delay)
    {
        var audio = Instantiate(Instance.OneShotAudioPrefab, position, Quaternion.identity) as AudioSource;

        audio.clip = clip;
        audio.volume = volume;
        audio.pitch = pitch;

        audio.Play((ulong)(delay * 44100));
        Destroy(audio.gameObject, clip.length * 2f);
        return audio;
    }

    public static AudioSource PlayRandomSound(List<AudioClip> sounds, Vector2 position, float volume, float pitch = 1f)
    {
        AudioClip clip = sounds[Random.Range(0, sounds.Count)];
        return PlayAt(clip, position, volume, pitch);
    }

    public static AudioSource PlayNextRandomSound(List<AudioClip> sounds, ref int lastPlayIndex, Vector2 position, float volume, float pitch = 1f)
    {
        int num = lastPlayIndex;
        var snds = sounds.Where(s => s.name != sounds[num].name).ToArray();
        lastPlayIndex = sounds.IndexOf(snds[Random.Range(0, snds.Length)]);
        AudioClip clip = sounds[lastPlayIndex];

        return PlayAt(clip, position, volume, pitch);
    }
}
