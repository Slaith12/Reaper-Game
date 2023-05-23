using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reaper.Data
{
    public sealed class BasicAttributes : MonoBehaviour, AttributeContainer
    {
        private static readonly IDictionary<Type, object> defaultAttributes;

        public static AttributeType GetDefaultAttributes<AttributeType>()
        {
            Type type = typeof(AttributeType);
            if(!defaultAttributes.ContainsKey(type))
            {
                Debug.LogError($"Attributes of type {type.Name} have no indicated default. " +
                    $"That type either isn't an attribute type or isn't properly added to BasicAttributes.cs");
                return default;
            }
            return (AttributeType)defaultAttributes[type];
        }

        public event Action OnAttributesChange;
        private IDictionary<Type, object> attributes;

        public void Awake()
        {
            attributes = new Dictionary<Type, object>();
        }

        public AttributeType GetAttributes<AttributeType>()
        {
            Type type = typeof(AttributeType);
            if (!attributes.ContainsKey(type))
            {
                Debug.LogWarning($"Attributes of type {type.Name} not found. Using default attributes.");
                return GetDefaultAttributes<AttributeType>();
            }
            return (AttributeType)attributes[type];
        }

        public void SetAttributes<AttributeType>(AttributeType attributeSet)
        {
            foreach(Type type in defaultAttributes.Keys)
            {
                if(type.IsAssignableFrom(typeof(AttributeType)))
                {
                    attributes[type] = attributeSet;
                }
            }
            OnAttributesChange?.Invoke();
        }

        public void AddAttributes<AttributeType>(AttributeType attributes)
        {
            this.attributes[typeof(AttributeType)] = attributes;
            OnAttributesChange?.Invoke();
        }

        static BasicAttributes()
        {
            defaultAttributes = new Dictionary<Type, object>
            {
                { typeof(MoverAttributes), new BasicMoverAttributes() },
                { typeof(SoftColliderAttributes), new BasicSoftColliderAttributes() }
            };
        }
    }
}