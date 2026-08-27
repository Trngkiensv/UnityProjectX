using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PuzzlePieceDrag :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerClickHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private PuzzlePiece piece;

    private bool isDragging;

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

    private void Update()
    {
        if (
            !isDragging ||
            Keyboard.current == null)
        {
            return;
        }

        if (
            Keyboard.current
                .rKey
                .wasPressedThisFrame)
        {
            piece.RotateClockwise();
        }
    }

    public void OnBeginDrag(
        PointerEventData eventData)
    {
        isDragging = true;

        piece.BeginDrag();

        canvasGroup.blocksRaycasts =
            false;

        canvasGroup.alpha = 0.9f;
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
        isDragging = false;

        canvasGroup.blocksRaycasts =
            true;

        canvasGroup.alpha = 1f;

        if (!piece.DropSucceededThisDrag)
        {
            piece.RestoreAfterFailedDrag();
        }
    }

    public void OnPointerClick(
        PointerEventData eventData)
    {
        if (isDragging)
        {
            return;
        }

        if (
            eventData.button ==
            PointerEventData.InputButton.Right)
        {
            piece.RotateClockwise();
        }
    }
}