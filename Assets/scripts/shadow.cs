using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class Shadow : MonoBehaviour
{
    public Color shadowColor;
    public float Alpha = 85;
    public Vector3 offset = new Vector3(1, -0.5f, 0);
    public Vector3 shadowScale = new Vector3(1, 1, 1);

    [SerializeField] private GameObject shadowObject;
    [SerializeField] private SpriteRenderer shadowRenderer;

    private void Start()
    {
        if (shadowObject == null)
        {
            CreateShadow();
        }
    }

    private void Update()
    {
        UpdateShadow();
        RemoveUnusedShadowChildren();
    }

    private void CreateShadow()
    {
        shadowObject = new GameObject("Shadow");
        shadowObject.transform.parent = transform;
        shadowObject.transform.localPosition = offset;
        shadowObject.transform.localScale = shadowScale;
        shadowObject.transform.localRotation = new Quaternion(0,0,0,0);

        shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();
        shadowRenderer.sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
        shadowRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
        shadowRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        shadowColor.a = Alpha / 255;
        shadowRenderer.color = shadowColor;

        shadowObject.tag = "shadow"; // Set the tag to "shadow"
    }

    private void UpdateShadow()
    {
        if (shadowObject == null)
        {
            CreateShadow();
        }

        shadowObject.transform.position = transform.position + offset;
        shadowObject.transform.localScale = shadowScale;
    }

    private void RemoveUnusedShadowChildren()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "shadow" && child.gameObject != shadowObject)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
