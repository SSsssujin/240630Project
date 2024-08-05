using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace INeverFall
{
    public class TextController : MonoBehaviour
    {
        private const float _letterPause = 0.05f;

        private Text _text;
        private MeshRenderer _renderer;
        private WaitForSeconds _letterDelay;
        private Collider _collider;

        [SerializeField] private bool _show;
        [SerializeField] private List<string> _textList;

        private void Start()
        {
#if !UNITY_EDITOR
        _show = false;
#endif
            TryGetComponent(out _renderer);
            TryGetComponent(out _collider);
            _renderer.enabled = _show;
            _text = GameManager.Instance.LowerText;
            _letterDelay = new WaitForSeconds(_letterPause);
        }

        private void OnTriggerEnter(Collider other)
        {
            _TriggerExecuted();
        }

        protected virtual void _TriggerExecuted()
        {
            _collider.enabled = false;
            StartCoroutine(_cTypeText());
        }

        private IEnumerator _cTypeText()
        {
            // Initialize text
            _ClearText();
            _text.gameObject.SetActive(true);

            // Start to print
            for (int textCount = 0; textCount < _textList.Count; textCount++)
            {
                string word = _textList[textCount];
                
                for (int letter = 0; letter < word.Length; letter++)
                {
                    yield return _letterDelay;
                    
                    if (string.Equals(word[letter], 'e'))
                    {
                        _text.text += "\n";
                        continue;
                    }

                    _text.text += word[letter];
                }

                yield return new WaitForSeconds(2.5f);
                _ClearText();
            }
            _text.gameObject.SetActive(false);
            _collider.enabled = true;
        }

        private void _ClearText()
        {
            _text.text = string.Empty;
        }
    }
}