using Microsoft.Extensions.Localization;
using MudBlazor;

namespace LumiumPortal.Web.Localization;

public class SrpskiLocalizationInterceptor : ILocalizationInterceptor
{
    private readonly Dictionary<string, string> _translations = new()
    {
        { "MudDataGrid_FilterValue", "Traži..." },
        { "MudDataGrid_ClearFilter", "Obriši filter" },
        { "MudDataGrid_Sort", "Sortiraj" },
        { "MudDataGrid_ShowColumnOptions", "Opcije kolone" },
        { "MudDataGridPager_FirstPage", "Prva strana" },
        { "MudDataGridPager_PreviousPage", "Prethodna strana" },
        { "MudDataGridPager_NextPage", "Sledeća strana" },
        { "MudDataGridPager_LastPage", "Poslednja strana" },
        { "MudDataGrid_True", "Da" },
        { "MudDataGrid_False", "Ne" },
        { "MudDataGrid_Contains", "Sadrži" },
        { "MudDataGrid_NotContains", "Ne sadrži" },
        { "MudDataGrid_Equals", "Jednako" },
        { "MudDataGrid_NotEquals", "Nije jednako" },
        { "MudDataGrid_StartsWith", "Počinje sa" },
        { "MudDataGrid_EndsWith", "Završava sa" },
        { "MudDataGrid_IsEmpty", "Prazno" },
        { "MudDataGrid_IsNotEmpty", "Nije prazno" },
        { "MudDataGrid_EqualSign", "Jednako" },
        { "MudDataGrid_NotEqualSign", "Nije jednako" },
        { "MudDataGrid_GreaterThanSign", "Veće od" },
        { "MudDataGrid_GreaterThanOrEqualSign", "Veće ili jednako od" },
        { "MudDataGrid_LessThanSign", "Manje od" },
        { "MudDataGrid_LessThanOrEqualSign", "Manje ili jednako od" },
        { "MudDataGrid_Unsort", "Ukloni sortiranje" },
        { "MudDataGrid_SortAscending", "Rastuće" },
        { "MudDataGrid_SortDescending", "Opadajuće" },
        { "MudDataGrid_Operator", "Operator" },
        { "MudDataGrid_Value", "Vrednost" },
        { "MudDataGrid_Column", "Kolona" },
        { "MudDataGrid_Columns", "Kolone" },
        { "MudDataGrid_Hide", "Sakrij" },
        { "MudDataGrid_HideAll", "Sakrij sve" },
        { "MudDataGrid_ShowAll", "Prikaži sve" },
        { "MudDataGrid_Group", "Grupiši" },
        { "MudDataGrid_Ungroup", "Razgrupiši" },
        { "MudDataGrid_CollapseAllGroups", "Skupi sve" },
        { "MudDataGrid_ExpandAllGroups", "Proširi sve" },
        { "MudDataGrid_RefreshData", "Osveži" },
    };

    public LocalizedString Handle(string key, params object[] arguments)
    {
        return _translations.TryGetValue(key, out var value)
            ? new LocalizedString(key, value, false)
            : new LocalizedString(key, key, true);
    }
}