extends SpineSprite

var last_update = 0.0
var update_interval = snapped(1/24, 0.01) # 24 fps


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	update_mode = SpineConstant.UpdateMode_Manual
	get_animation_state().set_animation("idle_loop")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	last_update += delta
	if last_update >= update_interval:
		update_skeleton(last_update)
		last_update = 0.0
	pass
