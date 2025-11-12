shader_type canvas_item;

// Textures from two viewports
uniform sampler2D viewport_a_color;
uniform sampler2D viewport_a_depth;
uniform sampler2D viewport_b_color;
uniform sampler2D viewport_b_depth;

void fragment() {
    vec2 uv = UV; // normalized UV

    // Sample color and depth
    vec4 color_a = texture(viewport_a_color, uv);
    float depth_a = texture(viewport_a_depth, uv).r; // depth stored in red channel

    vec4 color_b = texture(viewport_b_color, uv);
    float depth_b = texture(viewport_b_depth, uv).r;

    // Compare depths: smaller depth is in front
    vec4 final_color = depth_a < depth_b ? color_a : color_b;

    COLOR = final_color;
}
