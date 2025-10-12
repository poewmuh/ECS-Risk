using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using TMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Risk.Gameplay.ECS.Components.View
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TimerProvider : MonoProvider<TimerComponent> { }
    
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct TimerComponent : IComponent 
    {
        [SerializeField] private TextMeshProUGUI _timerText;

        public void UpdateTimer(float time)
        {
            var minutes = Mathf.FloorToInt(time / 60F);
            var seconds = Mathf.FloorToInt(time % 60F);

            _timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}