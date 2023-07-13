using Animancer;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformerGame
{
    public sealed class HealthDisplay : MonoBehaviour
    {
        [SerializeField] private Health _Health;
        [SerializeField] private Text _Text;

#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Text);
        }
#endif

        private void Awake()
        {
            _Health.OnCurrentHealthChanged += (oldValue, newValue) => OnHealthChanged();
            _Health.OnMaximumHealthChanged += (oldValue, newValue) => OnHealthChanged();
            OnHealthChanged();
        }

        private void OnHealthChanged()
        {
            _Text.text = $"Health: {_Health.CurrentHealth} / {_Health.MaximumHealth}";
        }

    }
}
