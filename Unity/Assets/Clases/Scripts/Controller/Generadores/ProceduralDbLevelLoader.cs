using System;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDbLevelLoader : MonoBehaviour
{
    [Header("JSON (Resources)")]
    [Tooltip("Ruta en Resources SIN extensión. Ej: DB/levels_bd")]
    public string jsonResourcePath = "DB/levels_bd";

    [Header("Prefabs (Resources)")]
    [Tooltip("Carpeta en Resources donde están los prefabs. Ej: Prefabs")]
    public string prefabsFolderInResources = "Prefabs";

    [Header("Level")]
    public int levelId = 1;
    public bool buildOnStart = true;
    public bool clearBeforeBuild = true;

    [Header("Parents (opcional)")]
    public Transform interactiveParent;
    public Transform staticParent;

    // --- Datos JSON ---
    [Serializable]
    public class LevelObject
    {
        public string prefab;
        public float[] position;
        public float[] rotation; // euler
        public float[] scale;
        public string type;      // "interactive" | "static"
    }

    [Serializable]
    public class LevelData
    {
        public int id;
        public string plane;
        public List<LevelObject> objects = new();
    }

    [Serializable]
    public class LevelsDb
    {
        public List<LevelData> levels = new();
    }

    private readonly List<GameObject> _spawned = new();

    private void Start()
    {
        if (buildOnStart) BuildLevel();
    }

    [ContextMenu("Build Level")]
    public void BuildLevel()
    {
        if (clearBeforeBuild) ClearSpawned();

        // 1) Cargar JSON
        var ta = Resources.Load<TextAsset>(jsonResourcePath);
        if (ta == null)
        {
            Debug.LogError($"[ProceduralDbLevelLoader] No se encontró JSON en Resources: {jsonResourcePath}");
            return;
        }

        LevelsDb db = JsonUtility.FromJson<LevelsDb>(ta.text);
        if (db == null || db.levels == null || db.levels.Count == 0)
        {
            Debug.LogError("[ProceduralDbLevelLoader] JSON inválido o sin niveles.");
            return;
        }

        // 2) Buscar nivel
        LevelData level = db.levels.Find(l => l.id == levelId);
        if (level == null)
        {
            Debug.LogError($"[ProceduralDbLevelLoader] No existe level con id={levelId}");
            return;
        }

        // 3) Instanciar objetos
        foreach (var obj in level.objects)
        {
            if (string.IsNullOrWhiteSpace(obj.prefab))
            {
                Debug.LogWarning("[ProceduralDbLevelLoader] Objeto sin 'prefab' en JSON, se ignora.");
                continue;
            }

            var prefab = Resources.Load<GameObject>($"{prefabsFolderInResources}/{obj.prefab}");
            if (prefab == null)
            {
                Debug.LogError($"[ProceduralDbLevelLoader] Prefab '{obj.prefab}' no encontrado en Resources/{prefabsFolderInResources}.");
                continue;
            }

            Vector3 pos = ReadVector3(obj.position, Vector3.zero);
            Vector3 eul = ReadVector3(obj.rotation, Vector3.zero);
            Vector3 scl = ReadVector3(obj.scale, Vector3.one);

            Transform parent = ChooseParent(obj.type);
            var instance = Instantiate(prefab, pos, Quaternion.Euler(eul), parent);
            instance.transform.localScale = scl;

            // (Opcional) marca por tipo: tags/layers si ya existen en tu proyecto
            // if (IsInteractive(obj.type)) { instance.tag = "Interactive"; }
            // else { instance.tag = "Static"; }

            _spawned.Add(instance);
        }

        Debug.Log($"[ProceduralDbLevelLoader] Nivel {levelId} construido. Objetos: {_spawned.Count}");
    }

    private Transform ChooseParent(string type)
    {
        if (IsInteractive(type) && interactiveParent) return interactiveParent;
        if (!IsInteractive(type) && staticParent) return staticParent;
        return this.transform;
    }

    private static bool IsInteractive(string type) =>
        string.Equals(type, "interactive", StringComparison.OrdinalIgnoreCase);

    private static Vector3 ReadVector3(float[] arr, Vector3 fallback)
    {
        if (arr == null || arr.Length == 0) return fallback;
        float x = arr.Length > 0 ? arr[0] : fallback.x;
        float y = arr.Length > 1 ? arr[1] : fallback.y;
        float z = arr.Length > 2 ? arr[2] : fallback.z;
        return new Vector3(x, y, z);
    }

    [ContextMenu("Clear Spawned")]
    public void ClearSpawned()
    {
        for (int i = _spawned.Count - 1; i >= 0; --i)
        {
            var go = _spawned[i];
            if (go) DestroyImmediate(go);
        }
        _spawned.Clear();
    }
}
