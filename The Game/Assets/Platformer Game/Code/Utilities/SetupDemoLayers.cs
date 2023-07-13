using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PlatformerGame
{
    public sealed class SetupDemoLayers : MonoBehaviour
    {
        #region Fake Layers
        public const int DefaultExecutionOrder = -32000;

        private const string
            DemoCharacterLayerName = "Ignore Raycast",
            DemoHitLayerName = "Water",
            DemoHitTargetLayerName = "TransparentFX";

        private static int DemoCharacterLayer => LayerMask.NameToLayer(DemoCharacterLayerName);
        private static int DemoHitLayer => LayerMask.NameToLayer(DemoHitLayerName);
        private static int DemoHitTargetLayer => LayerMask.NameToLayer(DemoHitTargetLayerName);

        private int _OriginalCharacterCollisionMask;
        private int _OriginalHitCollisionMask;

        private void Awake()
        {
            _OriginalCharacterCollisionMask = Physics2D.GetLayerCollisionMask(DemoCharacterLayer);
            _OriginalHitCollisionMask = Physics2D.GetLayerCollisionMask(DemoHitLayer);

            Physics2D.IgnoreLayerCollision(DemoCharacterLayer, DemoCharacterLayer);
        }

        private void OnDestroy()
        {
            Physics2D.SetLayerCollisionMask(DemoCharacterLayer, _OriginalCharacterCollisionMask);
            Physics2D.SetLayerCollisionMask(DemoHitLayer, _OriginalHitCollisionMask);
        }

        #endregion
        #region Real Layers
#if UNITY_EDITOR
        [CustomEditor(typeof(SetupDemoLayers))]
        private sealed class Editor : UnityEditor.Editor
        {
            private const string
                ProperCharacterLayerName = "Character",
                ProperHitLayerName = "Hit",
                ProperHitTargetLayerName = "HitTarget",
                ProperLayerNames = "'" + ProperCharacterLayerName + "', '" + ProperHitLayerName + "', and '" + ProperHitTargetLayerName + "'";

            private static int ProperCharacterLayer => LayerMask.NameToLayer(ProperCharacterLayerName);
            private static int ProperHitLayer => LayerMask.NameToLayer(ProperHitLayerName);
            private static int ProperHitTargetLayer => LayerMask.NameToLayer(ProperHitTargetLayerName);

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (EditorApplication.isPlayingOrWillChangePlaymode)
                    return;

                const string Explanation = "By default, this system uses several of Unity's inbuilt physics layers" +
                    " and configures their collision masks in Play Mode so that it doesn't need to modify your project settings:" +
                    "\n• Characters are on the '" + DemoCharacterLayerName + "' layer" +
                    "\n• Hit Triggers are on the '" + DemoHitLayerName + "' layer" +
                    "\n• Destructible Blocks are on the '" + DemoHitTargetLayerName + "' layer" +
                    "\n\nBut for a real project it is better to use custom layers with meaningful names.";

                if (ProperCharacterLayer == -1 || ProperHitLayer == -1 || ProperHitTargetLayer == -1)
                {
                    EditorGUILayout.HelpBox(Explanation + " Click the button below to configure the recommended layers.", MessageType.Info);

                    if (GUILayout.Button("Create Custom Layers"))
                    {
                        if (EditorUtility.DisplayDialog("Create Custom Layers?",
                            $"This will create new layers called {ProperLayerNames} and assign them to the Demo prefabs.",
                            "Create Layers", "Cancel"))
                        {
                            CreateProperLayers();
                            SwapLayers(
                                new int[] { DemoCharacterLayer, DemoHitLayer, DemoHitTargetLayer },
                                new int[] { ProperCharacterLayer, ProperHitLayer, ProperHitTargetLayer, });
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox(Explanation + " Click the button below to remove the recommended layers.", MessageType.Info);

                    if (GUILayout.Button("Delete Custom Layers"))
                    {
                        if (EditorUtility.DisplayDialog("Delete Custom Layers?",
                            $"This will delete the layers called {ProperLayerNames} and revert the Demo prefabs to their default layers.",
                            "Delete Layers", "Cancel"))
                        {
                            SwapLayers(
                                new int[] { ProperCharacterLayer, ProperHitLayer, ProperHitTargetLayer, },
                                new int[] { DemoCharacterLayer, DemoHitLayer, DemoHitTargetLayer });
                            DeleteProperLayers();
                        }
                    }
                }

                if (GUILayout.Button("Open Project Settings"))
                    EditorApplication.ExecuteMenuItem("Edit/Project Settings...");
            }

            private static SerializedProperty GetFirstUserLayerProperty(out int index)
            {
                const string AssetPath = "ProjectSettings/TagManager.asset";
                var asset = AssetDatabase.LoadAssetAtPath<Object>(AssetPath);
                if (asset == null)
                {
                    Debug.LogError($"Unable to load '{AssetPath}'");
                    index = -1;
                    return null;
                }

                var serializedObject = new SerializedObject(asset);
                var layers = serializedObject.FindProperty("layers");
                if (layers == null)
                {
                    Debug.LogError($"Unable to find '{nameof(layers)}' property");
                    index = -1;
                    return null;
                }

                index = 8;
                return layers.GetArrayElementAtIndex(index);
            }

            private static void CreateProperLayers()
            {
                var layer = GetFirstUserLayerProperty(out var index);
                if (layer == null)
                    return;

                var characterLayer = ProperCharacterLayer;
                var hitLayer = ProperHitLayer;
                var hitTargetLayer = ProperHitTargetLayer;

                var depth = layer.depth;
                do
                {
                    if (string.IsNullOrEmpty(layer.stringValue))
                    {
                        if (characterLayer == -1)
                        {
                            characterLayer = index;
                            layer.stringValue = ProperCharacterLayerName;
                        }
                        else if (hitLayer == -1)
                        {
                            hitLayer = index;
                            layer.stringValue = ProperHitLayerName;
                        }
                        else if (hitTargetLayer == -1)
                        {
                            hitTargetLayer = index;
                            layer.stringValue = ProperHitTargetLayerName;
                            break;
                        }
                    }

                    index++;
                    layer.Next(false);
                }
                while (layer.depth == depth);

                layer.serializedObject.ApplyModifiedProperties();

                Physics2D.IgnoreLayerCollision(characterLayer, characterLayer);

                for (int i = 0; i < 32; i++)
                    Physics2D.IgnoreLayerCollision(hitLayer, i, i != characterLayer && i != hitTargetLayer);
            }

            private static void DeleteProperLayers()
            {
                var layer = GetFirstUserLayerProperty(out var _);
                if (layer == null)
                    return;

                var depth = layer.depth;
                do
                {
                    switch (layer.stringValue)
                    {
                        case ProperCharacterLayerName:
                        case ProperHitLayerName:
                        case ProperHitTargetLayerName:
                            layer.stringValue = null;
                            break;
                    }

                    layer.Next(false);
                }
                while (layer.depth == depth);

                layer.serializedObject.ApplyModifiedProperties();
            }

       }


        private static void SwapLayers(int[] from, int[] to)
        {

            EditorUtility.ClearProgressBar();
        }


        private static void SwapLayersOnGameObject(GameObject gameObject, int[] from, int[] to)
        {
            var index = ArrayUtility.IndexOf(from, gameObject.layer);
            if (index >= 0)
                gameObject.layer = to[index];

            var transform = gameObject.transform;
            for (int i = 0; i < transform.childCount; i++)
                SwapLayersOnGameObject(transform.GetChild(i).gameObject, from, to);
        }

#endif
        #endregion
    }
}
