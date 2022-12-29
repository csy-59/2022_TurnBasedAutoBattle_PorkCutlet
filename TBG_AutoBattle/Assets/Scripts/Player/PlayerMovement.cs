using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    public GameObject OtherPlayer { get; set; }

    private bool _isMoving = false;
    public bool IsMoving 
    { 
        get => _isMoving; 
        set 
        {
            _isMoving = value;
            if (!value)
            {
                OnMovingOver.Invoke();
                OnMovingOver.RemoveAllListeners();
            }
        } 
    }
    public UnityEvent OnMovingOver { get; set; } = new UnityEvent();

    public void MoveToPosition(Vector3 targetPosition, UnityAction afterMoveAction)
    {
        StartCoroutine(CoMoveToPosition(targetPosition, afterMoveAction));
    }

    private IEnumerator CoMoveToPosition(Vector3 targetPosition, UnityAction afterMoveAction)
    {
        while (true)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);

            transform.position = newPosition;

            if ((transform.position - targetPosition).sqrMagnitude < 0.001)
            {
                transform.position = targetPosition;
                break;
            }

            yield return null;
        }

        afterMoveAction.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OtherPlayer = other.gameObject;
        }
    }
}
