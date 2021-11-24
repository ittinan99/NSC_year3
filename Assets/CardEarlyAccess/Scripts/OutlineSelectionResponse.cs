using UnityEngine;

internal class OutlineSelectionResponse : MonoBehaviour, ISelectionResponse
{
    [SerializeField] public Color HighLightOutline;

    [SerializeField] string canAttack_Tag;
    [SerializeField] string player_Tag;



    public void OnSelect(Transform selection)
    {
        Debug.Log(selection.name);
        if (selection.CompareTag(canAttack_Tag) || selection.CompareTag(player_Tag))
        {
            var outline = selection.GetComponentInParent<Outline>();
            if (outline != null)
            {
                outline.OutlineColor = HighLightOutline;
                outline.OutlineWidth = 10;
            }
        }
    }
    public void OnDeselect(Transform selection)
    {
        if (selection.CompareTag(canAttack_Tag) || selection.CompareTag(player_Tag))
        {
            var outline = selection.GetComponentInParent<Outline>();
            if (outline != null)
            {
                outline.OutlineWidth = 0;
            }
        }
    }
}
