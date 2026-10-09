using System;
using UnityEngine;
using UnityEngine.UI;

namespace DaveWolfe.FSM.Examples.States
{
    public class FSMExampleWelcomeBehaviour : MonoBehaviour
    {
        public event Action ShowInfo;
        
        [SerializeField] private Button infoButton;

        private void Awake()
        {
            infoButton.onClick.AddListener(() => ShowInfo?.Invoke());
        }
    }
}