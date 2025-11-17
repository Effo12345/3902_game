namespace ECS.Components;

public class StarSystem : AbstractSystem {


    private bool PlayingStarSong = false;
    public override void Update(Engine engine, GameTime gametime)
    {
        float deltaTime = (float)gametime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status))
                continue;

            if (status.StarTimer > 0)
            {   
                if (!PlayingStarSong)
                {
                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.STAR); //TODO: this should be music, not a sound effect, and it should pause the overworld song when it plays
                }
                status.StarTimer -= deltaTime;
                Console.WriteLine(status.StarTimer);
            } 
            
        }
    }
}