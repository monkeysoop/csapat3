# MekkDonalds

[Scrum report](https://docs.google.com/document/d/1WuZMPSuczGhm5r1XOD4gtDwlU9mPLK-HyWbvd_U-gJ0/edit?usp=sharing)

[Documentation](https://szofttech-ab-2024.szofttech.gitlab-pages.hu/group-07/csapat3)

## Installation

.NET 8.0 needed

```
dotnet build
```

## Usage

### Simulation

![Simulation demo](/docs/images/usage/Simulation.gif)

There are a few options you can set in the main menu:
 - Number of steps: how many steps to simulate, defaults to -1 (all steps)
 - Interval: how much time the algorithm has to calculate a step, defaults to 0.2s
 - Select an algorithm (currently there are 3 algorithms: A*, BFS, DFS; each of them is cooperative)

Then click on the 'Open Simulation' button and you will be asked to select a configuration file.

Inside the simulation you have several options:
- Bottom left (from left to right): start/resume simulation (also toggleable with spacebar); jump one step forward
- Bottom right: simulation speed changer
- Middle right: you can save the current log at any time
- Middle is the map of the warehouse with numbered robots and packages; you can zoom in with Ctrl+MouseWheel (or TouchPad)

When the simulation ends, a log file is automatically created and the program exists.

### Replay

![Replay demo](/docs/images/usage/Replay.gif)

There are no options in the main menu for the replay function, you just click on the 'Open Replay' button.

You will be asked to select a log file (the end result of the simulation) and then a map file.
MAKE SURE THAT THE MAP FILE IS THE WAREHOUSE LAYOUT FROM WHICH THE SIMULATION WAS RUN.

The replay function will then load the log file and visualize the robot movements and package deliveries according to the simulation steps.
Within the replay function you have several options:
- Bottom left (from left to right): step back; start/resume simulation (also toggleable with spacebar); step forward
- Bottom middle: Progress bar, where you can grab the current position and move it to any point in time.
- Bottom right (from bottom to top): current step / all steps (you can change the current step to jump to a specific step in the replay); speed changer.
- Middle: map of the warehouse with numbered robots and packages; you can zoom in with Ctrl+MouseWheel (or TouchPad)


## Contributing

Merge requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License

[![MIT License](https://img.shields.io/badge/License-MIT-yellow.svg)](/LICENSE)
