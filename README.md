# Image Sprites

[![Build status](https://ci.appveyor.com/api/projects/status/ox04djmajibm3qqv?svg=true)](https://ci.appveyor.com/project/madskristensen/imagesprites)

Download this extension from the [VS Gallery](https://visualstudiogallery.msdn.microsoft.com/8bb845e9-5717-4eae-aed3-1fdf6fe5819a)
or get the [CI build](http://vsixgallery.com/extension/cd92c0c6-2c32-49a3-83ca-0dc767c7d78e/).

---------------------------------------

An image sprite is a collection of images put into a single
image.

A web page with many images can take a long time to load
and generates multiple server requests.

Using image sprites will reduce the number of server
requests and save bandwidth.

This extension makes it easier than ever to create, maintain
and use image sprites in any web project.

See the [changelog](CHANGELOG.md) for changes and roadmap.

## Features

- Easy to create and update image sprites
- Supports png, jpg and gif images
- Configure vertical or horizontal sprite layouts
- Produce LESS, Sass or CSS file with sprite image locations
- [Image Optimizer](https://visualstudiogallery.msdn.microsoft.com/a56eddd3-d79b-48ac-8c8f-2db06ade77c3) integration
- Configurable DPI for high resolution images
- Works with both [Web Compiler](https://visualstudiogallery.msdn.microsoft.com/3b329021-cd7a-4a01-86fc-714c2d05bb6c)
and [Bundler & Minifier](https://visualstudiogallery.msdn.microsoft.com/9ec27da7-e24b-4d56-8064-fd7e88ac1c40)

### Create image sprite
Select the images in Solution Explorer and click
*Create image Sprite* from the context menu.

![Context menu](art/context-menu-images.png)

This will generate a **.sprite** manifest file as well as
the resulting image file and a **.css** file.

![Sol Exp](art/sol-exp.png)

### The .sprite file
The .sprite file is where information about the image sprite
is stored. It looks something like this:

```json
{
  "images": {
    "pic1": "a.png",
    "pic2": "b.png"
  },
  "orientation": "vertical",
  "optimize": "lossless",
  "padding": 10,
  "output": "png",
  "dpi": 384,
  "stylesheet": "css",
  "pathprefix": "/images/"
}
```

**images** is an array of relative file paths to the image
files that make up the resulting sprite file. The order
of the images are maintained in the generated sprite image.
The name of the image will be persisted in the generated
stylesheet as class names.

**direction** determines if the images are layed out either
horizontally or vertically. 

**padding** is the distance of whitespace inserted around each
individual image in the sprite. The value is in pixels.

**dpi** sets the resolution of the image. 96 is the default value.

**optimize** controls how the generated image sprite should be
optimized. Choose from *lossless*, *lossy* or *none*. This
feature requires the
[Images Optimizer](https://visualstudiogallery.msdn.microsoft.com/a56eddd3-d79b-48ac-8c8f-2db06ade77c3)
to be installed. 

**stylesheet** outputs LESS, Sass or plain CSS files to make
it easy to use the image sprite in any web project.

**pathprefix** adds a prefix string to the image path in
the *url(path)* value in the stylesheet.

### Update image sprite
Every time the .sprite file is modified and saved, the image
sprite and optional stylesheets are updated.

A button is also located on the context-menu of .sprite files
to make it even easier.

![Context menu update](art/context-menu-update.png)

## Consume the sprite
You can use the sprite from CSS, LESS or Sass.

### CSS
Make sure to configure the .sprite to produce a .css file.
Here's how to do that:

```json
"stylesheets": {
	"formats": [ "css" ]
}
```

That will produce a file called something like *mysprite.sprite.css*
nested under the *mysprite.sprite* file.

All you have to do is to include the .css file in your HTML
like so:

```html
<link href="mysprite.sprite.css" rel="stylesheet" />
```

You can then add HTML markup with 2 class names. The first
class name is the name of the .sprite file. In this case
*mysprite*. The other class name is the name of the individual
image in the sprite you wish to inject.

```html
<div class="mysprite pic1"></div>
```

### LESS and Sass
The support for LESS and Sass is very similar, so this guide
just shows how to use LESS.

Make sure to configure the .sprite to produce a .less file.
Here's how to do that:

```json
"stylesheets": {
	"formats": [ "less" ]
}
```

Then import the generated .less file into the .less files that
will consume the mixins generated by this extension.

```less
@import "mysprite.sprite.less";

.myclass {
    .mysprite-pic1();
}
```

That will produce the following CSS:

```css
.myclass {
  width: 16px;
  height: 16px;
  display: block;
  background: url('mysprite.sprite.png') -36px -10px no-repeat;
}
```

To use the generated CSS on your page, see the above section
on *CSS*.

## Contribute
Check out the [contribution guidelines](.github/CONTRIBUTING.md)
if you want to contribute to this project.

For cloning and building this project yourself, make sure
to install the
[Extensibility Tools 2015](https://visualstudiogallery.msdn.microsoft.com/ab39a092-1343-46e2-b0f1-6a3f91155aa6)
extension for Visual Studio which enables some features
used by this project.

## License
[Apache 2.0](LICENSE)