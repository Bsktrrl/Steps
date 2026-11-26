using UnityEngine;

internal static class TerrainRaycasts
{
    public static int Cast(
        Vector3 origin, Vector3 direction, float distance, int layers,
        QueryTriggerInteraction triggers, ref RaycastHit[] hits)
    {
        hits ??= new RaycastHit[8];

        while (true)
        {
            int count = Physics.RaycastNonAlloc(origin, direction, hits, distance, layers, triggers);
            if (count < hits.Length)
                return count;

            // A full buffer may omit the nearest valid support.
            hits = new RaycastHit[hits.Length * 2];
        }
    }
}
