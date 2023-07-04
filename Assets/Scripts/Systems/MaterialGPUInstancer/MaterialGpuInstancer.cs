using System.Collections.Generic;
using UnityEngine;

// Copyright <2023> <William Dean Clark>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

/// <summary>
/// Enables/Disables GPU Instancing to guarantee the materials in your scene are properly set up.
/// </summary>

public struct MaterialGpuInstancerKey
{
    public MaterialGpuInstancerKey(Mesh m, Material mat)
    {
        mesh = m;
        material = mat;
    }

    public Mesh mesh;
    public Material material;
}

public class MaterialGpuInstancer : MonoBehaviour
{
    [SerializeField] private uint _minimumMeshDuplicationAmount = 10; // The amount of times a mesh must be duplicated in a scene to enable gpu instancing.
    private Dictionary<MaterialGpuInstancerKey, int> _instances = new Dictionary<MaterialGpuInstancerKey, int>();

    public void UpdateMaterialGpuInstancing()
    {
        PopulateDictionary();
        List<Material> materials = new List<Material>();

        // Enable or disable gpu instancing
        foreach (KeyValuePair<MaterialGpuInstancerKey, int> pair in _instances)
        {
            MaterialGpuInstancerKey key = pair.Key;
            int value = pair.Value;
            Material mat = key.material;

            // Check to see if material has been checked already
            bool matExist = false;
            foreach (var m in materials)
            {
                if (m == mat && m.enableInstancing == false)
                {
                    if (value >= _minimumMeshDuplicationAmount)
                    {
                        mat.enableInstancing = true;
                        LogMaterialUpdate(mat.name, mat.enableInstancing, value);
                    }

                    matExist = true;
                    break;
                }
            }

            // Early return
            if (matExist == true)
            {
                continue;
            }

            // Check, update and add material
            bool enableInstacning = value >= _minimumMeshDuplicationAmount;
            if (mat.enableInstancing != enableInstacning)
            {
                mat.enableInstancing = enableInstacning;
                materials.Add(mat);
                LogMaterialUpdate(mat.name, mat.enableInstancing, value);
            }
        }

        _instances.Clear();
        Debug.Log("Finished Updating GPU Instancing on Materials");
    }

    private void PopulateDictionary()
    {
        _instances.Clear();
        object[] gameObjects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (object obj in gameObjects)
        {
            GameObject g = (GameObject)obj;

            MeshRenderer renderer = g.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                continue;
            }

            Material material = renderer.sharedMaterial;
            // TODO: Check if the material even supports GPU Instacning because if not then whats the point.

            MeshFilter meshFilter = g.GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                continue;
            }
            Mesh mesh = meshFilter.sharedMesh;

            // Populate dictionary
            MaterialGpuInstancerKey key = new MaterialGpuInstancerKey(mesh, material);
            if (_instances.TryGetValue(key, out int count)) // Increase the count value
            {
                count++;
                _instances[key] = count;
            }
            else // Doesn't exist in the dictionary, add it with count = 1
            {
                _instances.Add(key, 1);
            }
        }
    }

    private void LogMaterialUpdate(string name, bool enabledInstancing, int value)
    {
        string color = enabledInstancing ? "00FF00" : "FF0000"; // Green or red
        Debug.Log($"Material: {name}, GPU Instancing was set to <color=#{color}>{enabledInstancing}</color>. Mesh Instances = {value} / {_minimumMeshDuplicationAmount}");
    }
}
