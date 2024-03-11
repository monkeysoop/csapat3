using Mekkdonalds.Persistence;
using MekkdonaldsModel.Persistence;
using System.Drawing;
using MekkdonaldsModel.Simulation;
using System.Numerics;


namespace Mekkdonalds.Simulation.Controller;


internal sealed class BFSController : SimulationController
{
    public BFSController(List<Robot> r) : this(r, 1) { }

    public BFSController(List<Robot> r, double interval) : base(r, interval) { }


    static private bool BFSPathFinder(Board2 board, Point start, int start_direction, Point end)
    {
        Step[] queue = new Step[5 * board.height * board.width];


        queue[0] = new Step(start, start_direction, 0);
        int start_index = 0;
        int end_index = 1;


        int[] parents = new int[board.height * board.width]; // all items are automatically set to 0


        bool found = false;
        while (start_index != end_index && !found)
        {
            Step current_step = queue[start_index];
            start_index++;
            
            if (ComparePoints(current_step.position, end))
            {
                found = true;
            }
            else
            {
                int forward_direction = current_step.direction;
                int left_direction = (current_step.direction + 3) % 4;
                int right_direction = (current_step.direction + 1) % 4;

                Point forward_offset = nexts_offsets[forward_direction];
                Point left_offset = nexts_offsets[left_direction];
                Point right_offset = nexts_offsets[right_direction];

                Point forward_next_position = new Point(current_step.position.X + forward_offset.X,
                                                        current_step.position.Y + forward_offset.Y);
                Point left_next_position = new Point(current_step.position.X + left_offset.X,
                                                        current_step.position.Y + left_offset.Y);
                Point right_next_position = new Point(current_step.position.X + right_offset.X,
                                                        current_step.position.Y + right_offset.Y);

                if (board.SetSearchedIfEmpty(forward_next_position))
                {
                    queue[end_index] = new Step(forward_next_position, forward_direction, 0);
                    end_index++;
                    parents[forward_next_position.Y * board.width + forward_next_position.X] = forward_direction;
                }
                if (board.SetSearchedIfEmpty(left_next_position))
                {
                    queue[end_index] = new Step(left_next_position, left_direction, 0);
                    end_index++;
                    parents[left_next_position.Y * board.width + left_next_position.X] = left_direction;
                }
                if (board.SetSearchedIfEmpty(right_next_position))
                {
                    queue[end_index] = new Step(right_next_position, right_direction, 0);
                    end_index++;
                    parents[right_next_position.Y * board.width + right_next_position.X] = right_direction;
                }
            }
        }
        return found;
    }
    protected override Task CalculatePath(Robot robot)
    {
        throw new NotImplementedException();
    }
}
