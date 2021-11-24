using UnityEngine;

internal class HighLightSelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField] public Material HighLightMaterial;
    [SerializeField] public Material DefaultMaterial;

    Renderer selectionRenderer;

    [SerializeField] string canAttack_Tag = "canAttack";

    public void OnSelect(Transform selection)
    {
        selectionRenderer = selection.GetComponent<Renderer>();
        DefaultMaterial = selectionRenderer.material;
        if (selectionRenderer != null)
        {
            if (selection.CompareTag(canAttack_Tag))
            {
                selectionRenderer.material = this.HighLightMaterial;
            }
        }
    }
    public void OnDeselect(Transform selection)
    {
        var selectionRenderer = selection.GetComponent<Renderer>();
        if (selection.CompareTag(canAttack_Tag))
        {
            selectionRenderer.material = this.DefaultMaterial;
        }
    }
}