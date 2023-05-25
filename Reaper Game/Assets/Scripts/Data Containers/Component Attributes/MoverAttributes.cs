namespace Reaper.Data
{
    public interface MoverAttributes
    {
        public float acceleration { get; }
        public float friction { get; }
        public float knockbackMult { get; }
        public bool partialKnockback { get; }
    }

    public sealed class BasicMoverAttributes : MoverAttributes
    {
        public float acceleration { get; set; }
        public float friction { get; set; }
        public float knockbackMult { get; set; }
        public bool partialKnockback { get; set; }

        public BasicMoverAttributes(float acceleration = 60, float friction = 30, float knockbackMult = 0, bool partialKnockback = false)
        {
            this.acceleration = acceleration;
            this.friction = friction;
            this.knockbackMult = knockbackMult;
            this.partialKnockback = partialKnockback;
        }
    }
}