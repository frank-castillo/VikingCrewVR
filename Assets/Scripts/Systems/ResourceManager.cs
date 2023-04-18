using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // private static readonly string IconPath = "/Art/Icons";

    public ResourceManager Initialize()
    {
        Debug.Log($"<color=Lime> Initializing {this.GetType()} ... </color>");

        return this;
    }
}
