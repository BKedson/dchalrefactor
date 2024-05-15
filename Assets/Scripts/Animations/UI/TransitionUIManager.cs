using UnityEditor;
using UnityEngine;

// This is a singleton transition UI manager to create and remove blackscreen as needed
public class TransitionUIManager : MonoBehaviour
{
    // Singleton
    public static TransitionUIManager _instance { get; private set; }

    [SerializeField] private float startTransitionSpan;  // The length of animation to create a balckscreen
    [SerializeField] private float endTransitionSpan;  // The length of animation to remove a balckscreen

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
