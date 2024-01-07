
namespace UnlockDleks.Modules;

public static class GameStates
{
    public static bool IsInGame = false;
    public static bool introDestroyed = false;
    public static bool IsMeeting => AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Joined && MeetingHud.Instance;
    public static bool DleksIsActive => (MapNames)GameOptionsManager.Instance.CurrentGameOptions.MapId == MapNames.Dleks;
}