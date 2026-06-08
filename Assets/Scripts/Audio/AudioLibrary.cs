using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/AudioLibrary")]
public class AudioLibrary : ScriptableObject
{
    [System.Serializable]
    public struct SFXEntry
    {
        public SoundEnum sound;
        public SoundSO clip;
    }

    [System.Serializable]
    public struct MusicEntry
    {
        public MusicType sound;
        public List<SoundSO> clips;
    }

    [Header("Sound Library")]
    public SFXEntry[] sounds;
    [Header("Music Library")]
    public MusicEntry[] musics;

    private Dictionary<SoundEnum, SoundSO> sfxLookup;
    private Dictionary<MusicType, List<SoundSO>> musicLookup;

    public void Init()
    {
        sfxLookup = new Dictionary<SoundEnum, SoundSO>();
        musicLookup = new Dictionary<MusicType, List<SoundSO>>();

        foreach (var entry in sounds)
            sfxLookup[entry.sound] = entry.clip;

        foreach(var entry in musics)
        {
            if (musicLookup.ContainsKey(entry.sound))
            {
                Debug.LogWarning($"Duplicate music type: {entry.sound}");
            }
            else { musicLookup.Add(entry.sound, entry.clips); }
        }
    }

    public SoundSO GetClip(SoundEnum sound)
    {
        return sfxLookup.TryGetValue(sound, out var clip) ? clip : null;
    }

    public SoundSO GetMusic(MusicType type)
    {
        if(musicLookup.TryGetValue(type, out var clips))
        {
            if (clips.Count == 0) return null;
            return clips[Random.Range(0, clips.Count)];
        }

        return null;
    }
}
