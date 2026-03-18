using Domain.Enums.Documents;
using MudBlazor;

namespace LumiumPortal.Web.Helpers;

public static class DocumentHelpers
{
    public static Color GetCategoryColor(DocumentCategory category) => (int)category < 10
        ? Color.Info
        : Color.Success;
}