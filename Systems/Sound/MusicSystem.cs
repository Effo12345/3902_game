namespace ECS.Systems;

public class MusicSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        HashSet<string> musicThisUpdate = [];

        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Music>(out var music))
                continue;

            if (!music.Playing)
            {
                SoundPlayer.PlayMusic(music);
                music.Playing = true;
            }
        }
    }
}