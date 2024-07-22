using Content.Client.Message;
using Content.Client.Resources; // Vanilla-edit
using Content.Client.UserInterface.Systems.EscapeMenu;
using Robust.Client.AutoGenerated;
using Robust.Client.Console;
using Robust.Client.Graphics; // Vanilla-edit
using Robust.Client.ResourceManagement; // Vanilla-edit
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Lobby.UI
{
    [GenerateTypedNameReferences]
    public sealed partial class LobbyGui : UIScreen
    {
        [Dependency] private readonly IClientConsoleHost _consoleHost = default!;
        [Dependency] private readonly IResourceCache _resourceCache = default!; // Vanilla-edit
        private readonly StyleBoxTexture _back; // Vanilla-edit

        public LobbyGui()
        {
            RobustXamlLoader.Load(this);
            IoCManager.InjectDependencies(this);
            SetAnchorPreset(MainContainer, LayoutPreset.Wide);
            SetAnchorPreset(Background, LayoutPreset.Wide);

            LobbySong.SetMarkup(Loc.GetString("lobby-state-song-no-song-text"));

            LeaveButton.OnPressed += _ => _consoleHost.ExecuteCommand("disconnect");
            OptionsButton.OnPressed += _ => UserInterfaceManager.GetUIController<OptionsUIController>().ToggleWindow();

            // Vanilla-start
            var panelTex = _resourceCache.GetTexture("/Textures/Interface/Nano/button.svg.96dpi.png");
            _back = new StyleBoxTexture
            {
                Texture = panelTex,
                Modulate = new Color(37, 37, 42),
            };
            _back.SetPatchMargin(StyleBox.Margin.All, 10);

            LeftSideTop.PanelOverride = _back;

            RightSide.PanelOverride = _back;

            LobbySongPanel.PanelOverride = _back;

            LeftBottomPanel.PanelOverride = _back;

            SetLobbyOpacity();
            // Vanilla-end
        }

        public void SwitchState(LobbyGuiState state)
        {
            DefaultState.Visible = false;
            CharacterSetupState.Visible = false;

            switch (state)
            {
                case LobbyGuiState.Default:
                    DefaultState.Visible = true;
                    RightSide.Visible = true;
                    break;
                case LobbyGuiState.CharacterSetup:
                    CharacterSetupState.Visible = true;

                    var actualWidth = (float) UserInterfaceManager.RootControl.PixelWidth;
                    var setupWidth = (float) LeftSide.PixelWidth;

                    if (1 - (setupWidth / actualWidth) > 0.30)
                    {
                        RightSide.Visible = false;
                    }

                    UserInterfaceManager.GetUIController<LobbyUIController>().ReloadCharacterSetup();

                    break;
            }
        }

        // Vanilla-start
        private void SetLobbyOpacity()
        {
            _back.Modulate = new Color(37, 37, 42).WithAlpha(0.93f);
        }
        // Vanilla-end

        public enum LobbyGuiState : byte
        {
            /// <summary>
            ///  The default state, i.e., what's seen on launch.
            /// </summary>
            Default,
            /// <summary>
            ///  The character setup state.
            /// </summary>
            CharacterSetup
        }
    }
}
