using Animancer;
using PlatformerGame.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace PlatformerGame
{
    public sealed class HitTrigger : MonoBehaviour
    {
        #region Prefab
        private static HitTrigger _Prefab;
        private static bool _HasLoadedPrefab;

        private static HitTrigger Prefab
        {
            get
            {
                if (!_HasLoadedPrefab)
                {
                    _HasLoadedPrefab = true;
                    _Prefab = Resources.Load<HitTrigger>(nameof(HitTrigger));
                    AnimancerUtilities.Assert(_Prefab != null,
                        $"There is no '{nameof(HitTrigger)}.prefab' asset (with that exact name) in the root of a Resources folder");
                }

                return _Prefab;
            }
        }


        private static int _HitLayers;

        public static int HitLayers
        {
            get
            {
                if (_HitLayers == 0)
                    _HitLayers = Physics2D.GetLayerCollisionMask(Prefab.gameObject.layer);
                return _HitLayers;
            }
        }

        #endregion
        #region Object Pool

        private static readonly List<HitTrigger>
            SpareInstances = new List<HitTrigger>();


        public static HitTrigger Activate(Character character, HitData data, bool flipX, HashSet<Hit.ITarget> ignore)
        {
            AnimancerUtilities.Assert(character != null,
                $"{nameof(Characters.Character)} is null.");
            AnimancerUtilities.Assert(data != null,
                $"{nameof(HitData)} is null.");
            AnimancerUtilities.Assert(data.Area != null,
                $"{nameof(HitData)}.{nameof(HitData.Area)} is null.");

            var instance = GetInstance();

            instance.Parent = character.Animancer.transform;
            instance.Character = character;
            instance.Data = data;
            instance.FlipX = flipX;
            instance.Ignore = ignore;

            var area = data.Area;
            if (!flipX)
            {
                instance.Collider.points = area;
            }
            else
            {
                var points = ObjectPool.AcquireList<Vector2>();

                var count = area.Length;
                for (int i = 0; i < count; i++)
                {
                    var point = area[i];
                    point.x = -point.x;
                    points.Add(point);
                }

                instance.Collider.SetPath(0, points);
                ObjectPool.Release(points);
            }

#if UNITY_EDITOR
            instance.name = $"{character.name}: D{data.Damage} F{data.Force}";
#endif

            instance.Transform.SetPositionAndRotation(instance.Parent.position, instance.Parent.rotation);
            return instance;
        }


        private static HitTrigger GetInstance()
        {
            var count = SpareInstances.Count;
            if (count == 0)
            {
                return CreateInstance();
            }
            else
            {
                count--;
                var instance = SpareInstances[count];
                SpareInstances.RemoveAt(count);
                instance.gameObject.SetActive(true);
                return instance;
            }
        }


        private static Transform _Group;

        private static HitTrigger CreateInstance()
        {
            if (_Group == null)
            {
                var gameObject = new GameObject("Hit Triggers");
                DontDestroyOnLoad(gameObject);
                _Group = gameObject.transform;
            }

            var instance = Instantiate(Prefab, _Group);
            instance.Transform = instance.transform;
            return instance;
        }


        public static void PreAllocate(int capacity)
        {
            while (SpareInstances.Count < capacity)
                SpareInstances.Add(CreateInstance());
        }


        public void Deactivate()
        {
            if (this == null || !gameObject.activeSelf)
                return;

            gameObject.SetActive(false);

            Parent = null;
            Character = null;
            Data = null;
            Ignore = null;

            SpareInstances.Add(this);
        }

        #endregion

        [SerializeField]
        private PolygonCollider2D _Collider;
        public PolygonCollider2D Collider => _Collider;


#if UNITY_EDITOR
        private void OnValidate()
        {
            gameObject.GetComponentInParentOrChildren(ref _Collider);
        }
#endif


        public Transform Transform { get; private set; }

        public Transform Parent { get; private set; }
        public Character Character { get; private set; }
        public bool FlipX { get; private set; }
        public HitData Data { get; private set; }
        public HashSet<Hit.ITarget> Ignore { get; private set; }


        private void FixedUpdate()
        {
            if (Parent == null)
            {
                Deactivate();
                return;
            }

            Transform.SetPositionAndRotation(Parent.position, Parent.rotation);
        }


        private void OnTriggerEnter2D(Collider2D collider)
            => OnTriggerStay2D(collider);

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (Data == null)
                return;

            AnimancerUtilities.Assert(Character != null,
                $"{nameof(Characters.Character)} has been destroyed but didn't release its {nameof(HitTrigger)}.");
            AnimancerUtilities.Assert(Character.gameObject.activeInHierarchy,
                $"{nameof(Characters.Character)} is inactive but didn't release its {nameof(HitTrigger)}.");

            var direction = HitData.AngleToDirection(Data.Angle, Transform.rotation, FlipX);

            var hit = new Hit(Character.transform, Character.Health.Team, Data.Damage, Data.Force, direction, Ignore);
            hit.TryHitComponent(collider);
        }

#if UNITY_EDITOR

        [UnityEditor.CustomEditor(typeof(HitTrigger), true)]
        public class Editor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (!UnityEditor.EditorApplication.isPlaying)
                    return;

                using (new UnityEditor.EditorGUI.DisabledScope(true))
                {
                    var target = (HitTrigger)this.target;
                    UnityEditor.EditorGUILayout.ObjectField("Parent", target.Parent, typeof(Transform), true);
                    UnityEditor.EditorGUILayout.ObjectField("Character", target.Character, typeof(Character), true);
                    UnityEditor.EditorGUILayout.LabelField("Hit Data", target.Data?.ToString());

                    UnityEditor.EditorGUILayout.LabelField("Ignore");
                    UnityEditor.EditorGUI.indentLevel++;
                    foreach (var item in target.Ignore)
                    {
                        if (item is Object obj)
                        {
                            UnityEditor.EditorGUILayout.ObjectField(obj, typeof(Object), true);
                        }
                        else
                        {
                            UnityEditor.EditorGUILayout.LabelField(item.ToString());
                        }
                    }
                    UnityEditor.EditorGUI.indentLevel--;
                }
            }

       }

#endif
    }
}
