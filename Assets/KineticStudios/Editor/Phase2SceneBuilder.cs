#if UNITY_EDITOR
using KineticStudios.Builder;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace KineticStudios.EditorTools
{
    public static class Phase2SceneBuilder
    {
        private const string MaterialsPath = "Assets/KineticStudios/Art/Materials";
        private const string WorkspacePath = "Assets/KineticStudios/Scenes/StudioWorkspace.unity";

        [MenuItem("Kinetic Studios/Phase 2/Integrate MVP Builder")]
        public static void Build()
        {
            Material wood = CreateMaterial("Wood", new Color(0.42f, 0.22f, 0.09f), 0f, 0.28f);
            Material metal = CreateMaterial("Metal", new Color(0.48f, 0.53f, 0.57f), 0.82f, 0.72f);
            Material rope = CreateMaterial("Rope", new Color(0.58f, 0.39f, 0.19f), 0f, 0.12f);
            Material glass = CreateGlassMaterial();

            EditorSceneManager.OpenScene(WorkspacePath);
            Camera studioCamera = Object.FindAnyObjectByType<Camera>();
            StudioBuilderController existing = Object.FindAnyObjectByType<StudioBuilderController>();
            if (existing != null)
            {
                Object.DestroyImmediate(existing.gameObject);
            }

            GameObject builderObject = new("Studio Builder");
            StudioBuilderController builder = builderObject.AddComponent<StudioBuilderController>();
            builder.Configure(studioCamera, wood, metal, rope, glass);

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            Debug.Log("Kinetic Studios Phase 2 scene integration completed.");
        }

        private static Material CreateMaterial(string name, Color color, float metallic, float smoothness)
        {
            string path = $"{MaterialsPath}/{name}.mat";
            Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null)
            {
                return existing;
            }

            Material material = new(Shader.Find("Standard"))
            {
                name = name,
                color = color
            };
            material.SetFloat("_Metallic", metallic);
            material.SetFloat("_Glossiness", smoothness);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static Material CreateGlassMaterial()
        {
            string path = $"{MaterialsPath}/Glass.mat";
            Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null)
            {
                return existing;
            }

            Material material = new(Shader.Find("Standard"))
            {
                name = "Glass",
                color = new Color(0.48f, 0.78f, 0.86f, 0.52f),
                renderQueue = 3000
            };
            material.SetFloat("_Mode", 3f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.SetFloat("_Metallic", 0.05f);
            material.SetFloat("_Glossiness", 0.9f);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }
    }
}
#endif
