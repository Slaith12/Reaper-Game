namespace Reaper.Data
{
    public interface SoftColliderAttributes
    {
        public float pushForce { get; }
    }

    public sealed class BasicSoftColliderAttributes : SoftColliderAttributes
    {
        public float pushForce { get; set; }

        public BasicSoftColliderAttributes(float pushForce = 20)
        {
            this.pushForce = pushForce;
        }
    }
}