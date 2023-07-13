using Animancer.Units;
using System.Collections.Generic;
using UnityEngine;
using static Animancer.Validate;

namespace PlatformerGame.Characters
{
    public sealed class BoxCharacterBody2D : CharacterBody2D
    {
        [SerializeField, Range(0, 90)]
        private float _GripAngle = 49;

        public override float GripAngle
        {
            get => _GripAngle;
            set
            {
                _GripAngle = value;
                _GripProductThreshold = Mathf.Cos(value * Mathf.Deg2Rad);
            }
        }

        private float _GripProductThreshold;

        [SerializeField, Meters]
        private float _StepHeight = 0.55f;

        public override float StepHeight => _StepHeight;

        [SerializeField]
        private bool _VerifyAvailableSpace;

        public ref bool VerifyAvailableSpace => ref _VerifyAvailableSpace;

#if UNITY_EDITOR
        [SerializeField]
        [Seconds(Rule = Value.IsNotNegative)]
        private float _DebugLineDuration;

        public ref float DebugLineDuration => ref _DebugLineDuration;
#endif

        private ContactFilter2D _GroundFilter;

        private void InitializeGroundFilter()
        {
            const float UpAngle = 90;// Would change if it needs to support rotated gravity.
            _GroundFilter = TerrainFilter;
            _GroundFilter.SetNormalAngle(UpAngle - _GripAngle, UpAngle + _GripAngle);
        }

        private float _GravityScale;

        public override Vector2 Gravity => Physics2D.gravity * _GravityScale;

        private PlatformContact2D _GroundContact;

        public override PlatformContact2D GroundContact => _GroundContact;

#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();

            if (Rigidbody != null && enabled && !Rigidbody.freezeRotation)
                Rigidbody.freezeRotation = true;

            if (Collider != null && Collider.bounds.size.y > 0)
                PlatformerUtilities.Clamp(ref _StepHeight, 0, Collider.bounds.size.y * 0.5f);
            else
                PlatformerUtilities.NotNegative(ref _StepHeight);

            PlatformerUtilities.Clamp(ref _GripAngle, 0, 90);

            PlatformerUtilities.NotNegative(ref _DebugLineDuration);

            GripAngle = GripAngle;
            InitializeGroundFilter();
        }
#endif

        protected override void Awake()
        {
            base.Awake();

            GripAngle = GripAngle;
            InitializeGroundFilter();

            OnGroundedChanged += (isGrounded) =>
            {
                if (isGrounded)
                {
                    _GravityScale = Rigidbody.gravityScale;
                    Rigidbody.gravityScale = 0;
                }
                else
                {
                    Rigidbody.gravityScale = _GravityScale;
                    _GroundContact = default;
                }
            };
        }
        private bool _HasUpdatedSinceLastCollision;

        private void FixedUpdate()
        {
            if (_HasUpdatedSinceLastCollision)
            {
                if (Rigidbody.IsAwake())
                    IsGrounded = false;
            }
            else if (IsGrounded && TrySnapToGround())
            {
                var velocity = Velocity;
                velocity.y = 0;
                Velocity = velocity;
                IsGrounded = true;
            }
            else
            {
                _HasUpdatedSinceLastCollision = true;
            }
        }


        private static readonly List<ContactPoint2D> Contacts = new List<ContactPoint2D>(16);

        private void OnCollisionEnter2D(Collision2D collision) => OnCollisionStay2D(collision);

        private void OnCollisionStay2D(Collision2D collision)
        {
            _HasUpdatedSinceLastCollision = false;
            if (IsGrounded ||
                !enabled)
                return;

            var count = collision.GetContacts(Contacts);
            for (int i = 0; i < count; i++)
            {
                var contact = Contacts[i];
                if (contact.normal.y >= _GripProductThreshold)
                {
                    _GroundContact = contact;
                    using (new Context<ContactPoint2D>(contact))
                        IsGrounded = true;

#if UNITY_EDITOR
                    PlatformerUtilities.DrawRay(
                        contact, new Color(0, 1, 0.5f), _DebugLineDuration);
#endif
                    return;
                }
#if UNITY_EDITOR
                else
                {
                    PlatformerUtilities.DrawRay(
                        contact, new Color(0, 0.75f, 0), _DebugLineDuration);
                }
#endif
            }
        }


        private bool TrySnapToGround()
        {
            if (_StepHeight <= Physics2D.defaultContactOffset)
                return false;

            var position = Position;
            var velocity = Velocity;

            var size = Collider.bounds.size;

            var castDistance = _StepHeight * 2;
            var castSize = new Vector2(size.x, size.y - castDistance);

            var origin = position;
            origin.x += velocity.x * Time.deltaTime;
            origin.y += _StepHeight;
            origin.y += castSize.y * 0.5f;


#if UNITY_EDITOR
            PlatformerUtilities.DrawBoxCast(
                origin, castSize, Vector2.down * castDistance, new Color(0, 1, 0), _DebugLineDuration);
#endif

            var count = Physics2D.BoxCast(
                origin, castSize, Rotation, Vector2.down, _GroundFilter, PlatformerUtilities.OneRaycastHit, castDistance);
            if (count == 0)// No Hit.
                return false;

            var hit = PlatformerUtilities.OneRaycastHit[0];

            if (hit.distance == 0)
            {
#if UNITY_EDITOR
                PlatformerUtilities.DrawRay(
                    hit, new Color(0.375f, 0, 0), _DebugLineDuration);
#endif

                return false;
            }

            if (_VerifyAvailableSpace && velocity.x != 0)
            {
                castSize.y = size.y - castSize.y;
                origin.y = hit.point.y + size.y - castSize.y * 0.5f;
                if (Physics2D.OverlapBox(origin, castSize, Rotation, _GroundFilter.layerMask) != null)
                {
#if UNITY_EDITOR
                    PlatformerUtilities.DrawBox(
                        origin, castSize, new Color(0, 0, 0.375f), _DebugLineDuration);
#endif

                    return false;
                }
            }

            _GroundContact = hit;

            var contactPoint = hit.point;
            if (_GroundContact.HasRigidbody)
                contactPoint += _GroundContact.Velocity * Time.deltaTime;

            if (velocity.x != 0 || Mathf.Abs(position.y - contactPoint.y) > Physics2D.defaultContactOffset * 2)
            {
                origin.y = contactPoint.y + Physics2D.defaultContactOffset;

                Position = origin;
                Rigidbody.MovePosition(origin);

#if UNITY_EDITOR
                PlatformerUtilities.DrawRay(
                    hit, new Color(0, 1, 0), _DebugLineDuration);
                PlatformerUtilities.DrawLine(
                    position, origin, new Color(0, 1, 0), _DebugLineDuration);
            }
            else
            {
                PlatformerUtilities.DrawRay(
                    hit, new Color(0.25f, 0.5f, 0.5f), _DebugLineDuration);
#endif
            }

            return true;
        }

    }
}
