using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace DotPosition.Windows
{
    public class Dot : Window
    {
        private readonly Plugin _plugin;
        public Dot(Plugin plugin) : base("Dot Position Overlay")
        {
            _plugin = plugin;
            Flags = ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoInputs;
        }

        public void Dispose() { }
        public override void Draw()
        {
            if (!_plugin.Configuration.IsPluginEnabled) return;

            var player = Plugin.ClientState.LocalPlayer;
            if (player == null) return;


            if (Plugin.GameGui.WorldToScreen(player.Position, out var screenPos)) 
            {
                var drawList = ImGui.GetBackgroundDrawList();
                drawList.AddCircleFilled(
                    new System.Numerics.Vector2(screenPos.X, screenPos.Y),
                    _plugin.Configuration.DotRadius,
                    ImGui.GetColorU32(_plugin.Configuration.DotColor)
                    );
            }
        }
    }
}
