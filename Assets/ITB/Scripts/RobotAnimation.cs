using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(scaleAnimation());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator scaleAnimation()
    {
        transform.DOScale(transform.localScale * 1.2f, 1.2f).SetEase(Ease.InOutQuart).SetLoops(-1, LoopType.Yoyo);
        transform.DOMoveY(transform.position.y + 0.0001f, 1).SetLoops(-1, LoopType.Yoyo);
        yield return null;
    }
}
