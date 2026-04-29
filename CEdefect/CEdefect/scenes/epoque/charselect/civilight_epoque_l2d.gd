extends Node2D

@export var civilight_l2d : SpineSprite
@export var clickbox : Area2D

@export var idle_animation = "Idle"
@export var click_animation = "Interact"
@export var special_animation = "Special"

@export var min_random_interval = 15.0
@export var max_random_interval = 40.0

var is_playing_animation = false
var time_until_rng = 0.0
var rng = RandomNumberGenerator.new()

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	time_until_rng = rng.randf_range(min_random_interval, max_random_interval);
	is_playing_animation = false
	civilight_l2d.get_animation_state().set_animation("Idle", true)
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if time_until_rng <= 0:
		if ([true, false].pick_random()):
			play_click_animation()
		else :
			play_special_animation()
		time_until_rng = rng.randf_range(min_random_interval, max_random_interval)
	
	if not is_playing_animation:
		time_until_rng -= delta
	#print(time_until_rng)
	pass

func play_click_animation():
	is_playing_animation = true
	if (time_until_rng <= 10): time_until_rng += 10 	#prevent special anim playing immediately after interact animation
	var animation = civilight_l2d.get_animation_state()
	#animation.clear_tracks()
	animation.set_animation("Interact")
	animation.add_animation("Idle", 0, true)

func play_special_animation():
	is_playing_animation = true
	var animation = civilight_l2d.get_animation_state()
	#animation.clear_tracks()
	animation.set_animation("Special")
	animation.add_animation("Idle", 0, true)

func _on_civilight_l_2d_animation_completed(spine_sprite: SpineSprite, animation_state: SpineAnimationState, track_entry: SpineTrackEntry) -> void:
	var completed_anim = track_entry.get_animation().get_name()
	if (is_playing_animation && completed_anim != "Idle"):
		is_playing_animation = false
	pass # Replace with function body.
