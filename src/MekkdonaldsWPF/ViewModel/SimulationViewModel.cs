namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    public SimulationViewModel() : base([new(1, 0, 0), new(2, 10, 25), new(3, 2, 2)], [new(1,1), new(1,3)])
    {
        Size = (20, 40);
    }
}
