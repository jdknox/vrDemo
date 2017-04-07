import wand.image
from wand.display import display
import logging
import numpy as np

dbg = logging.debug
logging.basicConfig(level=logging.DEBUG,
                    format='[%(levelname)s] (%(threadName)-10s) %(message)s',
                    )

def FloatExr(**kwargs):
    from ctypes import c_void_p, c_char_p
    setOption = wand.api.library.MagickSetOption
    getOption = wand.api.library.MagickGetOption
    setOption.argtypes = [c_void_p,  # MagickWand * wand
                          c_char_p,  # const char * option
                          c_char_p]  # const char * value
    getOption.argtypes = [c_void_p,  # MagickWand * wand
                          c_char_p,  # const char * option
                          ]

    class Wrapper(object):
        def __init__(self, **kwargs):
            self.wrapped = wand.image.Image(**kwargs)
            # -define quantum:format=floating-point
            setOption(self.wrapped.wand, b'quantum:format', b'floating-point')
            setOption(self.wrapped.wand, b'option:compose:clamp', b'false')
            # key = b'option:compose:clamp'
            # result = getOption(self.wrapped.wand, key)
            # dbg(f'magick {key}: {result}')

    return Wrapper(**kwargs).wrapped


def blit(inImage, outImage, locations):
    _, inImgHeight = inImage.size
    dimension, x, y = integerPositions(locations['source'], inImgHeight)
    y += dimension
    dbg('\tsource: ' + str([x, inImgHeight - y, dimension, dimension]))
    with FloatExr(image=inImage) as section:
        section.crop(x, inImgHeight - y, width=dimension, height=dimension)  # left, top, right, bottom
        _, outImgHeight = outImage.size
        s, x, y = integerPositions(locations['destination'], outImgHeight)
        scale = s / dimension
        if scale != 1.0:
            dbg(f'\tresizing by scale factor: {scale}')
            section.transform(resize=f'{scale * 100}%')
            dimension = int(dimension * scale)
        y += dimension
        dbg(f'\tdest: {x}, {y}, ({outImgHeight - y})')
        # dbg(f'\tsection.mode: {section.mode}')
        # image.composite(section, x, imgHeight - y + dimension)
        outImage.composite_channel('default_channels', section, 'copy', x, outImgHeight - y)
    return outImage


def integerPositions(values, scale):
    return (np.array(values) * scale).astype('int')


filename = 'Lightmap-0_comp_light'
inFilename = f'../../scenes/lightBakingScene/{filename}.exr'
filename = 'Lightmap-custom-0_comp_light'
outFilename = f'../../scenes/mainScene/{filename}.exr'

lightmapScaleOffsets = {
    'doorFrame': {
        'source': (0.4824795, 0.03838703, -1.09475E-06),  # 'source': (0.2412397, 0.01919352, -5.473751E-07),
        'destination': (0.1206199, 0.7687286, 0.3191034)
    },
    'stonePlate': {
        'source': (0.1191147, 0.5662228, -0.0009312111),
        'destination': (0.1191147 / 2, -0.0004260079, 0.5650037)
    },
}


objectsToBlit = ['stonePlate']
with FloatExr(filename=inFilename) as inImg:
    for gameObject, locations in lightmapScaleOffsets.items():
        if gameObject not in objectsToBlit:
            continue
        dbg(f'copying locations for "{gameObject}"')
        with FloatExr(filename=outFilename) as outImg:
            img = blit(inImg, outImg, locations)
            img.save(filename=outFilename)
