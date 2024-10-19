using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MoreMountains.TopDownEngine;
using System.Collections;

public class DistanceBasedScreenDarkener : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Image screenOverlay;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private bool isEffectActive = false;
    [SerializeField] private UnityEvent onEndScreenFade;

    private Color originalColor;
    private Transform player;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LevelManager.Current.Players != null);
        player = LevelManager.Current.Players[0].transform;

        if (screenOverlay != null)
        {
            originalColor = screenOverlay.color;
        }
    }

    private void Update()
    {
        if (isEffectActive && target != null && screenOverlay != null)
        {
            float distance = Vector3.Distance(player.position, target.position);
            float alpha = Mathf.Clamp01(distance / maxDistance);
            
            Color overlayColor = originalColor;
            overlayColor.a = alpha;
            screenOverlay.color = overlayColor;

            if (alpha == 1f)
            {
                onEndScreenFade?.Invoke();
                gameObject.SetActive(false);
            }
        }
        else if(screenOverlay != null)
        {
            screenOverlay.color = originalColor;
        }
    }

    public void SetEffectActive(bool active)
    {
        isEffectActive = active;
        if (!active && screenOverlay != null)
        {
            screenOverlay.color = originalColor;
        }
    }
}
