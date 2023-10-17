using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Action2D
{
    public static IEnumerator MoveTo(Transform target, Vector3 to, float duration, bool bSelfRemove = false)
    {
        Vector2 startPos = target.transform.position;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.smoothDeltaTime;
            target.transform.position = Vector2.Lerp(startPos, to, elapsed / duration);

            yield return null;
        }

        target.transform.position = to;

        if (bSelfRemove)
            Object.Destroy(target.gameObject, 0.1f);

        yield break;
    }

    public static IEnumerator Scale(Transform target, float toScale, float speed)  //수정해서 커지는 것도 만들기
    {
        bool bInc = target.localScale.x < toScale;
        float fDir = bInc ? 1 : -1;

        float factor;
        while (true)
        {
            factor = Time.deltaTime * speed * fDir;
            target.localScale = new Vector3(target.localScale.x + factor, target.localScale.y + factor, target.localScale.z);

            if ((!bInc && target.localScale.x <= toScale) || (bInc && target.localScale.x >= toScale))
                break;

            yield return null;
        }

        yield break;
    }

}
