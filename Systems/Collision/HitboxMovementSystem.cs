namespace ECS.Systems;

public class HitboxMovementSystem : AbstractSystem {

    public override void Update(Engine engine, GameTime gametime) {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Collider>(out var collider) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Scale>(out var scale) ||
                !entity.TryGetComponent<SpriteState>(out var sprite))
                continue;

            //Don't adjust hitboxes for dead enemies or enemies whose colliders are off
            if (entity.TryGetComponent<BasicEnemy>(out var enemy) && enemy.DeathType != DeathType.Alive)
                continue;

            if (!collider.IsActive)
                continue;

            //TODO: make hitbox relative to position
            collider.HitBox.X = (int)position.Value.X;
            collider.HitBox.Y = (int)position.Value.Y;

            //These two lines replaced lots of branching for changing mario's and enemies' hitbox sizes, but I imagine this would have some performance costs scaled up.
            collider.HitBox.Width = sprite.SourceRect.Width * (int)scale.Value.X;
            collider.HitBox.Height = sprite.SourceRect.Height * (int)scale.Value.Y;    
        }
    }
}