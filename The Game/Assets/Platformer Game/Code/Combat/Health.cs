using System;
using UnityEngine;

namespace PlatformerGame
{
    public delegate void ValueChangeEvent<T>(T oldValue, T newValue);

    public sealed class Health : MonoBehaviour, ITeam, Hit.ITarget, ISerializationCallbackReceiver
    {
        public const int DefaultExecutionOrder = -5000;

        [SerializeField]
        private Team _Team;
        public Team Team => _Team;


        [SerializeField]
        private int _MaximumHealth;
        public int MaximumHealth
        {
            get => _MaximumHealth;
            private set
            {
                var oldValue = _MaximumHealth;
                _MaximumHealth = value;
                OnMaximumHealthChanged?.Invoke(oldValue, value);
            }
        }

        public event ValueChangeEvent<int> OnMaximumHealthChanged;

        public enum HealthChangeMode
        {
            Scale,

            Offset,

            Ignore,
        }

        public void SetMaximumHealth(int value, HealthChangeMode mode)
        {
            if (_MaximumHealth == value)
                return;

            switch (mode)
            {
                case HealthChangeMode.Scale:
                    var percentage = _CurrentHealth / _MaximumHealth;
                    MaximumHealth = value;
                    CurrentHealth = value * percentage;
                    break;

                case HealthChangeMode.Offset:
                    var offset = value - _MaximumHealth;
                    MaximumHealth = value;
                    CurrentHealth += offset;
                    break;

                case HealthChangeMode.Ignore:
                    MaximumHealth = value;
                    CurrentHealth = _CurrentHealth;
                    break;

                default:
                    throw Animancer.AnimancerUtilities.CreateUnsupportedArgumentException(mode);
            }
        }

        private int _CurrentHealth;
        public int CurrentHealth
        {
            get => _CurrentHealth;
            set
            {
                var oldValue = _CurrentHealth;
                _CurrentHealth = Mathf.Clamp(value, 0, _MaximumHealth);
                if (_CurrentHealth != oldValue)
                {
                    if (OnCurrentHealthChanged != null)
                        OnCurrentHealthChanged(oldValue, _CurrentHealth);
                    else if (_CurrentHealth <= 0)
                        Destroy(gameObject);
                }
            }
        }

        public event ValueChangeEvent<int> OnCurrentHealthChanged;

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            _MaximumHealth = Math.Max(1, _MaximumHealth);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            CurrentHealth = _MaximumHealth;
        }

#if UNITY_ASSERTIONS
        private void Awake()
        {
            Debug.Assert(_MaximumHealth > 0, $"{nameof(MaximumHealth)} isn't positive.", this);
        }
#endif

        bool Hit.ITarget.CanBeHit(ref Hit hit) =>
            _CurrentHealth > 0 &&
            _Team.IsEnemy(hit.team);

        void Hit.ITarget.ReceiveHit(ref Hit hit)
        {
            CurrentHealth -= hit.damage;
            OnHitReceived?.Invoke(hit);
        }

        public event Action<Hit> OnHitReceived;

    }
}
