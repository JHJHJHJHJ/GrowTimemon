using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwipeMenu : MonoBehaviour
{
    [SerializeField] GameObject scrollbar = null;
    [SerializeField] TextMeshProUGUI indexText = null;

    float scrollPos = 0f;
    float[] poses;
    float distance = 0f;

    int currentIndex = 1;
    int maxIndex = 0;

    bool canWork = true;

    void Update()
    {
        poses = new float[transform.childCount];

        if (transform.childCount > 1) distance = 1f / (poses.Length - 1f);

        for (int i = 0; i < poses.Length; i++)
        {
            poses[i] = distance * i;
        }

        UpdateIndex();
        AnchorAndResize();
    }

    void UpdateIndex()
    {
        if (canWork)
        {
            for (int i = 0; i < poses.Length; i++)
            {
                if (scrollPos < poses[i] + (distance / 2) && scrollPos > poses[i] - (distance / 2))
                {
                    currentIndex = i + 1;
                }
            }
        }

            indexText.text = currentIndex + "/" + poses.Length;
    }

    void AnchorAndResize()
    {
        Anchor();
        Resize();
    }

    void Anchor()
    {
        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollbar.GetComponent<Scrollbar>().value;
            canWork = true;
        }
        else if (canWork)
        {
            for (int i = 0; i < poses.Length; i++)
            {
                if (scrollPos < poses[i] + (distance / 2) && scrollPos > poses[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, poses[i], 0.1f);
                }
            }
        }
    }

    void Resize()
    {
        if (canWork)
        {
            for (int i = 0; i < poses.Length; i++)
            {
                if (scrollPos < poses[i] + (distance / 2) && scrollPos > poses[i] - (distance / 2))
                {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                    for (int a = 0; a < poses.Length; a++)
                    {
                        if (a != i)
                        {
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                    }
                }
            }
        }
    }

    public void DoNotScroll()
    {
        canWork = false;
    }
}
