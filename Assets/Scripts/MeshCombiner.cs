using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[RequireComponent(typeof(MeshFilter))]
//[RequireComponent(typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
    public void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(false);
        List<Material> materials = new List<Material>();
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(false);
        MeshFilter thisMeshFilter = GetComponent<MeshFilter>();
        MeshRenderer thisMeshRenderer = GetComponent<MeshRenderer>();
        MeshCollider thisMeshCollider = GetComponent<MeshCollider>();
        Vector3 thisPosition = transform.position;

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            if (meshRenderer.transform == transform)
            {
                continue;
            }

            Material[] localMats = meshRenderer.sharedMaterials;
            foreach (Material localMat in localMats)
            {
                if (!materials.Contains(localMat))
                {
                    materials.Add(localMat);
                }
            }
        }

        List<Mesh> subMeshes = new List<Mesh>();
        foreach (Material material in materials)
        {
            List<CombineInstance> combines = new List<CombineInstance>();
            foreach (MeshFilter meshFilter in meshFilters)
            {
                MeshRenderer meshRenderer = meshFilter.GetComponent<MeshRenderer>();
                if (meshRenderer == null)
                {
                    Debug.LogError(meshFilter.name + " has no MeshRenderer");
                    continue;
                }
                Material[] localMaterials = meshRenderer.sharedMaterials;
                for (int materialIndex = 0; materialIndex < localMaterials.Length; materialIndex++)
                {
                    if (localMaterials[materialIndex] != material)
                    {
                        continue;
                    }
                    CombineInstance combineInstance = new CombineInstance();
                    combineInstance.mesh = meshFilter.sharedMesh;
                    combineInstance.subMeshIndex = materialIndex;
                    combineInstance.transform = meshFilter.transform.localToWorldMatrix;
                    combines.Add(combineInstance);
                }
            }

            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combines.ToArray(), true);
            subMeshes.Add(mesh);
        }

        List<CombineInstance> finalCombines = new List<CombineInstance>();
        foreach (Mesh mesh in subMeshes)
        {
            CombineInstance combineInstance = new CombineInstance();
            combineInstance.mesh = mesh;
            combineInstance.subMeshIndex = 0;
            combineInstance.transform = Matrix4x4.identity;
            finalCombines.Add(combineInstance);
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(finalCombines.ToArray(), false);
        thisMeshFilter.sharedMesh = finalMesh;
        transform.position = thisPosition;
        Debug.Log("Final mesh has " + subMeshes.Count + " materials");
        thisMeshCollider.sharedMesh = transform.GetComponent<MeshFilter>().sharedMesh;
    }
}


