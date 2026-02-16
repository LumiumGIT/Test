using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Shared;

public partial class EnumColumnFilter<TItem, TEnum> : ComponentBase where TEnum : struct, Enum 
{
   [Parameter, EditorRequired] public FilterContext<TItem> Context { get; set; } = null!;
    [Parameter, EditorRequired] public Func<TItem, TEnum> ValueSelector { get; set; } = null!;
    [Parameter] public int PopoverWidth { get; set; } = 150;

    private bool _isInitialized;
    private bool _isOpen;
    private bool _selectAll = true;
    private string _currentIcon = Icons.Material.Outlined.FilterAlt;
    private HashSet<TEnum> _selectedValues = [];
    private FilterDefinition<TItem> _filterDefinition = null!;

    protected override void OnParametersSet()
    {
        if (Context?.Items != null && Context.Items.Any())
        {
            InitializeFilter();
        }
    }

    private void InitializeFilter()
    {
        if (_isInitialized) return;

        _selectedValues = Enum.GetValues<TEnum>().ToHashSet();

        _filterDefinition = new FilterDefinition<TItem>
        {
            FilterFunction = item => _selectedValues.Contains(ValueSelector(item))
        };

        _isInitialized = true;
    }

    private void OpenFilter() => _isOpen = true;
    private void CloseFilter() => _isOpen = false;

    private void HandleValueChanged(bool isChecked, TEnum value)
    {
        if (isChecked)
        {
            _selectedValues.Add(value);
        }
        else
        {
            _selectedValues.Remove(value);
        }

        _selectAll = _selectedValues.Count == Enum.GetValues<TEnum>().Length;
    }

    private void HandleSelectAll(bool value)
    {
        _selectAll = value;

        if (value)
        {
            _selectedValues = Enum.GetValues<TEnum>().ToHashSet();
        }
        else
        {
            _selectedValues.Clear();
        }
    }

    private async Task HandleClear()
    {
        _selectedValues = Enum.GetValues<TEnum>().ToHashSet();
        _selectAll = true;
        _currentIcon = Icons.Material.Outlined.FilterAlt;
        await Context.Actions.ClearFilterAsync(_filterDefinition);
        _isOpen = false;
    }

    private async Task HandleApply()
    {
        _currentIcon = _selectedValues.Count == Enum.GetValues<TEnum>().Length
            ? Icons.Material.Outlined.FilterAlt
            : Icons.Material.Filled.FilterAlt;

        await Context.Actions.ApplyFilterAsync(_filterDefinition);
        _isOpen = false;
    }
}