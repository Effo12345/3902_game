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

            if (entity.TryGetComponent<Status>(out var status))
            {
                if (status.State == "small")
                {
                    collider.HitBox.Width = 14 * 3; //Times three included for the scale
                    collider.HitBox.Height = 20 * 3;
                }
                else if (status.State == "big" || status.State == "fire")
                {
                    Console.WriteLine("working");
                    collider.HitBox.Width = 20 * 3;
                    collider.HitBox.Height = 32 * 3;
                }
            }
        }
    }
}