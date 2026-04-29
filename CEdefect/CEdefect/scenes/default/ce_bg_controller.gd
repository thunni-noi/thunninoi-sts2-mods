extends Sprite2D

@export var hover_multiplier = 1.0
@export var hover_speed = 1.5
@export var sprite : Sprite2D
# Called when the node enters the scene tree for the first time.

var is_mouse_in = false
var curve_t = 0
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	offset = Vector2(0, hover_multiplier * sin(curve_t))
	sprite.translate(offset)
	curve_t += delta * hover_speed
	pass
