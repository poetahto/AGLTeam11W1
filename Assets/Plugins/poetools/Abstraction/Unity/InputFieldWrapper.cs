using System;
using UnityEngine;

namespace poetools.Abstraction.Unity
{
    [RequireComponent(typeof(UnityEngine.UI.InputField))]
    public class InputFieldWrapper : InputFieldDisplay
    {
        public override event Action<string> OnSubmit;
        public override event Action<string, string> OnValueChange;
        
        private UnityEngine.UI.InputField _inputField;
        private string _previousValue = "";
        
        private void Awake()
        {
            _inputField = GetComponent<UnityEngine.UI.InputField>();
        }

        private void OnEnable()
        {
            _inputField.onEndEdit.AddListener(SendSubmitEvent);
            _inputField.onValueChanged.AddListener(SendValueChangedEvent);
        }

        private void OnDisable()
        {
            _inputField.onEndEdit.RemoveListener(SendSubmitEvent);
        }

        private void SendValueChangedEvent(string newValue)
        {
            OnValueChange?.Invoke(_previousValue, newValue);
            _previousValue = newValue;
        }
        
        private void SendSubmitEvent(string submittedValue)
        {
            OnSubmit?.Invoke(submittedValue);
        }

        public override void Focus()
        {
            _inputField.ActivateInputField();
        }

        public override string GetText()
        {
            return _inputField.text;
        }

        public override void SetText(string value)
        {
            _inputField.text = value;
            _inputField.caretPosition = value.Length;
        }

    }
}