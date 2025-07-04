using System;

namespace Reaper.Data
{
    public interface AttributeContainer
    {
        public event Action OnAttributesChange;
        public AttributeType GetAttributes<AttributeType>();
        public void SetAttributes<AttributeType>(AttributeType attributeSet);
    }
}