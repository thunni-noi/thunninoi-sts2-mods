extends Parallax2D

var viewport_size
var multiplier
var relative_x
var relative_y

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	viewport_size = get_viewport().size
	multiplier = 10
	relative_x = 0
	relative_y = 0
	pass # Replace with function body.
	
func _input(event):
	if (event) is InputEventMouseMotion:
		var mouse_x = event.position.x
		var mouse_y = event.position.y
		
		relative_x = -1 * (mouse_x - (viewport_size.x/2)) / (viewport_size.x/2)
		relative_y = -1 * (mouse_y - (viewport_size.y/2)) / (viewport_size.y/2)
		
		var count = 0
		for child in self.get_children():
			if child is Parallax2D:
				child.scroll_offset = Vector2(((count + 1) * multiplier) * relative_x, ((count + 1) * multiplier) * relative_y)
				count += 1
			
