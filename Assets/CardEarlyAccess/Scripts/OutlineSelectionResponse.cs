using UnityEngine;

internal class OutlineSelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField] public Color HighLightOutline;

    [SerializeField] string canAttack_Tag = "canAttack";

    public void OnSelect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            if (selection.CompareTag(canAttack_Tag))
            {
                outline.OutlineColor = HighLightOutline;
                outline.OutlineWidth = 10;
            }
        }
    }
    public void OnDeselect(Transform selection)
    {
        var outline = selection.GetComponent<Outline>();
        if (outline != null)
        {
            if (selection.CompareTag(canAttack_Tag))
            {
                outline.OutlineWidth = 0;
            }
        }    
    }
}
