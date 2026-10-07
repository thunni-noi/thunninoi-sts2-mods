extends Node2D

@export var target_sprite : SpineSprite = null
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	target_sprite.get_animation_state().set_animation("relaxed_loop")
	print("ready")
	pass # Replace with function body.

func _input(event : InputEvent):
	#if Input.action_press()
	if (Input.is_action_just_pressed("take_screenshot")):
		take_screenshot()
	pass	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func take_screenshot():
	var sshot = get_viewport().get_texture().get_image()
	sshot.save_png("user://screenshot_godot.png")
	print("screenshot taken")
