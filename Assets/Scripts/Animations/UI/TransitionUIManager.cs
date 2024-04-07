using UnityEditor.Animations;
using UnityEngine;

public class TransitionUIManager : MonoBehaviour
{
    public static TransitionUIManager _instance { get; private set; }

    [SerializeField] private float startTransitionSpan;
    [SerializeField] private float endTransitionSpan;

    private Animator animator;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        animator = GetComponent<Animator>();
    }

    public void StartTransition()
    {
        animator.SetTrigger("Transition Start");
    }

    public void EndTransition()
    {
        animator.SetTrigger("Transition End");
    }

    public float GetStartTransitionSpan()
    {
        return startTransitionSpan;
    }

    public float GetEndTransitionSpan()
    {
        return endTransitionSpan;
    }
}
