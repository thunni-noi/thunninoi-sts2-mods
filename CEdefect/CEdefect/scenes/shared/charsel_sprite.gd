extends SpineSprite



# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	self.get_animation_state().set_animation("entry", false)
	self.get_animation_state().add_animation("idle_loop", true)
