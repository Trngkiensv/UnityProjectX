using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzlePieceDrag :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private PuzzlePiece piece;

    private Vector2 startPosition;

    private void Awake()
    {
        rectTransform =
            GetComponent<RectTransform>();

        canvas =
            GetComponentInParent<Canvas>();

        piece =
            GetComponent<PuzzlePiece>();

        canvasGroup =
            GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup =
                gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(
        PointerEventData eventData)
    {
        startPosition =
            rectTransform.anchoredPosition;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(
        PointerEventData eventData)
    {
        rectTransform.anchoredPosition +=
            eventData.delta /
            canvas.scaleFactor;
    }

    public void OnEndDrag(
        PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (piece != null &&
            piece.IsPlaced)
        {
            gameObject.SetActive(false);
            return;
        }

        rectTransform.anchoredPosition =
            startPosition;
    }
}