using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SilhouetteBehaviour : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _silhouetteMaterial;

    [Header("Silhouette Settings")]
    [Tooltip("Sorting layer del silhouette renderer. Ha d'estar PER SOBRE del tilemap.")]
    [SerializeField] private string _silhouetteSortingLayer = "Default";
    [SerializeField] private int _silhouetteSortingOrder = 10;

    private SpriteRenderer _mainRenderer;
    private SpriteRenderer _silhouetteRenderer;
    private GameObject _silhouetteGO;

    private void Awake()
    {
        _mainRenderer = GetComponent<SpriteRenderer>();
        CreateSilhouetteRenderer();
    }

    private void CreateSilhouetteRenderer()
    {
        _silhouetteGO = new GameObject("Silhouette_XRay");

        _silhouetteGO.transform.SetParent(transform);
        _silhouetteGO.transform.localPosition = Vector3.zero;
        _silhouetteGO.transform.localScale = Vector3.one;
        _silhouetteGO.transform.localRotation = Quaternion.identity;

        _silhouetteRenderer = _silhouetteGO.AddComponent<SpriteRenderer>();
        _silhouetteRenderer.material = _silhouetteMaterial;
        _silhouetteRenderer.sortingLayerName = _silhouetteSortingLayer;
        _silhouetteRenderer.sortingOrder = _silhouetteSortingOrder;
    }

    private void LateUpdate()
    {
        _silhouetteRenderer.sprite = _mainRenderer.sprite;
        _silhouetteRenderer.flipX = _mainRenderer.flipX;
        _silhouetteRenderer.flipY = _mainRenderer.flipY;
    }

    private void OnDestroy()
    {
        if (_silhouetteGO != null)
            Destroy(_silhouetteGO);
    }
}