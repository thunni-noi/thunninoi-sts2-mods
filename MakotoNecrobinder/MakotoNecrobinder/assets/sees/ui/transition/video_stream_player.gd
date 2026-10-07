@tool
extends VideoStreamPlayer

func _ready():
	# This checks if the code is running inside the editor
	if Engine.is_editor_hint():
		play()
