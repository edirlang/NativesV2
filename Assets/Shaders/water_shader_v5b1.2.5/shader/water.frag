#version 120

#define PI 3.141592653589793238462643383279

uniform float water_dist;
uniform float timer;
uniform sampler2D colorMap;
uniform sampler2D reflectedColorMap;
uniform sampler2D stencilMap;
uniform vec3 water_color;
uniform float surface_effects;
varying vec4 texcoord;

//TEST
const int gaussRadius = 11;
const float gaussFilter[gaussRadius] = float[gaussRadius](
    0.0402,0.0623,0.0877,0.1120,0.1297,0.1362,0.1297,0.1120,0.0877,0.0623,0.0402
);


void main ()
{
    //vec3 water_color = vec3(0.0, 0.4, 0.2); //0.22,0.2 (now uniform through config)
    float in_water; //'boolean'

    vec4 color;
    vec4 stencil = texture2D(stencilMap,texcoord.rg);
    
    if (stencil.r > 0.05) in_water = 1.0;
    else in_water = 0.0;
    
    //distortion begin
    float x_scale = 1.0;
    float z_scale = 1.0;
    
    float used_timer = timer;
    float time_scale = 2.0; //2.0
    float size_scale = 1.6*6.3; //also dependent on radius
    
    if (stencil.r <= 0.15) {
        size_scale *= 6.0;
        time_scale *= 1.5;
    } else {
        size_scale *= stencil.r;
    }

    //timer needs to be 'in period'
    if (stencil.r >= 0.5) { //
        x_scale = ( 0.995 + (sin(2.0*time_scale*3.14159*used_timer - sin(0.5*size_scale*3.14159*stencil.g) + (size_scale*3.14159*stencil.g))/100.0)); //scales btw 0.995 and 1.005
    } //- sin(0.5*size_scale*3.14159*stencil.b)
    z_scale = ( 0.995 + (sin(sin(time_scale*3.14159*used_timer)  + 1.5*sin(0.8*size_scale*3.14159*stencil.b))/150.0));
    vec2 disturbed = vec2(x_scale*texcoord.x, z_scale*texcoord.y);
    
    vec4 reflection = texture2D(reflectedColorMap,disturbed.rg);
    //if (x_scale + z_scale > 2.00099) reflection *= 1.8; //to monitor effects...!
    
    time_scale = 3.0; //2.0
    size_scale = 2.4*6.3 * stencil.r;
    
    //timer needs to be 'in period'
    if (stencil.r >= 0.5) { //- sin(0.25*size_scale*3.14159*stencil.g)
        x_scale = ( 0.995 + (sin(2.0*time_scale*3.14159*used_timer - sin(0.25*size_scale*3.14159*stencil.g) + size_scale*3.14159*stencil.g)/100.0)); //scales btw 0.995 and 1.005
    }
    z_scale = ( 0.995 + (sin(sin(time_scale*3.14159*used_timer) + 1.5*sin(size_scale*3.14159*stencil.b))/100.0));
    vec2 disturbed_2 = vec2(x_scale*texcoord.x, z_scale*texcoord.y);
    //distortion end
    
    vec4 reflection_2 = texture2D(reflectedColorMap,disturbed_2.rg);
    
        
    reflection = (reflection+reflection_2)/2.0;
    
    //'refraction'(for all under-water)
    if (in_water > 0.05) {
        float look_up_range = 0.008; //0.005 //0.008
        //costs performance! (masking to avoid outside water look-ups, alternative another scene clipping)
        if (texture2D(stencilMap,vec2(disturbed.r + look_up_range, disturbed.g + look_up_range)).r > 0.001 &&
            texture2D(stencilMap,vec2(disturbed.r - look_up_range, disturbed.g - look_up_range)).r > 0.001 &&
            texture2D(stencilMap,vec2(disturbed.r, disturbed.g)).r > 0.001) {
            color = texture2D(colorMap,disturbed.rg); //drunken effect without stencil if
        } else {
            color = texture2D(colorMap,texcoord.rg);
        }
    } else {
        color = texture2D(colorMap,texcoord.rg);
    }
    
    //combine reflection and scene at water surfaces
    //modify reflection in distance?
    float reflection_strength = 0.3 * (stencil.r-0.1); //0.4, 0.55, 0.1, 0.14, 0.16, 0.17, 0.5
    float disable_refl = stencil.r-0.1;
    
    if (disable_refl <= 0.0) disable_refl = 0.0; //no reflection
    
    //times inverted color.r for a stronger reflection in darker water parts!
    //used to be 8.0, 6.0, 3.5
    vec3 reflection_color = vec3(1.0, 1.0, 1.0);
    reflection_color =  reflection_strength * disable_refl * reflection.rgb;// * reflection.rgb * in_water * (1.0-(color.r*color.g*color.b));
    
    //more color in darker water in relation to the reflection
    //color darkened
    float difference = (reflection_color.r+reflection_color.g+reflection_color.b)/3.0 - (color.r + color.g + color.b)/5.5; //5.5
    if (difference < 0.0) difference = 0.0;
    vec3 regular_color = color.rgb * (1.0-in_water*reflection_strength) + (in_water * (difference * water_color));
    
    if (surface_effects > 0.0) {
    
        //"waves"
        //float t = 3.0*(PI*0.1*timer)+12.0;
        //float u = (1.1*stencil.g);
        //float v = (1.1*stencil.b);
        
        //water "height" bumps
        //sin(PI*t*v) -> also for size of the "bumps"
        //20.0*t -> "speed"
        //float rsx = (sin( 0.9*sin(PI*t*v) + 0.7*sin(PI*t*v) + 18.1*PI*stencil.g) + sin( t*t + sin(PI*t*v*u) + 26.3*PI*stencil.g))*0.05;
        //float rsz = (sin( 0.6*sin(PI*t*u) + 0.8*sin(PI*t*u) + 16.4*PI*stencil.b) + sin( t*t + sin(PI*t*u+u) + 32.2*PI*stencil.b))*0.05;
        
        //rsx += 0.15;
        //rsz += 0.15;
        
        //float fresn = clamp(4.0/water_dist,0.0,1.0);
        
        //rsx = clamp(rsx, 0.0, 1.0);
        //rsz = clamp(rsz, 0.0, 1.0);
        
        //sinc filter (alternative, not used yet)
        
        //float tm = (timer/550.0)+0.255; //0.45 0.26 0.28
        //if (tm > 0.28) tm = 0.28-(timer/55.0); //HMM
        
        //float tm = 0.45;
        
        //float pow3 = (tm+rsx+rsz)*(tm+rsx+rsz)*(tm+rsx+rsz);
        //rsx *= (1.6 + 0.7*sin(16.6*(tm+rsx+rsz))/pow3);
        //rsz *= (1.3 + 0.7*sin(16.1*(tm+rsx+rsz))/pow3);
        
        //rsx = 1.0-rsx;
        //rsz = 1.0-rsz;
        
        //surface color increase
        //if (rsx + rsz > 1.9 && rsx + rsz < 1.999) {
        //    rsx *= 1.05;
        //    rsz *= 1.05;
        //    if (rsx + rsz > 1.999) {
        //        rsx *= 1.07;
        //        rsz *= 1.07;
        //        if (rsx + rsz > 1.9993) {
        //            rsx *= 1.1;
        //            rsz *= 1.1;
        //        }
        //    }
        //}
        
        //float increase = rsx + rsz;
        //1.9 -> 1, 1.999 (~2) -> 1.2
        //first scale 1-1.2 to 0-1
        //float mult = 5.0;
        //float max = 1.15;
        //increase = mult * (increase - 0.9);
        //if (increase > mult*max) increase = 0.0;
        //increase = clamp(increase, 1.0, 2.0);
        
        //rsx *= increase;
        //rsz *= increase;
        
        //if (increase > 1) rsx = 100;
        
        //rsx = 1.0-rsx;
        //rsz = 1.0-rsz;
        
        //reflection_color *= vec3(0.6, 0.95, 0.95);
        //reflection_color *= 0.8;
        //reflection_color = 1.1*reflection_color + 
        //        fresn * (1.0-reflection_strength) * 
        //        (reflection_color*1.5*rsx + reflection_color*1.5*rsz);
        
        //float count = 1.0;
        for (int i=-3; i<3; i++) {
            vec2 uv = disturbed.rg;
            uv.y += 0.007 * i;
            
            
            vec3 col = texture2D(reflectedColorMap,uv).xyz;
            if (reflection_color.r < 0.01) col = vec3(0.0);
            
            float str = (col.x + col.y + col.z) / 3.0;
            str = str * str - 0.2;
            
            //if (col.x + col.y + col.z > 2.3) {
                reflection_color += clamp((str * col), 0.0, 3.0);// * vec3(1.0, 0.0, 0.0);
                //count ++;
            //}
        }
        reflection_color /= 6;
        
        //if (count > 1) reflection_color.rgb = vec3(1.0, 0.0, 0.0);
                
    }
    
    vec4 out_color = vec4(regular_color, 1.0) + vec4(reflection_color, 1.0);
    
    //TEST
    //vec3 add = vec3(0.0, 0.0, 0.0);
    
    //if (    stencil.x > 0.1) {
    
        //vec2 uShift = vec2(0.005, 0.0);
        
        //vec2 texCoord = texcoord.xy - float(int(gaussRadius/2)) * uShift;
         
        //for (int i=0; i<gaussRadius; ++i) { 
        //    add += gaussFilter[i] * texture2D(colorMap, texCoord).xyz;
        //    texCoord += uShift;
        //}
        
        //uShift = vec2(0.0, 0.007);
        
        //texCoord = texcoord.xy - float(int(gaussRadius/2)) * uShift;
        //for (int i=0; i<gaussRadius; ++i) { 
         //   add += gaussFilter[i] * texture2D(colorMap, texCoord).xyz;
        //    texCoord += uShift;
        //}
        
    //}
    //TEST END
    
    gl_FragColor = out_color;// * vec4(1.5, 1.3, 1.0, 1.0);
    
    //gl_FragColor -= 2.0*vec4(0.1, 0.05, 0.0, 1.0);
    //gl_FragColor += 1.2*vec4(1.5, 1.3, 0.8,1.0)*vec4(add,1.0);
    
    //debug section
    //gl_FragColor = texture2D(colorMap,texcoord.rg);
    //gl_FragColor = stencil;
    //gl_FragColor = vec4(vec3(stencil.r), 1.0);
    //gl_FragColor = texture2D(reflectedColorMap,texcoord.rg);
    
    //if (rsx > 0.28) gl_FragColor = vec4(1.0, 0.0, 0.0, 1.0);
}