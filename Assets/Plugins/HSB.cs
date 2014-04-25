using UnityEngine;

// RGB<->HSB at runtime
public struct HSB {
	public float hue;			// 0,360
	public float saturation;	// 0,1
	public float brightness;	// 0,1
	public float alpha;
	
	public HSB(Color c) {
		hue = 0.0f;
		saturation = 0.0f;
		brightness = 0.0f;
		alpha = 0.0f;
		FromARGB(ref c);
	}

	//-------------------------------------------------------------------------
	// TEXTURE METHODS
	//-------------------------------------------------------------------------

	public void Apply(Texture2D t) {
		HSB transformer = new HSB();
		hue %= 360.0f;
		if (hue < 0.0f) {
			hue += 360.0f;
		}
		for (int i=0; i<t.mipmapCount; i++) {
			Color[] pixels = t.GetPixels(i);
			for (int j=0; j<pixels.Length; j++) {
				transformer.FromARGB(ref pixels[j]);
				transformer.hue += hue;
				transformer.brightness += brightness;
				transformer.saturation += saturation;
				transformer.hue = transformer.hue >= 360.0f ? transformer.hue-360.0f: transformer.hue;
				transformer.brightness = transformer.brightness < 0.0f ? 0.0f : 
					transformer.brightness > 1.0f ? 1.0f: transformer.brightness;
				transformer.saturation = transformer.saturation < 0.0f ? 0.0f : 
					transformer.saturation > 1.0f ? 1.0f : transformer.brightness;
				transformer.ToARGB(out pixels[j]);
			}
			t.SetPixels(pixels, i);
		}
		t.Apply(false);
	}
	
	//-------------------------------------------------------------------------
	// INSTANCE METHODS
	//-------------------------------------------------------------------------
	
	public Color ARGB {
		get {
			Color result;
			ToARGB(out result);
			return result;
		}
		set {
			FromARGB(ref value);
		}
	}
	
	public void ToARGB(out Color c) {
		c.a = alpha;
		if (saturation == 0.0f) {
			// grey
			c.r = brightness;
			c.g = brightness;
			c.b = brightness;
		} else {
			float h = hue/60.0f;
			int i = (int)h;
			float f = h - i;
			// p = brightness * (1 - saturation);
			// q = brightness * (1 - saturation * f);
			// t = brightness * (1 - saturation * (1-f));
			switch(i) {
			case 0: // v,t,p
				c.r = brightness;
				c.g = brightness * (1.0f - saturation * (1.0f-f));
				c.b = brightness * (1.0f - saturation);
				break;
			case 1: // q,v,p
				c.r = brightness * (1.0f - saturation * f);
				c.g = brightness;
				c.b = brightness * (1.0f - saturation);
				break;
			case 2: // p,v,t
				c.r = brightness * (1.0f - saturation);
				c.g = brightness;
				c.b = brightness * (1.0f - saturation * (1.0f-f));
				break;
			case 3: // p,q,v
				c.r = brightness * (1.0f - saturation);
				c.g = brightness * (1.0f - saturation * f);
				c.b = brightness;
				break;
			case 4: // t,p,v
				c.r = brightness * (1.0f - saturation * (1.0f-f));
				c.g = brightness * (1.0f - saturation);
				c.b = brightness;
				break;
			default: // v,p,q
				c.r = brightness;
				c.g = brightness * (1.0f - saturation);
				c.b = brightness * (1.0f - saturation * f);
				break;
			}
		}
	}
	
	public void FromARGB(ref Color c) {
			alpha = c.a;
			brightness = c.r;
			brightness = c.g > brightness ? c.g : brightness;
			brightness = c.b > brightness ? c.b : brightness;
			float min = c.r;
			min = c.g < min ? c.g : min;
			min = c.b < min ? c.b : min;
			float delta = brightness - min;
			if (brightness > 0.0f) {
				saturation = delta / brightness;
			} else {
				saturation = 0.0f;
				hue = 0.0f;
				return;
			}
			if (c.r == brightness) {
				// between yellow and magenta	
				hue = (c.g - c.b) / delta;
			} else if (c.g == brightness) {
				// between cyan and yellow	
				hue = 2.0f + (c.b - c.r) / delta;
			} else {
				// between magenta and cyan	
				hue = 4.0f + (c.r - c.g) / delta;
			}
			hue *= 60.0f;
			if (hue < 0.0f) {
				hue += 360.0f;
			}
		
	}
		
}
