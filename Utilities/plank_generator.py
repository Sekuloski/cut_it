from pathlib import Path

from PIL import Image


def split_images(image_path):
    image = Image.open(image_path)
    width, height = image.size
    images = {}

    midpoint = width // 2

    images['left_half.png'] = image.crop((0, 0, midpoint, height))
    images['right_half.png'] = image.crop((midpoint, 0, width, height))

    midpoint = width // 4

    images['left_quarter.png'] = image.crop((0, 0, midpoint, height))
    images['right_three_quarter.png'] = image.crop((midpoint, 0, width, height))
    images['left_three_quarter.png'] = image.crop((0, 0, midpoint*3, height))
    images['right_quarter.png'] = image.crop((midpoint*3, 0, width, height))

    midpoint = width // 8

    images['left_eight.png'] = image.crop((0, 0, midpoint, height))
    images['right_seven_eight.png'] = image.crop((midpoint, 0, width, height))
    images['left_seven_eight.png'] = image.crop((0, 0, midpoint*7, height))
    images['right_eight.png'] = image.crop((midpoint*7, 0, width, height))

    return images


def main():
    results = Path(__file__).resolve().parent / 'results'
    image_path = input("Full path to image: ")
    images = split_images(image_path)
    counter = 0
    for url, image in images.items():
        counter += 1
        image.save(results / url)


if __name__ == '__main__':
    main()
