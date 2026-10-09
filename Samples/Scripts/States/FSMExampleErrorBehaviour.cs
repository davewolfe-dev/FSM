using System;
using UnityEngine;
using UnityEngine.UI;

namespace DaveWolfe.FSM.Examples.States
{
    public class FSMExampleErrorBehaviour : MonoBehaviour
    {
        public event Action BackToWelcome;
        
        [SerializeField] private Button backToWelcomeButton;

        private void Awake()
        {
            backToWelcomeButton.onClick.AddListener(() => BackToWelcome?.Invoke());
        }
    }
}