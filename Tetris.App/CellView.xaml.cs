using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisApp;

public enum CellState
{
    Empty,
    Static,
    Moving,
}

public partial class CellView : ContentView
{
    public static readonly BindableProperty StateProperty = BindableProperty.Create(nameof(State), typeof(CellState), typeof(CellView), default(CellState));

    public CellState State
    {
        get => (CellState)GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    public CellView()
    {
        InitializeComponent();
    }
}
