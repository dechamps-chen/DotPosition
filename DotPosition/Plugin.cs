using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using DotPosition.Windows;

namespace DotPosition;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IGameGui GameGui { get; private set; } = null!;

    private const string CommandName = "/dotpos";

    public Configuration Configuration { get; init; }
    public readonly WindowSystem WindowSystem = new("DotPosition");
    private MainWindow MainWindow { get; init; }
    private Dot Dot { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        MainWindow = new MainWindow(this);
        Dot = new Dot(this);

        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Open Dot Position Configuration\n" +
                         "/dotpos on - Enable plugin\n" +
                         "/dotpos off - Disable plugin\n" +
                         "/dotpos toggle - Toggle visibility"
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;
        PluginInterface.UiBuilder.OpenConfigUi += ToggleMainUI;
        PluginInterface.UiBuilder.Draw += DrawDot;
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= DrawUI;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUI;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleMainUI;
        PluginInterface.UiBuilder.Draw -= DrawDot;

        WindowSystem.RemoveAllWindows();
        MainWindow.Dispose();
        Dot.Dispose();
        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        switch (args.ToLower())
        {
            case "on":
                Configuration.IsPluginEnabled = true;
                PluginInterface.SavePluginConfig(Configuration);
                break;
            case "off":
                Configuration.IsPluginEnabled = false;
                PluginInterface.SavePluginConfig(Configuration);
                break;
            case "toggle":
                Configuration.IsPluginEnabled = !Configuration.IsPluginEnabled;
                PluginInterface.SavePluginConfig(Configuration);
                break;
            default:
                ToggleMainUI();
                break;
        }
    }

    private void DrawUI() => WindowSystem.Draw();
    public void ToggleMainUI() => MainWindow.Toggle();
    private void DrawDot() => Dot.Draw();
}
