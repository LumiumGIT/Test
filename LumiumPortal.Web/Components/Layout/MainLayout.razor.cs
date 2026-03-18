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
            Primary = Colors.Blue.Darken3,
            Secondary = "#0097A7",      // dublja nijansa cyan za kontrast
            Background = "#ECEFF1",     // hladna siva, ne bela
            Surface = "#FFFFFF",        // za kartice i komponente
            AppbarBackground = Colors.Blue.Darken3, // tamniji cyan za AppBar
            DrawerBackground = "#CFD8DC",  // sivo-plava nijansa za meni
            TextPrimary = "#0D0D0D",    // gotovo crna
            TextSecondary = "#37474F",  // tamnosiva s blagim plavim tonom
            ActionDefault = "#00ACC1",  // hover/active akcije u tirkiznoj
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#00E5FF",        // jak neon-cyan kao u logou
            Secondary = "#4DD0E1",      // mekša varijanta
            Background = "#0D0D0D",     // gotovo crna pozadina (kao logo)
            Surface = "#121212",        // tamnija površina
            AppbarBackground = "#000000", // čista crna traka gore
            TextPrimary = "#FFFFFF",    // beli tekst
            TextSecondary = "#B0BEC5"
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
                    _isDarkMode = await _mudThemeProvider.GetSystemDarkModeAsync();
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