using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lunara.Gameplay;

namespace Lunara.Editor
{
    public static class LunaraSceneBootstrap
    {
        private const string ScenePath = "Assets/LUNARA/Scenes/LUNARA_WhiteNight.unity";

        [MenuItem("LUNARA/Build First World Scene")]
        public static void BuildFirstScene()
        {
            EnsureFolder("Assets/LUNARA/Scenes");
            EnsureFolder("Assets/LUNARA/Materials");
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            Material terrainMat = Mat("MAT_Moonland", new Color(0.23f, 0.31f, 0.29f), 0.25f);
            Material waterMat = Mat("MAT_Silverwater", new Color(0.18f, 0.48f, 0.66f), 0.6f);
            Material rockMat = Mat("MAT_Moonstone", new Color(0.58f, 0.65f, 0.73f), 0.22f);
            Material barkMat = Mat("MAT_MoonwoodBark", new Color(0.16f, 0.13f, 0.16f), 0.15f);
            Material leafMat = Mat("MAT_MoonwoodLeaves", new Color(0.20f, 0.39f, 0.37f), 0.18f);
            Material playerMat = Mat("MAT_PlayerArmor", new Color(0.12f, 0.24f, 0.34f), 0.48f);
            Material enemyMat = Mat("MAT_Sentinel", new Color(0.32f, 0.20f, 0.48f), 0.45f);
            Terrain terrain = CreateTerrain(terrainMat);
            CreateRiver(waterMat, terrain);
            CreateMoonheartTemple(rockMat, terrain);
            CreateMoonwoodForest(barkMat, leafMat, terrain);
            LunaraPlayerController player = CreatePlayer(playerMat, terrain);
            CreateSentinel(enemyMat, terrain, player);
            CreateMoonlight();
            LunaraHUD hud = player.gameObject.AddComponent<LunaraHUD>();
            SerializedObject hudData = new SerializedObject(hud);
            hudData.FindProperty("player").objectReferenceValue = player;
            hudData.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            Selection.activeGameObject = player.gameObject;
            Debug.Log("LUNARA White Night scene generated. First development scene only; not a release build.");
        }

        private static Terrain CreateTerrain(Material material)
        {
            const int res = 257;
            const int size = 900;
            TerrainData data = new TerrainData { heightmapResolution = res, size = new Vector3(size, 120, size) };
            float[,] heights = new float[res, res];
            for (int z = 0; z < res; z++)
            for (int x = 0; x < res; x++)
            {
                float nx = x / (float)(res - 1);
                float nz = z / (float)(res - 1);
                float rolling = Mathf.PerlinNoise(nx * 3.7f + 3.1f, nz * 3.7f + 8.6f) * 0.045f;
                float detail = Mathf.PerlinNoise(nx * 14f, nz * 14f) * 0.009f;
                float ridge = Mathf.Pow(Mathf.Max(0, Mathf.PerlinNoise(nx * 5.2f + 11f, nz * 5.2f) - 0.59f), 1.3f) * 0.13f;
                heights[z, x] = Mathf.Clamp01(0.025f + rolling + detail + ridge);
            }
            data.SetHeights(0, 0, heights);
            AssetDatabase.CreateAsset(data, "Assets/LUNARA/Scenes/WhiteNight_Terrain.asset");
            GameObject go = Terrain.CreateTerrainGameObject(data);
            go.name = "White Night Terrain";
            Terrain terrain = go.GetComponent<Terrain>();
            terrain.materialTemplate = null;
            terrain.heightmapPixelError = 7;
            terrain.basemapDistance = 1200;
            return terrain;
        }

        private static void CreateRiver(Material material, Terrain terrain)
        {
            GameObject root = new GameObject("Silver Moon River");
            Mesh mesh = new Mesh { name = "Silver Moon River Mesh" };
            const int count = 120;
            Vector3[] vertices = new Vector3[(count + 1) * 2];
            int[] triangles = new int[count * 6];
            for (int i = 0; i <= count; i++)
            {
                float t = i / (float)count;
                float z = 22 + t * 840;
                float x = 440 + Mathf.Sin(t * Mathf.PI * 2.25f) * 118 + Mathf.Sin(t * 17f) * 16;
                float width = 10 + Mathf.Sin(t * Mathf.PI) * 4;
                float y = terrain.SampleHeight(new Vector3(x, 0, z)) + 0.24f;
                vertices[i * 2] = new Vector3(x - width, y, z);
                vertices[i * 2 + 1] = new Vector3(x + width, y, z);
                if (i < count)
                {
                    int k = i * 6, v = i * 2;
                    triangles[k] = v; triangles[k + 1] = v + 2; triangles[k + 2] = v + 1;
                    triangles[k + 3] = v + 1; triangles[k + 4] = v + 2; triangles[k + 5] = v + 3;
                }
            }
            mesh.vertices = vertices; mesh.triangles = triangles; mesh.RecalculateNormals();
            root.AddComponent<MeshFilter>().sharedMesh = mesh;
            root.AddComponent<MeshRenderer>().sharedMaterial = material;
        }

