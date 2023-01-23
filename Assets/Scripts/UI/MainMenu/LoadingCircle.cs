using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed;

    private Transform _transform;
    private Vector3 _rotationEulerVector;

    private void Awake()
    {
        _transform = transform;
    }
    private void Update()
    {
        _rotationEulerVector.Set(0f, 0f, _rotationSpeed * Time.deltaTime * -1f);
        _transform.Rotate(_rotationEulerVector);
    }
}
