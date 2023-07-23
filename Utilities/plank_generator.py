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


def save_sheet(images, name):
    max_width = max(image.width for image in images)
    max_height = max(image.height for image in images)

    spacing = 25

    sheet_width = max_width * 5 + spacing * 6  # 5 images with 5 spacings in between
    sheet_height = max_height * 2 + spacing * 3  # 2 rows with 2 spacings in between

    sheet = Image.new("RGBA", (sheet_width, sheet_height), (0, 0, 0, 0))

    y_offset_top = spacing
    y_offset_bottom = max_height + 2 * spacing
    for i in range(5):
        # Top row
        x_offset_top = i * (max_width + spacing) + spacing
        sheet.paste(images[i], (x_offset_top, y_offset_top))

        # Bottom row
        x_offset_bottom = i * (max_width + spacing) + spacing
        sheet.paste(images[i + 5], (x_offset_bottom, y_offset_bottom))

    sheet.save(f'results/{name} Sprites.png')


def main():
    root = Path(__file__).resolve().parent.parent
    name = 'Initial Plank'
    image_path = root / 'Assets' / 'Resources' / 'Planks' / f'{name}.png'
    images = dict(sorted(split_images(image_path).items()))

    save_sheet(list(images.values()), name)


if __name__ == '__main__':
    main()
