using System.Reflection;
using Godot;
using Godot.Collections;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;

namespace CEdefect.CEdefectCode.Core;

[GlobalClass]
public partial class CEParticlesContainer : NParticlesContainer
{
	public void SetParticlesFromChildren()
	{
		Array<GpuParticles2D> particles = new Array<GpuParticles2D>();
		foreach (Node child in GetChildren())
		{
			if (child is GpuParticles2D p)
				particles.Add(p);
		}
		// use reflection once here internally
		typeof(NParticlesContainer)
			.GetField("_particles", BindingFlags.Instance | BindingFlags.NonPublic)
			?.SetValue(this, particles);
	}
}
