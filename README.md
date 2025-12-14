# Blueberry

A collection of utilities for C# Godot projects.

## Overview

Blueberry provides a set of utilities and tools to enhance your Godot development experience when using C#. This library focuses on providing robust, type-safe functionality that leverages C#'s advanced features.

## Requirements

- **Godot 4.x** with .NET support
- **C# / .NET**

## GDScript Support

**GDScript is not supported** as of the current release of Godot. This is because GDScript lacks several language features that are essential for this library's functionality, including:

- C# constructors with parameters
- Parameter overloading
- Other advanced type system features

## Features

### State Machine

A node-based state machine system for managing game states and transitions. Features include:

- **Easy Setup**: Add states as child nodes of a StateMachine node
- **Lifecycle Management**: Automatic calling of Entry, Process, PhysicsProcess, and Exit methods
- **Pause Support**: Pause and resume state execution
- **State Switching**: Switch states by name, index, or reference
- **Signal Support**: StateChanged signal emitted on transitions
- **Input Handling**: Automatic input forwarding to the current state

Create custom states by inheriting from the `State` class and override the lifecycle methods you need.

### Noise Generation

Flexible noise generation system supporting 1D, 2D, and 3D noise with customizable settings and filters.

## Installation

1. Copy the `addons/blueberry` folder into your Godot project's `addons/` directory
2. Enable the plugin in **Project → Project Settings → Plugins**

## License

This project is licensed under the MIT License - see the [LICENSE](LISCENCE) file for details.
