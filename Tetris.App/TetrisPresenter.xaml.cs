﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;
using Tetris.CommonLib;

namespace TetrisApp;

public partial class TetrisPresenter : ContentView
{
    private const int CellSize = 20;

    public bool[,]? CurrentState { get; set; }

    public PositionedShape? CurrentShape { get; set; }

    private GameController _gameController;
    private CellView[,] _cells;

    private void GameControllerOnCurrentShapeUpdated(PositionedShape obj)
    {
        CurrentShape = obj;

        Refresh();
    }

    public TetrisPresenter()
    {
        InitializeComponent();
    }

    public void SetGameController(GameController controller)
    {
        _gameController = controller;

        CurrentState = controller.GameField.CurrentStaticState.AsArray();

        // create base grid
        var grid = new Grid
        {
            RowDefinitions = [],
            ColumnDefinitions = [],
        };

        var width = _gameController.GameField.CurrentStaticState.Width;
        var height = _gameController.GameField.CurrentStaticState.Height;

        _cells = new CellView[width, height];

        for (var row = 0; row < height; row++)
        {
            grid.RowDefinitions.Add(new RowDefinition(height: CellSize));
        }

        for (var col = 0; col < width; col++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition(width: CellSize));
        }


        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var cell = _cells[col, row] = new CellView()
                {
                    State = CellState.Static,
                };

                grid.Add(cell, col, row);
            }
        }

        this.Content = grid;

        // subscribe to events leading to refresh view
        _gameController.GameStateUpdated += GameControllerOnGameStateUpdated;
        _gameController.CurrentShapeUpdated += GameControllerOnCurrentShapeUpdated;
    }

    private void GameControllerOnGameStateUpdated(bool[,] obj)
    {
        CurrentState = obj;

        Refresh();
    }

    private void Refresh()
    {
        if (CurrentState is null)
            return;

        // draw state and current shape

        for (var row = 0; row < CurrentState.GetLength(1); row++)
        {
            for (var col = 0; col < CurrentState.GetLength(0); col++)
            {
                var state = CellState.Empty;
                state = CurrentState[col, row] ? CellState.Static : CellState.Empty;

                if (CurrentShape != null)
                {
                    var x0 = CurrentShape.Position.X;
                    var y0 = CurrentShape.Position.Y;

                    if (col >= x0 && col < x0 + CurrentShape.Shape.Width)
                    {
                        if (row >= y0 && row < y0 + CurrentShape.Shape.Height)
                        {
                            var shapeCol = col - x0;
                            var shapeRow = row - y0;

                            if (CurrentShape.Shape[shapeCol, shapeRow])
                            {
                                state = CellState.Moving;
                            }
                        }
                    }

                    _cells[col, row].State = state;
                }
            }
        }
    }
}
