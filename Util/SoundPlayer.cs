using Microsoft.Xna.Framework.Audio;

namespace ECS.Util;

public static class SoundPlayer
{
    private static Assets Assets;

    private static Dictionary<string, SoundEffectInstance> PlayingSounds = [];

    private static float SFX_VOLUME = 1.0f;

    public static void RegisterAssets(Assets assets)
    {
        Assets = assets;
    }

    public static void PlaySfx(string key)
    {
        var sound = Assets.GetSound(key).CreateInstance();

        PlayingSounds.TryAdd(key, sound);

        PlayingSounds[key].Volume = SFX_VOLUME;
        PlayingSounds[key].Play();
    }

    public static void PlayMusic(Music song)
    {
        var sound = Assets.GetSound(song.Key).CreateInstance();

        PlayingSounds.TryAdd(song.Key, sound);

        PlayingSounds[song.Key].Volume = song.Volume;
        PlayingSounds[song.Key].IsLooped = true;
        PlayingSounds[song.Key].Play();
    }

    public static void Pause(string key)
    {
        PlayingSounds[key].Pause();
    }

    public static void PauseAll()
    {
        foreach (string key in PlayingSounds.Keys)
            Pause(key);
    }

    public static void Stop(string key)
    {
        if (PlayingSounds.ContainsKey(key)) {
            PlayingSounds[key].Stop();
            PlayingSounds.Remove(key);
        }
    }

    public static void StopAll()
    {
        foreach (string key in PlayingSounds.Keys)
            Stop(key);
    }

    public static void Resume(string key)
    {
        PlayingSounds[key].Resume();
    }
    
    public static void ResumeAll()
    {
        foreach (string key in PlayingSounds.Keys)
            Resume(key);
    }
}