using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleCell :
    MonoBehaviour,
    IDropHandler
{
    [SerializeField] private Image image;

    public bool IsOccupied { get; private set; }

    private Color emptyColor;
    private Color filledColor =
        new Color(0.75f, 0.45f, 0.2f, 1f);

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        emptyColor = image.color;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (IsOccupied)
        {
            return;
        }

        PuzzlePieceDrag piece =
            eventData.pointerDrag?
                .GetComponent<PuzzlePieceDrag>();

        if (piece == null)
        {
            return;
        }

        Fill();

        piece.gameObject.SetActive(false);
    }

    public void Fill()
    {
        IsOccupied = true;
        image.color = filledColor;
    }

    public void Clear()
    {
        IsOccupied = false;
        image.color = emptyColor;
    }
}