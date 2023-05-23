using Reaper.Data;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SoftCollider : MonoBehaviour
{
    public bool spawnEnviroCollider = true;
    private new Rigidbody2D rigidbody;
    private SoftColliderAttributes attributes;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (spawnEnviroCollider)
        {
            AddEnviroCollider();
        }
        AttributeContainer container = GetComponent<AttributeContainer>();
        GetAttributes(container);
        container.OnAttributesChange += delegate { GetAttributes(container); };
    }

    private void GetAttributes(AttributeContainer container)
    {
        attributes = container.GetAttributes<SoftColliderAttributes>();
        if (attributes == null)
        {
            Debug.LogError($"Object {gameObject.name}'s attributes do not include attributes for the soft collider component. Using default attributes.");
            attributes = BasicAttributes.GetDefaultAttributes<SoftColliderAttributes>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<SoftCollider>() && rigidbody != null) //if the other object also has a soft collider
        {
            rigidbody.AddForce((transform.position - collision.transform.position).normalized * attributes.pushForce);
        }
    }

    private void AddEnviroCollider()
    {
        Transform enviroObject = new GameObject("Environment Collision").transform;
        enviroObject.gameObject.layer = 9;
        enviroObject.SetParent(transform);
        enviroObject.position = transform.position;
    }

}
