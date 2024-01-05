using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        var state  = _animator.GetCurrentAnimatorStateInfo(0);
        _animator.Play(state.fullPathHash, 0, Random.Range(0f, 1f));

    }

}
