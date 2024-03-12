namespace MekkdonaldsModel.Simulation
{
    internal readonly struct Step
    {
        public Point position { get; init; }
        public int direction { get; init; }
        public int heuristic { get; init; }

        public Step(Point position, int direction, int heuristic)
        {
            this.position = position;
            this.direction = direction;
            this.heuristic = heuristic;
        }
    }
}
