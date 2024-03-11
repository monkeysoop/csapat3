﻿using Mekkdonalds.Persistence;
using MekkdonaldsModel.Persistence;
using System.Drawing;
using MekkdonaldsModel.Simulation;
using System.Numerics;


namespace Mekkdonalds.Simulation.Controller;


internal sealed class AstarController : SimulationController
{
    public AstarController(List<Robot> r) : this(r, 1) { }

    public AstarController(List<Robot> r, double interval) : base(r, interval) { }



    private static void HeapInsert(Step[] heap, int length, Step item)
    {
        heap[length] = item;

        int index = length;
        int root_index = (index - 1) / 2;


        // <= is used (instead of <), because it makes the newer Step with same value prefered, 
        // which usually leads to finding the solution quicker.
        while (index > 0 && heap[index].heuristic <= heap[root_index].heuristic)
        {
            // swap
            Step t = heap[index];
            heap[index] = heap[root_index];
            heap[root_index] = t;

            index = root_index;
            root_index = (index - 1) / 2;
        }
    }

    private static Step HeapRemoveMin(Step[] heap, int length)
    {
        Step min_item = heap[0];
        heap[0] = heap[heap.Length - 1];
        length--; // this is only local!!!

        int index = 0;
        int left_child_index = 1;
        int right_child_index = 1;

        int next_child_index = left_child_index;
        if (right_child_index < length && heap[right_child_index].heuristic < heap[left_child_index].heuristic)
        {
            next_child_index = right_child_index;
        }

        while (left_child_index < length && heap[next_child_index].heuristic < heap[index].heuristic)
        {
            // swap
            Step t = heap[index];
            heap[index] = heap[next_child_index];
            heap[next_child_index] = t;

            index = next_child_index;
            left_child_index = 2 * index + 1;
            right_child_index = 2 * index + 2;

            next_child_index = left_child_index;
            if (right_child_index < length && heap[right_child_index].heuristic < heap[left_child_index].heuristic)
            {
                next_child_index = right_child_index;
            }
        }

        return min_item;
    }
    private static bool AstarPathFinder(Board2 board, Point start, int start_direction, Point end)
    {
        const int COST_BIAS = 1;
        const int HEURISTIC_BIAS = 1;
        Step[] heap = new Step[5 * board.height * board.width];


        int heap_length = 0;
        HeapInsert(heap, heap_length, new Step(start, start_direction, 0));
        heap_length++;

        int[] costs = new int[board.height * board.width]; // all items are automatically set to 0



        bool found = false;
        while (heap_length != 0 && !found)
        {
            Step current_step = HeapRemoveMin(heap, heap_length);
            heap_length--;
            board.SetSearched(current_step.position);

            if (ComparePoints(current_step.position, end))
            {
                found = true;
            } else
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

                int current_cost = costs[current_step.position.Y * board.width + current_step.position.X];

                int forward_cost = current_cost + 1;
                int left_cost = current_cost + 2;
                int right_cost = current_cost + 2;

                int forward_heuristic = COST_BIAS * forward_cost +
                                        HEURISTIC_BIAS * MaxTurnsRequired(forward_next_position, forward_offset, end) +
                                        HEURISTIC_BIAS * ManhattenDistance(forward_next_position, end);

                int left_heuristic = COST_BIAS * left_cost +
                                     HEURISTIC_BIAS * MaxTurnsRequired(left_next_position, left_offset, end) +
                                     HEURISTIC_BIAS * ManhattenDistance(left_next_position, end);

                int right_heuristic = COST_BIAS * right_cost +
                                      HEURISTIC_BIAS * MaxTurnsRequired(right_next_position, right_offset, end) +
                                      HEURISTIC_BIAS * ManhattenDistance(right_next_position, end);


                if (board.SetOpenIfEmpty(forward_next_position))
                {
                    costs[forward_next_position.Y * board.width + forward_next_position.X] = forward_cost;
                    HeapInsert(heap, heap_length, new Step(forward_next_position, forward_direction, forward_heuristic));
                    heap_length++;
                } else if (forward_cost < costs[forward_next_position.Y * board.width + forward_next_position.X])
                {
                    costs[forward_next_position.Y * board.width + forward_next_position.X] = forward_cost;
                }

                if (board.SetOpenIfEmpty(left_next_position))
                {
                    costs[left_next_position.Y * board.width + left_next_position.X] = left_cost;
                    HeapInsert(heap, heap_length, new Step(left_next_position, left_direction, left_heuristic));
                    heap_length++;
                }
                else if (left_cost < costs[left_next_position.Y * board.width + left_next_position.X])
                {
                    costs[left_next_position.Y * board.width + left_next_position.X] = left_cost;
                }

                if (board.SetOpenIfEmpty(right_next_position))
                {
                    costs[right_next_position.Y * board.width + right_next_position.X] = right_cost;
                    HeapInsert(heap, heap_length, new Step(right_next_position, right_direction, right_heuristic));
                    heap_length++;
                }
                else if (right_cost < costs[right_next_position.Y * board.width + right_next_position.X])
                {
                    costs[right_next_position.Y * board.width + right_next_position.X] = right_cost;
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

