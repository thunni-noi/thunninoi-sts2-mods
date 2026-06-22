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
    public ModelId characterId { get; set; }
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
        
        _prevButton.Connect(Button.SignalName.Pressed, Callable.From(OnPrevPressed));
        _nextButton.Connect(Button.SignalName.Pressed, Callable.From(OnNextPressed));
        if (characterModel != null)
        {
            LoadPreview();
        }
    }
    
    private void LoadPreview()
    {
        _skinNameLable.Text = SkinRegistry.GetActiveSkin(characterId).SkinName;
        // remove old if existed
        Node? existed = GetNodeOrNull("VisualContainer/PreviewSprite");
        existed?.Free();

        Node2D baseVisual = characterModel.CreateVisuals();
        Node2D characterVisuals = baseVisual.GetNode<Node2D>("Visuals");
        characterVisuals.Name = "PreviewSprite";
        characterVisuals.Scale = characterVisuals.GetScale() * 0.85f;
        _previewSpriteContainer.AddChild(characterVisuals.Duplicate());
        //GetNode<Node2D>("VisualContainer/PreviewSprite").SetPosition(new Vector2(0,0));
        //MegaSprite megaSprite = new MegaSprite(previewVisuals);
        PlayPreviewSprite();
    }

    private void PlayPreviewSprite()
    {
        Node animNode = GetNode<Node>("VisualContainer/PreviewSprite");
        MegaSprite megaSprite = new MegaSprite(GetNode("VisualContainer/PreviewSprite"));
        var animState = megaSprite.GetAnimationState();
        if (megaSprite.HasAnimation("entry"))
        {
            modEntry.Logger.Info("Play entry anim");
            animState.SetAnimation("entry", loop:false);
            animState.AddAnimation("idle_loop", loop: true);
        }
        else
        {
            animState.AddAnimation("idle_loop", loop: true);   
        }
        

    }

    private void OnPrevPressed()
    {
        SkinRegistry.CyclePrevious(characterId);
        Refresh();
    }
    
    private void OnNextPressed()
    {
        modEntry.Logger.Info("OnNextPressed");
        SkinRegistry.CycleNext(characterId);
        Refresh();
    }

    public void Refresh()
    {
        RefreshBg();
        RefreshButton();
        MultiplayerIconRefresh();
        LoadPreview();
        //playIntroSfx();
    }

    private void RefreshBg()
    { 
        var parent = GetParent();
        modEntry.Logger.Info(parent.Name);
        Node? bgContainer = parent.GetNode("AnimatedBg");

        Node? curBg = bgContainer.GetNode(characterId.Entry.ToUpper() + "_bg");
        if (curBg == null) return;
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

    private void RefreshButton()
    {
        Node? mainScreen = GetParent();
        NCharacterSelectButton? targetButton = mainScreen.GetNodeOrNull<NCharacterSelectButton>("CharSelectButtons/ButtonContainer/" + characterId.Entry.ToUpper() + "_button");
        if (targetButton == null) return;
        TextureRect? charPortrait = targetButton.GetNodeOrNull<TextureRect>("%Icon");
        if (charPortrait == null) return;
        charPortrait.Texture = characterModel.CharacterSelectIcon;
    }

    private void MultiplayerIconRefresh()
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