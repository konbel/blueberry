using Godot;

[GlobalClass, Tool]
public partial class NoiseLayer : Resource {
    [Export] public bool Enabled = true;
    [Export] public bool UseFirstLayerAsMask = false;
    [Export] public NoiseSettings3D NoiseSettings;
}
