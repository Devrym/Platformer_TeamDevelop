using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlatformerGame
{
    public sealed class NextExampleTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                var scene = SceneManager.GetActiveScene();
#if UNITY_EDITOR

                var path = scene.path.Replace('\\', '/');
                var directory = Path.GetDirectoryName(path);

                var files = Directory.GetFiles(directory, "*.unity");
                var index = -1;
                for (int i = 0; i < files.Length; i++)
                {
                    var file = files[i].Replace('\\', '/');
                    if (file == path)
                    {
                        index = i;
                        break;
                    }
                }

                index++;
                if (index >= files.Length)
                    index = 0;

                UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(files[index], default);
#else
                SceneManager.LoadScene(scene.buildIndex + 1);
#endif
            }
        }

    }
}
