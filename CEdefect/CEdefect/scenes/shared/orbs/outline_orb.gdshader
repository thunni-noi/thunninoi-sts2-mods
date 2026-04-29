shader_type canvas_item;
// render_mode unshaded; // Uncomment this if the outline behaves weirdly with 2D lights

uniform vec4 outline_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);
uniform float outline_width : hint_range(0.0, 10.0) = 2.0;

// CanvasGroup requires reading from the screen texture!
uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
    // 1. Read the CanvasGroup render from the screen backbuffer
    vec4 sprite_color = textureLod(screen_texture, SCREEN_UV, 0.0);
    
    // CanvasGroup premultiplies alpha, so we need to un-premultiply it 
    // to get the correct colors back.
    if (sprite_color.a > 0.0001) {
        sprite_color.rgb /= sprite_color.a;
    }
    
    // 2. Calculate the outline alpha
    float alpha = 0.0;
    for (float x = -ceil(outline_width); x <= ceil(outline_width); x++) {
        for (float y = -ceil(outline_width); y <= ceil(outline_width); y++) {
            if (x * x + y * y <= outline_width * outline_width) {
                // Notice we use SCREEN_PIXEL_SIZE and SCREEN_UV here
                vec2 uv_offset = vec2(x, y) * SCREEN_PIXEL_SIZE;
                alpha = max(alpha, textureLod(screen_texture, SCREEN_UV + uv_offset, 0.0).a);
            }
        }
    }
    
    // 3. Mix the outline with the sprite
    vec4 final_color = mix(outline_color, sprite_color, sprite_color.a);
    final_color.a = max(sprite_color.a, alpha * outline_color.a);
    
    COLOR = final_color;
}