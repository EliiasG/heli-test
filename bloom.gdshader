shader_type canvas_item;

uniform vec4 glow_color : source_color = vec4(1.0,1.0,1.0,1.0);
uniform float glow_radius = 8.0;
uniform float glow_intensity = 2.0;
uniform float threshold = 0.5;

void fragment() {
    vec4 base = texture(TEXTURE, UV) * COLOR;
    float brightness = max(max(base.r, base.g), base.b);
    vec4 result = base;

    if (brightness > threshold) {
        // Simple radial blur: sample neighbours
        vec2 tex_size = TEXTURE_PIXEL_SIZE;
        vec4 sum = vec4(0.0);
        int count = 0;
        for (int x = -16; x <= 16; ++x) {
            for (int y = -16; y <= 16; ++y) {
                vec2 off = vec2(float(x), float(y)) * (tex_size * glow_radius * 0.1);
                sum += texture(TEXTURE, UV + off);
                count++;
            }
        }
        vec4 blur = sum / float(count);
        result += (blur * glow_color) * glow_intensity;
    }

    COLOR = result;
}
