using System.Collections;
using UnityEngine;

public class BorderFx : MonoBehaviour
{
    [SerializeField] private RectTransform border;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float period = 1;
    [SerializeField] private float sizeOffset = 0.35f;

    private void Awake()
    {
        canvasGroup.alpha = 0;
    }

    public void Show(float width, float height, Vector3 pos)
    {
       
        border.sizeDelta = new Vector2(width + sizeOffset, height + sizeOffset);
        border.position = pos;
        
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        float t = 1;
        while (t > 0)
        {
            canvasGroup.alpha = t;
            t -= Time.deltaTime / period;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}