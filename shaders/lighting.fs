#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D textureSampler;
uniform sampler2D lightMap;

uniform vec2 tileSize;
uniform vec2 gridSize;

out vec4 finalColor;

void main() {
    vec2 tileCoord = floor(fragTexCoord.xy  / tileSize);
    vec2 lightUV = (tileCoord + 0.5) / gridSize;

    float lightCenter = texture(lightMap, lightUV).r;
    float lightRight = texture(lightMap, (tileCoord + vec2(1.0, 0.0) + 0.5) / gridSize).r;
    float lightTop = texture(lightMap, (tileCoord + vec2(0.0, 1.0) + 0.5) / gridSize).r;

    float interpolatedLight = mix(
        mix(lightCenter, lightRight, fragTexCoord.x),
        mix(lightCenter, lightTop, fragTexCoord.y),
        0.5
    );

    vec4 tileColor = texture(textureSampler, fragTexCoord);
    finalColor = tileColor * vec4(vec3(interpolatedLight), 0.5);
}