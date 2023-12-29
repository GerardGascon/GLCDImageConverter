# GLCD Image Converter

GLCD Image Converter converts an image with 128x64 pixels or less and a height multiple of 8 into an array and a draw function for a GLCD.

## Usage

- Create an image within 128x64 pixels.
    - Transparent pixels are mapped to 0 and non-transparent pixels are mapped to 1.
- Make sure the image’s height is a multiple of 8.
- Drag the image onto the .exe file.
- Two files, a .h and a .c should be generated if no errors happen.
