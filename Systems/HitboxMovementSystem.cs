namespace ECS.Systems;

public class HitboxMovementSystem : AbstractSystem {

    public override void Update(Engine engine, GameTime gametime) {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Collider>(out var collider) ||
                !entity.TryGetComponent<Position>(out var position))
                continue;

            //TODO: make hitbox relative to position
            collider.HitBox.X = (int)position.Value.X;
            collider.HitBox.Y = (int)position.Value.Y;

            //Change hitbox size for different sized marios
            if (entity.TryGetComponent<Status>(out var status))
            {
                if (status.State == "small")
                {
                    collider.HitBox.Width = 14 * 3; //Times three included for the scale
                    collider.HitBox.Height = 20 * 3;
                }
                else if (status.State == "big" || status.State == "fire")
                {
                    collider.HitBox.Width = 15 * 3;
                    collider.HitBox.Height = 28 * 3;
                }
            }

            //Change hitbox size for different sized koopa / shell
            if (entity.TryGetComponent<BasicEnemy>(out var enemy))
            {
                if (collider.Type == ColliderType.Shell)
                {
                    collider.HitBox.Height = 16 * 3;
                }
                else if (collider.Type == ColliderType.Koopa)
                {
                    collider.HitBox.Height = 27 * 3;
                }
            }
        }
    }
}