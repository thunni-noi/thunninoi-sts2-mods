using System.ComponentModel;
using System.Reflection;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;

namespace thunninoiSkinManager.thunninoiSkinManagerCode;

[GlobalClass]
public partial class SkinSelector : Control
{
    public string characterId { get; set; }
    public CharacterModel characterModel { get; set; }
    
    private Node2D? _previewSpriteContainer;
    private Label? _skinNameLable;
    private NGoldArrowButton _prevButton;
    private NGoldArrowButton _nextButton;
    private GodotObject? skinIntroSfx = null; 

    public override void _Ready()
    {
        _previewSpriteContainer = GetNode<Node2D>("VisualContainer");
        _skinNameLable = GetNodeOrNull<Label>("HBoxContainer/ScrollTextContainer/SkinName");
        _prevButton = GetNodeOrNull<NGoldArrowButton>("HBoxContainer/PrevArrow");
        _nextButton = GetNodeOrNull<NGoldArrowButton>("HBoxContainer/NextArrow");
        
        _prevButton.Connect(Button.SignalName.Pressed, Callable.From(onPrevPressed));
        _nextButton.Connect(Button.SignalName.Pressed, Callable.From(onNextPressed));
        if (characterModel != null)
        {
            loadPreview();
        }
    }
    
    private void loadPreview()
    {
        _skinNameLable.Text = SkinRegistry.GetActiveSkin(characterId).SkinName;
        // remove old if existed
        Node? existed = GetNodeOrNull("VisualContainer/PreviewSprite");
        existed?.Free();
        
        Node2D characterVisuals = characterModel.CreateVisuals().GetNode<Node2D>("Visuals");
        characterVisuals.Name = "PreviewSprite";
        characterVisuals.Scale = characterVisuals.GetScale() * 0.85f;
        _previewSpriteContainer.AddChild(characterVisuals.Duplicate());
        //GetNode<Node2D>("VisualContainer/PreviewSprite").SetPosition(new Vector2(0,0));
        //MegaSprite megaSprite = new MegaSprite(previewVisuals);
        CallDeferred("playPreviewAnim");
    }

    private void playPreviewAnim()
    {
        
        MegaSprite megaSprite = new MegaSprite(GetNode("VisualContainer/PreviewSprite"));
        var animState = megaSprite.GetAnimationState();
        if (megaSprite.HasAnimation("entry"))
        {
            animState.SetAnimation("entry", false, 0);
            animState.AddAnimation("idle_loop", 0f, true, 0);
        }
        else
        {
            animState.SetAnimation("idle_loop", true, 0);
        }
    }

    private void onPrevPressed()
    {
        SkinRegistry.CyclePrevious(characterId);
        refresh();
    }
    
    private void onNextPressed()
    {
        modEntry.Logger.Info("OnNextPressed");
        SkinRegistry.CycleNext(characterId);
        refresh();
    }

    public void refresh()
    {
        refreshBg();
        refreshButton();
        multiplayerIconRefresh();
        loadPreview();
        //playIntroSfx();
    }

    private void refreshBg()
    { 
        var parent = GetParent();
        modEntry.Logger.Info(parent.Name);
        Node? bgContainer = parent.GetNode("AnimatedBg");

        Node? curBg = bgContainer.GetNode(characterId.ToUpper() + "_bg");
        string bgName = curBg.Name;

        string skinBgPath = characterModel.CharacterSelectBg;
        if (string.IsNullOrWhiteSpace(skinBgPath)) return;
        
        PackedScene bgScene = ResourceLoader.Load<PackedScene>(skinBgPath, null, ResourceLoader.CacheMode.Reuse);
        if (bgScene == null) return;

        Control skinBg = bgScene.Instantiate<Control>();
        skinBg.Name = bgName;
        curBg.Free();
        bgContainer.AddChildSafely(skinBg);
    }

    private void refreshButton()
    {
        Node? mainScreen = GetParent();
        NCharacterSelectButton? targetButton = mainScreen.GetNodeOrNull<NCharacterSelectButton>("CharSelectButtons/ButtonContainer/" + characterId.ToUpper() + "_button");
        if (targetButton == null) return;
        TextureRect? charPortrait = targetButton.GetNodeOrNull<TextureRect>("%Icon");
        if (charPortrait == null) return;
        charPortrait.Texture = characterModel.CharacterSelectIcon;
    }

    private void multiplayerIconRefresh()
    {
        //FieldInfo? charId = typeof(NRemoteLobbyPlayer).GetField("_character", BindingFlags.NonPublic | BindingFlags.Instance);
        MethodInfo? refreshMultiplayer = typeof(NRemoteLobbyPlayer).GetMethod("RefreshVisuals", BindingFlags.NonPublic | BindingFlags.Instance);

        Node? mainScreen = GetParent();
        //NRemoteLobbyPlayerContainer playerContainer = mainScreen.GetNodeOrNull<NRemoteLobbyPlayerContainer>("RemotePlayerContainer");
        FlowContainer playerContainer = mainScreen.GetNode<FlowContainer>("RemotePlayerContainer/Container");
        if (playerContainer == null) return;
        foreach (Node? child in playerContainer.GetChildren())
        {
            modEntry.Logger.Info($"Found {child.Name} ({child.GetType()})");
            if (child is NRemoteLobbyPlayer)
            {
                refreshMultiplayer.Invoke(child, null);
            }
        }
    }

    /*
    private void playIntroSfx()
    {
        if (skinIntroSfx != null && (bool)skinIntroSfx.Call("is_playing")) skinIntroSfx.Call("stop");
        SkinData.AudioData? introSfx = SkinRegistry.sfxResolve(characterId, "charIntro");
        if (introSfx == null) return;
        float sfxVolume = FmodAudio.GetBusVolume("bus:/master/music");
        skinIntroSfx = FmodAudio.PlayFile(introSfx.audioPath, volume: sfxVolume * introSfx.volume, pitch:introSfx.pitch);
    }
    */
}