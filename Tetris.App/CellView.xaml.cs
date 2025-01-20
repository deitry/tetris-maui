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
    private CellState _state;

    public CellState State
    {
        get => _state;
        set
        {
            if (value == _state) return;

            _state = value;
            OnPropertyChanged();

            App.RunInUiContext(_state, state =>
            {
                BackgroundColor = state switch
                {
                    CellState.Moving => Colors.Red,
                    CellState.Static => Colors.Gold,
                    _ => Colors.White,
                };
            });
        }
    }

    public CellView()
    {
        InitializeComponent();
    }
}
