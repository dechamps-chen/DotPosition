using Dalamud.Configuration;
using System;
using System.Numerics;

namespace DotPosition;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    public bool IsPluginEnabled { get; set; } = false;
    public Vector4 DotColor { get; set; } = new(1f, 1f, 0f, 1f);
    public float DotRadius { get; set; } = 2f;

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
