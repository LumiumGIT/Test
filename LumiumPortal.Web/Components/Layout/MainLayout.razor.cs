using Microsoft.JSInterop;
using MudBlazor;

namespace LumiumPortal.Web.Components.Layout;

public partial class MainLayout()
{
    private MudThemeProvider? _mudThemeProvider;
    private bool _isDarkMode = true;
    private bool _drawerOpen = true;

    private readonly MudTheme _customTheme = new()
    {
        PaletteLight = new PaletteLight
        {
            // Primary colors
            Primary = Colors.Blue.Darken3,
            Secondary = Colors.Cyan.Darken2,
            Tertiary = Colors.BlueGray.Lighten1,

            // Backgrounds
            Background = Colors.BlueGray.Lighten5,
            Surface = Colors.Shades.White,
            AppbarBackground = Colors.Blue.Darken3,
            DrawerBackground = Colors.BlueGray.Lighten4,

            // Text
            TextPrimary = Colors.Shades.Black,
            TextSecondary = Colors.BlueGray.Darken2,
            TextDisabled = Colors.BlueGray.Lighten1,

            // Actions
            ActionDefault = Colors.Cyan.Darken1,
            ActionDisabled = Colors.BlueGray.Lighten2,
            ActionDisabledBackground = Colors.BlueGray.Lighten4,

            // Status colors
            Success = Colors.Green.Darken1,
            Warning = Colors.Amber.Darken2,
            Error = Colors.Red.Darken1,
            Info = Colors.LightBlue.Darken1,

            // Dividers
            Divider = Colors.BlueGray.Lighten3,
            DividerLight = Colors.BlueGray.Lighten4,

            // Tables
            TableLines = Colors.BlueGray.Lighten3,
            TableStriped = Colors.BlueGray.Lighten5,
            TableHover = Colors.LightBlue.Lighten5,

            // Overlay
            OverlayDark = Colors.BlueGray.Darken4,
            OverlayLight = Colors.BlueGray.Lighten5,
        },
        PaletteDark = new PaletteDark
        {
            // Primary colors
            Primary = Colors.Cyan.Default,
            Secondary = Colors.Cyan.Darken1,
            Tertiary = Colors.BlueGray.Default,

            // Backgrounds
            Background = "#111318", 
            Surface = "#1C1F26",     
            AppbarBackground = "#13161C",
            DrawerBackground = "#13161C", 

            // Text
            TextPrimary = Colors.Shades.White,
            TextSecondary = Colors.BlueGray.Lighten2,
            TextDisabled = Colors.BlueGray.Default,

            // Actions
            ActionDefault = Colors.Cyan.Accent2,
            ActionDisabled = Colors.BlueGray.Darken1,
            ActionDisabledBackground = Colors.Gray.Darken3,

            // Status colors
            Success = Colors.Green.Darken1,
            Warning = Colors.Orange.Darken1,
            Error = Colors.Red.Lighten1,
            Info = Colors.LightBlue.Accent2,

            // Dividers
            Divider = Colors.BlueGray.Darken2,
            DividerLight = Colors.BlueGray.Darken3,

            // Tables
            TableLines = Colors.BlueGray.Darken2,
            TableStriped = Colors.BlueGray.Darken3,
            TableHover = Colors.Cyan.Darken4,

            // Overlay
            OverlayDark = Colors.Shades.Black,
            OverlayLight = Colors.Gray.Darken3,
        }
    };

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var stored = await JsRuntime.InvokeAsync<string?>("localStorage.getItem", "lumium-theme-dark");

            if (stored is null)
            {
                if (_mudThemeProvider is not null)
                {
                    _isDarkMode = await _mudThemeProvider.GetSystemDarkModeAsync();
                }
            }
            else
            {
                _isDarkMode = stored == "true";
            }

            StateHasChanged();
        }
    }

    private void ToggleDrawer() => _drawerOpen = !_drawerOpen;

    private async Task ToggleTheme()
    {
        _isDarkMode = !_isDarkMode;
        await JsRuntime.InvokeVoidAsync("localStorage.setItem", "lumium-theme-dark", _isDarkMode.ToString().ToLower());
    }
}