﻿using Mekkdonalds.Simulation.Assigner;

namespace Mekkdonalds.ViewModel;

/// <summary>
/// ViewModel for the simulation view using an instance of <see cref="SimulationController"/>."/>
/// </summary>
internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController _simulationController;

    public ICommand LogSave { get; }
    public event EventHandler? Ended;

    public SimulationViewModel(string path, Type pathfinder, double speed, int length) : base(new SimulationController(path, SimDataAccess.Instance, pathfinder,  speed, length))
    {
        if (Controller is not SimulationController controller)
        {
            throw new ArgumentException("Controller is not a SimulationController");
        }
        else
        {
            _simulationController = controller;
            _simulationController.Ended += (_, _) => Ended?.Invoke(this, EventArgs.Empty);
        }

        _simulationController.Loaded += (_, _) => OnLoaded(this);
        _simulationController.Tick += (_, _) => OnTick(this);

        LogSave = new DelegateCommand(_ => _simulationController.SaveLog());
    }

    public void AssignTask(Robot selectedRobot, int x, int y)
    {
        _simulationController.Assign(selectedRobot, new(x, y));
    }

    public void Dispose()
    {
        _simulationController.Dispose();
    }
}
