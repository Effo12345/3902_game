namespace ECS.Systems;

public class BoundedMotionSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Position>(out var pos) ||
                !entity.TryGetComponent<Velocity>(out var vel) ||
                !entity.TryGetComponent<BoundedMotion>(out var motion)
            )
                continue;

            Vector2 nMotionVector = motion.End - motion.Start;
            nMotionVector.Normalize();
            
            Vector2 nPosVector = motion.End - pos.Value;
            nPosVector.Normalize();
            
            // I only had this happen once. No clue why, but this should
            // prevent it from happening again
            if (VectorIsNan(nMotionVector) || VectorIsNan(nPosVector))
                continue;

            if (Math.Sign(nMotionVector.X) == Math.Sign(nPosVector.X) &&
                Math.Sign(nMotionVector.Y) == Math.Sign(nPosVector.Y)
            ) {
                vel.Value = nMotionVector * motion.Velocity;
            }
            else
            {
                // Reverse direction
                (motion.Start, motion.End) = (motion.End, motion.Start);
                vel.Value = nMotionVector * -motion.Velocity;
            }
        }
    }

    bool VectorIsNan(Vector2 vector)
    {
        return float.IsNaN(vector.X) || float.IsNaN(vector.Y);
    }
}