        private static void CreateMoonheartTemple(Material stone, Terrain terrain)
        {
            Vector3 site = new Vector3(460, 0, 720);
            site.y = terrain.SampleHeight(site);
            GameObject root = new GameObject("Moonheart Temple");
            root.transform.position = site;
            Primitive(root, PrimitiveType.Cylinder, "Temple dais", new Vector3(0, 1.4f, 0), new Vector3(22, 1.4f, 22), stone);
            for (int i = 0; i < 8; i++)
            {
                float a = i * Mathf.PI * 2f / 8f;
                Vector3 p = new Vector3(Mathf.Cos(a) * 16, 8, Mathf.Sin(a) * 16);
                Primitive(root, PrimitiveType.Cylinder, "Moonstone pillar", p, new Vector3(1.5f, 8, 1.5f), stone);
            }
            Primitive(root, PrimitiveType.Cube, "Sanctum lintel", new Vector3(0, 17, 0), new Vector3(29, 1.7f, 5), stone);
            Primitive(root, PrimitiveType.Sphere, "Moonheart crystal", new Vector3(0, 23, 0), new Vector3(3.5f, 3.5f, 3.5f), stone);
        }

        private static void CreateMoonwoodForest(Material bark, Material leaves, Terrain terrain)
        {
            Random.InitState(7421);
            for (int i = 0; i < 220; i++)
            {
                float x = Random.Range(25f, 875f);
                float z = Random.Range(25f, 875f);
                if (x > 340 && x < 550) continue;
                float y = terrain.SampleHeight(new Vector3(x, 0, z));
                float scale = Random.Range(0.75f, 1.65f);
                GameObject tree = new GameObject("Moonwood Tree");
                tree.transform.position = new Vector3(x, y, z);
                Primitive(tree, PrimitiveType.Cylinder, "Trunk", new Vector3(0, 3f * scale, 0),
                    new Vector3(0.42f * scale, 3f * scale, 0.42f * scale), bark);
                Primitive(tree, PrimitiveType.Sphere, "Canopy", new Vector3(0, 6.2f * scale, 0),
                    new Vector3(3.2f * scale, 4.3f * scale, 3.2f * scale), leaves);
            }
        }

        private static LunaraPlayerController CreatePlayer(Material material, Terrain terrain)
        {
            Vector3 spawn = new Vector3(410, 0, 110);
            spawn.y = terrain.SampleHeight(spawn) + 2;
            GameObject player = new GameObject("LUNARA Player");
            player.transform.position = spawn;
            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = 1.9f; cc.radius = 0.43f; cc.center = new Vector3(0, 0.95f, 0);
            Primitive(player, PrimitiveType.Capsule, "Player visual", new Vector3(0, 0.95f, 0),
                new Vector3(0.78f, 0.95f, 0.78f), material);
            GameObject pivot = new GameObject("Camera Pivot");
            pivot.transform.SetParent(player.transform, false);
            pivot.transform.localPosition = new Vector3(0, 1.5f, 0);
            GameObject camGo = new GameObject("Third Person Camera");
            camGo.transform.SetParent(pivot.transform, false);
            camGo.transform.localPosition = new Vector3(0, 0.8f, -5.4f);
            camGo.transform.localRotation = Quaternion.Euler(8, 0, 0);
            Camera cam = camGo.AddComponent<Camera>();
            cam.fieldOfView = 72; cam.nearClipPlane = 0.08f;
            camGo.AddComponent<AudioListener>();
            LunaraPlayerController controller = player.AddComponent<LunaraPlayerController>();
            SerializedObject data = new SerializedObject(controller);
            data.FindProperty("cameraPivot").objectReferenceValue = pivot.transform;
            data.ApplyModifiedPropertiesWithoutUndo();
            return controller;
        }

        private static void CreateSentinel(Material material, Terrain terrain, LunaraPlayerController player)
        {
            Vector3 pos = player.transform.position + player.transform.forward * 12;
            pos.y = terrain.SampleHeight(pos) + 1.1f;
            GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Moon Sentinel";
            enemy.transform.position = pos;
            enemy.GetComponent<Renderer>().sharedMaterial = material;
            enemy.AddComponent<LunaraDamageable>();
            enemy.AddComponent<LunaraSentinelAI>().SetTarget(player.transform);
        }

        private static void CreateMoonlight()
        {
            GameObject lightGo = new GameObject("White Night Moonlight");
            Light light = lightGo.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 0.8f;
            light.color = new Color(0.72f, 0.82f, 1f);
            light.shadows = LightShadows.Soft;
            lightGo.transform.rotation = Quaternion.Euler(38, -32, 0);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.23f, 0.29f, 0.39f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.10f, 0.16f, 0.25f);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.0009f;
        }

        private static GameObject Primitive(GameObject parent, PrimitiveType type, string name,
            Vector3 localPosition, Vector3 localScale, Material material)
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.name = name; obj.transform.SetParent(parent.transform, false);
            obj.transform.localPosition = localPosition; obj.transform.localScale = localScale;
            Collider col = obj.GetComponent<Collider>();
            if (col != null) Object.DestroyImmediate(col);
            obj.GetComponent<Renderer>().sharedMaterial = material;
            return obj;
        }

        private static Material Mat(string name, Color color, float metallic)
        {
            string path = "Assets/LUNARA/Materials/" + name + ".mat";
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                Shader shader = Shader.Find("Standard");
                if (shader == null) shader = Shader.Find("Universal Render Pipeline/Lit");
                mat = new Material(shader) { name = name };
                AssetDatabase.CreateAsset(mat, path);
            }
            if (mat.HasProperty("_Color")) mat.color = color;
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static void EnsureFolder(string path)
        {
            string[] parts = path.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next)) AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
