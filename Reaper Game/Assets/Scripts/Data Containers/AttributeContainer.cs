using System;

namespace Reaper.Data
{
    public interface AttributeContainer
    {
        public event Action OnAttributesChange;
        public AttributeType GetAttributes<AttributeType>();
    }
}