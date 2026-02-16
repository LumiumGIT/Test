using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Shared;

public partial class ComboBoxColumnFilter<T> : ComponentBase
{
    [Parameter, EditorRequired] public FilterContext<T> Context { get; set; } = null!;
    [Parameter, EditorRequired] public Func<T, string> ValueSelector { get; set; } = null!;
    [Parameter] public int PopoverWidth { get; set; } = 200;

    private bool _isInitialized;
    private bool _isOpen;
    private bool _selectAll = true;
    private string _currentIcon = Icons.Material.Outlined.FilterAlt;
    private HashSet<string> _availableValues = [];
    private HashSet<string> _selectedValues = [];
    private FilterDefinition<T> _filterDefinition = null!;

    protected override void OnParametersSet()
    {
        if (Context?.Items != null && Context.Items.Any())
        {
            InitializeFilter();
        }
    }

    private void InitializeFilter()
    {
        if (_isInitialized)
        {
            return;
        }
        
        _availableValues = Context.Items
            .Select(ValueSelector)
            .Where(v => !string.IsNullOrEmpty(v))
            .Distinct()
            .ToHashSet();

        _selectedValues = _availableValues.ToHashSet();

        _filterDefinition = new FilterDefinition<T>
        {
            FilterFunction = item => _selectedValues.Contains(ValueSelector(item))
        };

        _isInitialized = true;
    }

    private void OpenFilter()
    {
        _isOpen = true;
    }

    private void CloseFilter()
    {
        _isOpen = false;
    }

    private void HandleValueChanged(bool isChecked, string value)
    {
        if (isChecked)
        {
            _selectedValues.Add(value);
        }
        else
        {
            _selectedValues.Remove(value);
        }

        _selectAll = _selectedValues.Count == _availableValues.Count;
    }

    private void HandleSelectAll(bool value)
    {
        _selectAll = value;

        if (value)
        {
            _selectedValues = _availableValues.ToHashSet();
        }
        else
        {
            _selectedValues.Clear();
        }
    }

    private async Task HandleClear()
    {
        _selectedValues = _availableValues.ToHashSet();
        _selectAll = true;
        _currentIcon = Icons.Material.Outlined.FilterAlt;
        await Context.Actions.ClearFilterAsync(_filterDefinition);
        _isOpen = false;
    }

    private async Task HandleApply()
    {
        _currentIcon = _selectedValues.Count == _availableValues.Count
            ? Icons.Material.Outlined.FilterAlt
            : Icons.Material.Filled.FilterAlt;

        await Context.Actions.ApplyFilterAsync(_filterDefinition);
        _isOpen = false;
    }
}