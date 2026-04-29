using CEdefect.CEdefectCode.SkinManager;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Logging;

namespace CEdefect.CEdefectCode.Core;

[GlobalClass]
public partial class CE_SkinSelector : Control
{
   //private Label _skinNameLabel;
   private TextureButton _cyclebutton;

   public override void _Ready()
   {
	   //GD.Print("[ce] Ready()");
	  //_skinNameLabel = GetNode<Label>("SkinName");
	  _cyclebutton = GetNode<TextureButton>("SkinController/CycleButton");
	  if (_cyclebutton == null)
	  {
		  //GD.PrintErr("[ce] CycleButton not found!");
		  return;
	  }

	  _cyclebutton.Pressed += CycleSkin;

   }

   private void CycleSkin()
   {
	   //GD.Print("[ce] CycleSkin()");
	   SkinRegistry.CycleSkin();
	   SfxCmd.Play("event:/sfx/ui/clicks/ui_click");
	   RefreshCharacterSelectBg();
	   
   }
   

   private void RefreshCharacterSelectBg()
   {
	  Node? parent = GetParent();
	  if (parent == null) return;

	  PackedScene? scene = SkinRegistry.ResolveScene(s => s.CharacterSelectBg);
	  if (scene == null)
	  {
		 Log.Error("CharacterSelectBg is null");
		 return;
	  }

	  Node newScene = scene.Instantiate();
	  parent.RemoveChild(this);
	  parent.AddChild(newScene);
	  QueueFree();
   }
   
}
