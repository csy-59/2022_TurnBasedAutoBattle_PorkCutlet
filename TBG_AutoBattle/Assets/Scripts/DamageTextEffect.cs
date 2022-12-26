using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextEffect : MonoBehaviour
{
    [SerializeField] private float _effectSpeed;

    [SerializeField] private Vector3 _targetPositionOffset;
    private Vector3 _targetPosition;
    private Vector3 _originalPosition;

    private Text _damageText;
    private Color _textOriginalColor;

    private void Awake()
    {
        _originalPosition = transform.position;

        _damageText = GetComponent<Text>();
        _textOriginalColor = _damageText.color;

        _targetPosition = _originalPosition + _targetPositionOffset;
    }

    public void ShowEffect(float damage)
    {
        StopAllCoroutines();
        _damageText.text = $"-{damage}";
        StartCoroutine(CoStartEffect());
    }

    private IEnumerator CoStartEffect()
    {
        transform.position = _originalPosition;
        _damageText.color = _textOriginalColor;

        float alphaValue = 1f;
        while(true)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _effectSpeed * Time.deltaTime);
            alphaValue = Mathf.Lerp(alphaValue, 0f, _effectSpeed * Time.deltaTime);
            _damageText.color = new Color(_textOriginalColor.r, _textOriginalColor.g, _textOriginalColor.b, alphaValue);

            if(Mathf.Abs(alphaValue) < 0.01f)
            {
                transform.position = _targetPosition;
                alphaValue = 0f;
                _damageText.color = new Color(_textOriginalColor.r, _textOriginalColor.g, _textOriginalColor.b, alphaValue);
                break;
            }

            yield return null;
        }
    }
}
