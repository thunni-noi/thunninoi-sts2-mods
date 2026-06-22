extends Label

@export var scroll_speed: float   = 60.0
@export var pause_duration: float = 1.5   
@export var edge_padding: float   = 0.0 

enum State { IDLE, PAUSE_START, SCROLL_LEFT, PAUSE_END, SCROLL_RIGHT }

var _state: State   = State.IDLE
var _offset_x: float = 0.0
var _max_offset: float = 0.0
var _pause_timer: float = 0.0

var _last_text: String = ""
var _last_container_w: float = 0.0

func _ready() -> void:
	autowrap_mode = TextServer.AUTOWRAP_OFF
	clip_text = false
	await get_tree().process_frame
	_check_for_changes()


func _process(delta: float) -> void:
	_check_for_changes()

	if _state == State.IDLE:
		return

	match _state:
		State.PAUSE_START:
			_pause_timer -= delta
			if _pause_timer <= 0.0:
				_state = State.SCROLL_LEFT

		State.SCROLL_LEFT:
			_offset_x -= scroll_speed * delta
			if _offset_x <= -_max_offset:
				_offset_x    = -_max_offset
				_pause_timer = pause_duration
				_state       = State.PAUSE_END

		State.PAUSE_END:
			_pause_timer -= delta
			if _pause_timer <= 0.0:
				_state = State.SCROLL_RIGHT

		State.SCROLL_RIGHT:
			_offset_x += scroll_speed * delta
			if _offset_x >= 0.0:
				_offset_x    = 0.0
				_pause_timer = pause_duration
				_state       = State.PAUSE_START

	position.x = _offset_x


func _check_for_changes() -> void:
	var container_w: float = get_parent_control().size.x if get_parent_control() else 0.0

	if text == _last_text and is_equal_approx(container_w, _last_container_w):
		return
	_last_text        = text
	_last_container_w = container_w

	var font      : Font  = get_theme_font("font")
	var font_size : int   = get_theme_font_size("font_size")
	var text_w    : float = get_minimum_size().x * scale.x

	var overflow: float = text_w - container_w
	print(text_w)
	print(container_w)
	if overflow <= 0.0:
		_state     = State.IDLE
		_offset_x  = 0.0
		position.x = 0.0
	else:
		_max_offset = overflow + edge_padding
		if _state == State.IDLE:
			_offset_x    = 0.0
			_pause_timer = pause_duration
			_state       = State.PAUSE_START
