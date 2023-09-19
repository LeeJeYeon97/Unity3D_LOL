using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOutline : MonoBehaviour
{
    Material outline;
    public Shader outlineShader;

    public Renderer renderers;
    List<Material> materialList = new List<Material>();

    private void OnMouseOver()
    {
        //renderers = this.GetComponent<Renderer>();

        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        if(materialList.Contains(outline) == false)
            materialList.Add(outline);

        renderers.materials = materialList.ToArray();
    }
    private void OnMouseDown()
    {
        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        if (materialList.Contains(outline) == false)
            materialList.Add(outline);

        renderers.materials = materialList.ToArray();
    }
    private void OnMouseUp()
    {
        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        materialList.Remove(outline);

        renderers.materials = materialList.ToArray();
    }

    private void OnMouseExit()
    {
        materialList.Clear();
        materialList.AddRange(renderers.sharedMaterials);
        materialList.Remove(outline);

        renderers.materials = materialList.ToArray();
    }

    void Start()
    {
        outline = new Material(outlineShader);
    }
}
