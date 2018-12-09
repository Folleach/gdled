#include <FastLED.h>

#define NUM_LEDS 60
#define DATA_PIN 3

CRGB activeColor;
CRGB passiveColor;

CRGB leds[NUM_LEDS];

union ArrayToInteger {
	byte bytes[4];
	uint32_t integer;
};

void progresChanged(byte data[]) {
	ArrayToInteger convert;
	for (int i = 0; i < 4; i++) {
		convert.bytes[i] = data[i];
	}
	int value = map(convert.integer, 0, 100, 0, NUM_LEDS);
	for (int i = 0; i < NUM_LEDS; i++) {
		if (i < value) {
			leds[i] = activeColor;
			continue;
		}
		leds[i] = passiveColor;
	}
	FastLED.show();
}

void setup() {
	delay(2000);
	FastLED.addLeds<WS2812, DATA_PIN, GRB>(leds, NUM_LEDS);
	Serial.begin(9600);
	activeColor = CRGB(171, 0, 52);
	passiveColor = CRGB(3, 3, 3);
	FastLED.showColor(passiveColor);
}

void loop() {
	if (Serial.available() > 0) {
		byte type[1];
		Serial.readBytes(type, 1);
		if (type[0] == 1) {
			byte data[4];
			Serial.readBytes(data, 4);
			progresChanged(data);
		}
	}
}
