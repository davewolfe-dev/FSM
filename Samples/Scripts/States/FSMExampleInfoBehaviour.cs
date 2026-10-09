using System;
using UnityEngine;
using UnityEngine.UI;

namespace DaveWolfe.FSM.Examples.States
{
    public class FSMExampleInfoBehaviour : MonoBehaviour
    {
        public event Action ShowError;
        
        [SerializeField] private Button errorButton;

        private void Awake()
        {
            errorButton.onClick.AddListener(() => ShowError?.Invoke());
        }
    }
}