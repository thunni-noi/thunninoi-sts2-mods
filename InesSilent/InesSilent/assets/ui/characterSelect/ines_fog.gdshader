shader_type canvas_item;

uniform sampler2D noise_texture: repeat_enable, filter_nearest;
uniform float fog_density: hint_range(0.0, 1.0, 0.1) = 0.9;
uniform vec2 fog_speed = vec2(0.02, 0.01);

void fragment() {
	
	vec2 uv = UV + fog_speed * TIME;
	// noise texture
	float noise = texture(noise_texture, uv).r;
	// convert range
	float fog = clamp(noise * 2.0 - 1.0, 0.0, 1.0);
	
	// apply
	COLOR.a *= fog * fog_density;
	// Called for every pixel the material is visible on.
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
