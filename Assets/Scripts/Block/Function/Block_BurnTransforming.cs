using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Block_BurnTransforming : MonoBehaviour
{
    [Header("New Block When Burned")]
    [SerializeField] GameObject burningBlock;

    [Header("Other Parameters")]
    [SerializeField] GameObject burningBlock_InScene;
    public bool isSteppedOn;

    private bool isBurningBlock;

    private Renderer[] originalRenderers;
    private Collider[] originalColliders;

    private GameObject effectBlock_InScene;


    //--------------------


    private void Awake()
    {
        originalRenderers = GetComponentsInChildren<Renderer>(true);
        originalColliders = GetComponentsInChildren<Collider>(true);
    }

    private void OnEnable()
    {
        Movement.Action_isSwitchingBlocks += CheckIfSteppenOn;
        Movement.Action_StepTaken += BurnBlock;
        Movement.Action_RespawnPlayerEarly += ResetBlock;

        Player_Burning.Action_PlayerStartedBurning += BurnBlock;
    }

    private void OnDisable()
    {
        Movement.Action_isSwitchingBlocks -= CheckIfSteppenOn;
        Movement.Action_StepTaken -= BurnBlock;
        Movement.Action_RespawnPlayerEarly -= ResetBlock;

        Player_Burning.Action_PlayerStartedBurning -= BurnBlock;
    }


    //--------------------


    void CheckIfSteppenOn()
    {
        if (burningBlock_InScene != null) { return; }

        if (Movement.Instance.blockStandingOn == gameObject)
            isSteppedOn = true;
        else
        {
            ResetBlock();
        }
    }

    void BurnBlock()
    {
        if (!Player_Burning.Instance.isBurning || Player_CeilingGrab.Instance.isCeilingGrabbing) { return; }

        if (burningBlock_InScene != null) { return; }

        if (isBurningBlock) { return; }

        if (!isSteppedOn && Movement.Instance.blockStandingOn != gameObject) { return; }

        StartCoroutine(WaitBeforeBurningBlock(0.005f));
    }

    IEnumerator WaitBeforeBurningBlock(float waitTime)
    {
        isBurningBlock = true;

        yield return new WaitForSeconds(waitTime);

        if (burningBlock_InScene != null)
        {
            isBurningBlock = false;
            yield break;
        }

        if (Movement.Instance.blockStandingOn != gameObject)
        {
            isBurningBlock = false;
            yield break;
        }

        burningBlock_InScene = Instantiate(burningBlock, transform.position, transform.rotation);
        burningBlock_InScene.transform.SetParent(transform, true);

        AddEffectBlockToBurningBlock();

        Movement.Instance.blockStandingOn = burningBlock_InScene;

        HideOriginalBlock();

        isSteppedOn = false;
        isBurningBlock = false;

        Movement.Instance.UpdateAvailableMovementBlocks();
    }

    public void ResetBlock()
    {
        isSteppedOn = false;
        isBurningBlock = false;

        if (effectBlock_InScene != null)
        {
            Destroy(effectBlock_InScene);
            effectBlock_InScene = null;
        }

        GameObject blockToRemove = burningBlock_InScene;

        if (blockToRemove != null)
        {
            if (Movement.Instance.blockStandingOn == blockToRemove)
            {
                Movement.Instance.blockStandingOn = gameObject;
            }

            Destroy(blockToRemove);
            burningBlock_InScene = null;
        }

        ShowOriginalBlock();

        // Do not call Movement.Instance.UpdateAvailableMovementBlocks() here.
        // ResetBlock can be called from Movement.Action_isSwitchingBlocks.
    }

    void HideOriginalBlock()
    {
        for (int i = 0; i < originalRenderers.Length; i++)
        {
            if (originalRenderers[i] != null)
            {
                originalRenderers[i].enabled = false;
            }
        }

        for (int i = 0; i < originalColliders.Length; i++)
        {
            if (originalColliders[i] != null)
            {
                originalColliders[i].enabled = false;
            }
        }
    }

    void ShowOriginalBlock()
    {
        for (int i = 0; i < originalRenderers.Length; i++)
        {
            if (originalRenderers[i] != null)
            {
                originalRenderers[i].enabled = true;
            }
        }

        for (int i = 0; i < originalColliders.Length; i++)
        {
            if (originalColliders[i] != null)
            {
                originalColliders[i].enabled = true;
            }
        }
    }

    void AddEffectBlockToBurningBlock()
    {
        if (burningBlock_InScene == null) { return; }

        if (EffectBlockManager.Instance == null) { return; }

        EffectVisualMarker originalEffectMarker = GetOriginalEffectVisualMarker();

        if (originalEffectMarker == null) { return; }

        GameObject effectPrefab = GetEffectPrefab(originalEffectMarker);

        if (effectPrefab == null) { return; }

        Transform parentForEffect = GetNumberDisplayParentOnBurningBlock();

        effectBlock_InScene = Instantiate(
            effectPrefab,
            originalEffectMarker.transform.position,
            originalEffectMarker.transform.rotation,
            parentForEffect
        );

        effectBlock_InScene.transform.localScale = originalEffectMarker.transform.localScale;

        SetupEffectBlockInfoOnBurningBlock(originalEffectMarker, effectBlock_InScene);
        AddCheckpointComponentToBurningBlock(originalEffectMarker, effectBlock_InScene);

        effectBlock_InScene.SendMessage("Setup", SendMessageOptions.DontRequireReceiver);
        effectBlock_InScene.SendMessage("Initialize", SendMessageOptions.DontRequireReceiver);
        effectBlock_InScene.SendMessage("Refresh", SendMessageOptions.DontRequireReceiver);

        burningBlock_InScene.SendMessage("Setup", SendMessageOptions.DontRequireReceiver);
        burningBlock_InScene.SendMessage("Initialize", SendMessageOptions.DontRequireReceiver);
        burningBlock_InScene.SendMessage("Refresh", SendMessageOptions.DontRequireReceiver);
    }

    EffectVisualMarker GetOriginalEffectVisualMarker()
    {
        EffectVisualMarker[] effectVisualMarkers = GetComponentsInChildren<EffectVisualMarker>(true);

        for (int i = 0; i < effectVisualMarkers.Length; i++)
        {
            if (effectVisualMarkers[i] == null) { continue; }

            if (burningBlock_InScene != null && effectVisualMarkers[i].transform.IsChildOf(burningBlock_InScene.transform))
            {
                continue;
            }

            string markerType = GetEffectVisualMarkerTypeAsString(effectVisualMarkers[i]);

            if (markerType == "SpawnPoint") { return effectVisualMarkers[i]; }
            if (markerType == "Checkpoint") { return effectVisualMarkers[i]; }
            if (markerType == "Teleporter") { return effectVisualMarkers[i]; }
            if (markerType == "Moveable") { return effectVisualMarkers[i]; }
            if (markerType == "Movable") { return effectVisualMarkers[i]; }
        }

        return null;
    }

    GameObject GetEffectPrefab(EffectVisualMarker effectVisualMarker)
    {
        string markerType = GetEffectVisualMarkerTypeAsString(effectVisualMarker);

        if (markerType == "SpawnPoint" || markerType == "Checkpoint")
        {
            return EffectBlockManager.Instance.effectBlock_SpawnPoint_Prefab;
        }

        if (markerType == "Teleporter")
        {
            return EffectBlockManager.Instance.effectBlock_Teleporter_Prefab;
        }

        if (markerType == "Moveable" || markerType == "Movable")
        {
            return EffectBlockManager.Instance.effectBlock_Moveable_Prefab;
        }

        return null;
    }

    Transform GetNumberDisplayParentOnBurningBlock()
    {
        NumberDisplay numberDisplay = burningBlock_InScene.GetComponentInChildren<NumberDisplay>(true);

        if (numberDisplay != null)
        {
            return numberDisplay.transform;
        }

        return burningBlock_InScene.transform;
    }

    void SetupEffectBlockInfoOnBurningBlock(EffectVisualMarker originalEffectMarker, GameObject newEffectObject)
    {
        if (burningBlock_InScene == null) { return; }

        if (originalEffectMarker == null) { return; }

        if (newEffectObject == null) { return; }

        EffectBlockInfo originalEffectBlockInfo = GetComponent<EffectBlockInfo>();

        if (originalEffectBlockInfo == null) { return; }

        EffectBlockInfo burningEffectBlockInfo = burningBlock_InScene.GetComponent<EffectBlockInfo>();

        if (burningEffectBlockInfo == null)
        {
            burningEffectBlockInfo = burningBlock_InScene.AddComponent<EffectBlockInfo>();
        }

        JsonUtility.FromJsonOverwrite(
            JsonUtility.ToJson(originalEffectBlockInfo),
            burningEffectBlockInfo
        );

        string markerType = GetEffectVisualMarkerTypeAsString(originalEffectMarker);

        SetEffectBlockInfoReferences(burningEffectBlockInfo, markerType, newEffectObject);

        burningEffectBlockInfo.enabled = true;
    }

    void AddCheckpointComponentToBurningBlock(EffectVisualMarker originalEffectMarker, GameObject newEffectObject)
    {
        if (burningBlock_InScene == null) { return; }

        if (originalEffectMarker == null) { return; }

        if (newEffectObject == null) { return; }

        string markerType = GetEffectVisualMarkerTypeAsString(originalEffectMarker);

        if (markerType != "SpawnPoint" && markerType != "Checkpoint")
        {
            return;
        }

        Block_Checkpoint originalCheckpoint = GetComponent<Block_Checkpoint>();

        Block_Checkpoint burningCheckpoint = burningBlock_InScene.GetComponent<Block_Checkpoint>();

        if (burningCheckpoint == null)
        {
            burningCheckpoint = burningBlock_InScene.AddComponent<Block_Checkpoint>();
        }

        if (originalCheckpoint != null)
        {
            JsonUtility.FromJsonOverwrite(
                JsonUtility.ToJson(originalCheckpoint),
                burningCheckpoint
            );

            CopySpawnDirection(originalCheckpoint, burningCheckpoint);
        }

        SetCheckpointReferences(burningCheckpoint, newEffectObject);

        burningCheckpoint.enabled = true;

        burningCheckpoint.SendMessage("Setup", SendMessageOptions.DontRequireReceiver);
        burningCheckpoint.SendMessage("Initialize", SendMessageOptions.DontRequireReceiver);
        burningCheckpoint.SendMessage("Refresh", SendMessageOptions.DontRequireReceiver);
    }

    void CopySpawnDirection(Block_Checkpoint originalCheckpoint, Block_Checkpoint burningCheckpoint)
    {
        if (originalCheckpoint == null) { return; }

        if (burningCheckpoint == null) { return; }

        FieldInfo[] originalFields = originalCheckpoint.GetType().GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        for (int i = 0; i < originalFields.Length; i++)
        {
            FieldInfo originalField = originalFields[i];

            if (originalField == null) { continue; }

            string fieldName = originalField.Name.ToLower();

            if (!fieldName.Contains("spawn") || !fieldName.Contains("direction"))
            {
                continue;
            }

            FieldInfo burningField = burningCheckpoint.GetType().GetField(
                originalField.Name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (burningField == null) { continue; }

            if (burningField.FieldType != originalField.FieldType) { continue; }

            burningField.SetValue(burningCheckpoint, originalField.GetValue(originalCheckpoint));
        }

        PropertyInfo[] originalProperties = originalCheckpoint.GetType().GetProperties(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        for (int i = 0; i < originalProperties.Length; i++)
        {
            PropertyInfo originalProperty = originalProperties[i];

            if (originalProperty == null) { continue; }

            if (!originalProperty.CanRead) { continue; }

            if (originalProperty.GetIndexParameters().Length > 0) { continue; }

            string propertyName = originalProperty.Name.ToLower();

            if (!propertyName.Contains("spawn") || !propertyName.Contains("direction"))
            {
                continue;
            }

            PropertyInfo burningProperty = burningCheckpoint.GetType().GetProperty(
                originalProperty.Name,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (burningProperty == null) { continue; }

            if (!burningProperty.CanWrite) { continue; }

            if (burningProperty.GetIndexParameters().Length > 0) { continue; }

            if (burningProperty.PropertyType != originalProperty.PropertyType) { continue; }

            burningProperty.SetValue(
                burningCheckpoint,
                originalProperty.GetValue(originalCheckpoint, null),
                null
            );
        }
    }

    void SetCheckpointReferences(Block_Checkpoint checkpoint, GameObject newEffectObject)
    {
        if (checkpoint == null) { return; }

        if (newEffectObject == null) { return; }

        FieldInfo[] fields = checkpoint.GetType().GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];

            if (field == null) { continue; }

            string fieldName = field.Name.ToLower();

            if (field.FieldType == typeof(GameObject))
            {
                if (fieldName.Contains("checkpoint") ||
                    fieldName.Contains("spawn") ||
                    fieldName.Contains("effect"))
                {
                    field.SetValue(checkpoint, newEffectObject);
                }
                else if (fieldName.Contains("block"))
                {
                    field.SetValue(checkpoint, burningBlock_InScene);
                }
            }
            else if (typeof(Component).IsAssignableFrom(field.FieldType))
            {
                if (fieldName.Contains("checkpoint") ||
                    fieldName.Contains("spawn") ||
                    fieldName.Contains("effect"))
                {
                    Component component = newEffectObject.GetComponent(field.FieldType);

                    if (component == null)
                    {
                        component = newEffectObject.GetComponentInChildren(field.FieldType, true);
                    }

                    if (component != null)
                    {
                        field.SetValue(checkpoint, component);
                    }
                }
                else if (fieldName.Contains("block"))
                {
                    Component component = burningBlock_InScene.GetComponent(field.FieldType);

                    if (component == null)
                    {
                        component = burningBlock_InScene.GetComponentInChildren(field.FieldType, true);
                    }

                    if (component != null)
                    {
                        field.SetValue(checkpoint, component);
                    }
                }
            }
        }

        PropertyInfo[] properties = checkpoint.GetType().GetProperties(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo property = properties[i];

            if (property == null) { continue; }

            if (!property.CanWrite) { continue; }

            if (property.GetIndexParameters().Length > 0) { continue; }

            string propertyName = property.Name.ToLower();

            if (property.PropertyType == typeof(GameObject))
            {
                if (propertyName.Contains("checkpoint") ||
                    propertyName.Contains("spawn") ||
                    propertyName.Contains("effect"))
                {
                    property.SetValue(checkpoint, newEffectObject, null);
                }
                else if (propertyName.Contains("block"))
                {
                    property.SetValue(checkpoint, burningBlock_InScene, null);
                }
            }
            else if (typeof(Component).IsAssignableFrom(property.PropertyType))
            {
                if (propertyName.Contains("checkpoint") ||
                    propertyName.Contains("spawn") ||
                    propertyName.Contains("effect"))
                {
                    Component component = newEffectObject.GetComponent(property.PropertyType);

                    if (component == null)
                    {
                        component = newEffectObject.GetComponentInChildren(property.PropertyType, true);
                    }

                    if (component != null)
                    {
                        property.SetValue(checkpoint, component, null);
                    }
                }
                else if (propertyName.Contains("block"))
                {
                    Component component = burningBlock_InScene.GetComponent(property.PropertyType);

                    if (component == null)
                    {
                        component = burningBlock_InScene.GetComponentInChildren(property.PropertyType, true);
                    }

                    if (component != null)
                    {
                        property.SetValue(checkpoint, component, null);
                    }
                }
            }
        }
    }

    void SetEffectBlockInfoReferences(EffectBlockInfo effectBlockInfo, string markerType, GameObject newEffectObject)
    {
        if (effectBlockInfo == null) { return; }

        if (newEffectObject == null) { return; }

        string normalizedType = markerType.ToLower();

        if (normalizedType == "checkpoint")
        {
            normalizedType = "spawnpoint";
        }

        if (normalizedType == "movable")
        {
            normalizedType = "moveable";
        }

        FieldInfo[] fields = effectBlockInfo.GetType().GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo field = fields[i];

            if (field == null) { continue; }

            string fieldName = field.Name.ToLower();

            if (field.FieldType == typeof(bool) && fieldName.Contains("isadded"))
            {
                if (fieldName.Contains("spawnpoint") || fieldName.Contains("checkpoint"))
                {
                    field.SetValue(effectBlockInfo, normalizedType == "spawnpoint");
                }
                else if (fieldName.Contains("teleporter"))
                {
                    field.SetValue(effectBlockInfo, normalizedType == "teleporter");
                }
                else if (fieldName.Contains("moveable") || fieldName.Contains("movable"))
                {
                    field.SetValue(effectBlockInfo, normalizedType == "moveable");
                }
            }

            if (field.FieldType == typeof(GameObject))
            {
                if (FieldMatchesEffectType(fieldName, normalizedType))
                {
                    field.SetValue(effectBlockInfo, newEffectObject);
                }
            }
            else if (typeof(Component).IsAssignableFrom(field.FieldType))
            {
                if (FieldMatchesEffectType(fieldName, normalizedType))
                {
                    Component component = newEffectObject.GetComponent(field.FieldType);

                    if (component == null)
                    {
                        component = newEffectObject.GetComponentInChildren(field.FieldType, true);
                    }

                    if (component != null)
                    {
                        field.SetValue(effectBlockInfo, component);
                    }
                }
            }
        }

        PropertyInfo[] properties = effectBlockInfo.GetType().GetProperties(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo property = properties[i];

            if (property == null) { continue; }

            if (!property.CanWrite) { continue; }

            if (property.GetIndexParameters().Length > 0) { continue; }

            string propertyName = property.Name.ToLower();

            if (property.PropertyType == typeof(bool) && propertyName.Contains("isadded"))
            {
                if (propertyName.Contains("spawnpoint") || propertyName.Contains("checkpoint"))
                {
                    property.SetValue(effectBlockInfo, normalizedType == "spawnpoint", null);
                }
                else if (propertyName.Contains("teleporter"))
                {
                    property.SetValue(effectBlockInfo, normalizedType == "teleporter", null);
                }
                else if (propertyName.Contains("moveable") || propertyName.Contains("movable"))
                {
                    property.SetValue(effectBlockInfo, normalizedType == "moveable", null);
                }
            }

            if (property.PropertyType == typeof(GameObject))
            {
                if (FieldMatchesEffectType(propertyName, normalizedType))
                {
                    property.SetValue(effectBlockInfo, newEffectObject, null);
                }
            }
            else if (typeof(Component).IsAssignableFrom(property.PropertyType))
            {
                if (FieldMatchesEffectType(propertyName, normalizedType))
                {
                    Component component = newEffectObject.GetComponent(property.PropertyType);

                    if (component == null)
                    {
                        component = newEffectObject.GetComponentInChildren(property.PropertyType, true);
                    }

                    if (component != null)
                    {
                        property.SetValue(effectBlockInfo, component, null);
                    }
                }
            }
        }
    }

    bool FieldMatchesEffectType(string fieldName, string normalizedType)
    {
        if (normalizedType == "spawnpoint")
        {
            return fieldName.Contains("spawnpoint") || fieldName.Contains("checkpoint");
        }

        if (normalizedType == "teleporter")
        {
            return fieldName.Contains("teleporter");
        }

        if (normalizedType == "moveable")
        {
            return fieldName.Contains("moveable") || fieldName.Contains("movable");
        }

        return false;
    }

    string GetEffectVisualMarkerTypeAsString(EffectVisualMarker effectVisualMarker)
    {
        if (effectVisualMarker == null) { return ""; }

        FieldInfo typeField = effectVisualMarker.GetType().GetField(
            "Type",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (typeField != null)
        {
            object value = typeField.GetValue(effectVisualMarker);

            if (value != null)
            {
                return value.ToString();
            }
        }

        FieldInfo typeFieldLower = effectVisualMarker.GetType().GetField(
            "type",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (typeFieldLower != null)
        {
            object value = typeFieldLower.GetValue(effectVisualMarker);

            if (value != null)
            {
                return value.ToString();
            }
        }

        PropertyInfo typeProperty = effectVisualMarker.GetType().GetProperty(
            "Type",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (typeProperty != null)
        {
            object value = typeProperty.GetValue(effectVisualMarker, null);

            if (value != null)
            {
                return value.ToString();
            }
        }

        PropertyInfo typePropertyLower = effectVisualMarker.GetType().GetProperty(
            "type",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (typePropertyLower != null)
        {
            object value = typePropertyLower.GetValue(effectVisualMarker, null);

            if (value != null)
            {
                return value.ToString();
            }
        }

        return "";
    }
}