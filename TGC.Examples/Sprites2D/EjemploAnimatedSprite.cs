using Microsoft.DirectX;
using System.Drawing;
using TGC.Core._2D;
using TGC.Core.Example;
using TGC.Core.Geometries;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Util;

namespace TGC.Examples.Sprites2D
{
    /// <summary>
    ///     Ejemplo Sprite2D:
    ///     Unidades Involucradas:
    ///     # Unidad 2 - Conceptos Avanzados de 2D - Transformaciones
    ///     # Unidad 2 - Conceptos Avanzados de 2D - Animaci�n 2D
    ///     Muestra como dibujar un Sprite Animado en 2D.
    ///     Es similar al concepto de un GIF animado. Se tiene una textura de Sprite, compuesta
    ///     por un conjunto de tiles, o frames de animaci�n.
    ///     El Sprite animado va iterando sobre cada frame de animaci�n y lo muestra en 2D.
    ///     Es muy �til para crear menues, �conos, etc.
    ///     Autor: Mat�as Leone, Leandro Barbagallo
    /// </summary>
    public class EjemploAnimatedSprite : TgcExample
    {
        private TgcAnimatedSprite animatedSprite;
        private TgcBox box;

        public override string getCategory()
        {
            return "Sprite 2D";
        }

        public override string getName()
        {
            return "Sprite Animado";
        }

        public override string getDescription()
        {
            return "Muestra como dibujar un Sprite Animado en 2D";
        }

        public override void init()
        {
            //Crear Sprite animado
            animatedSprite = new TgcAnimatedSprite(
                GuiController.Instance.ExamplesMediaDir + "\\Texturas\\Sprites\\Explosion.png", //Textura de 256x256
                new Size(64, 64), //Tama�o de un frame (64x64px en este caso)
                16, //Cantidad de frames, (son 16 de 64x64px)
                10 //Velocidad de animacion, en cuadros x segundo
                );

            //Ubicarlo centrado en la pantalla
            var screenSize = GuiController.Instance.Panel3d.Size;
            var textureSize = animatedSprite.Sprite.Texture.Size;
            animatedSprite.Position = new Vector2(screenSize.Width / 2 - textureSize.Width / 2,
                screenSize.Height / 2 - textureSize.Height / 2);

            //Modifiers para variar parametros del sprite
            GuiController.Instance.Modifiers.addFloat("frameRate", 1, 30, 10);
            GuiController.Instance.Modifiers.addVertex2f("position", new Vector2(0, 0),
                new Vector2(screenSize.Width, screenSize.Height), animatedSprite.Position);
            GuiController.Instance.Modifiers.addVertex2f("scaling", new Vector2(0, 0), new Vector2(4, 4),
                animatedSprite.Scaling);
            GuiController.Instance.Modifiers.addFloat("rotation", 0, 360, 0);

            //Creamos un Box3D para que se vea como el Sprite es en 2D y se dibuja siempre arriba de la escena 3D
            box = TgcBox.fromSize(new Vector3(10, 10, 10),
                TgcTexture.createTexture(GuiController.Instance.ExamplesMediaDir + "\\Texturas\\pasto.jpg"));

            //Hacer que la camara se centre en el box3D
            GuiController.Instance.RotCamera.targetObject(box.BoundingBox);
        }

        public override void render(float elapsedTime)
        {
            //Actualizar valores cargados en modifiers
            animatedSprite.setFrameRate((float)GuiController.Instance.Modifiers["frameRate"]);
            animatedSprite.Position = (Vector2)GuiController.Instance.Modifiers["position"];
            animatedSprite.Scaling = (Vector2)GuiController.Instance.Modifiers["scaling"];
            animatedSprite.Rotation = FastMath.ToRad((float)GuiController.Instance.Modifiers["rotation"]);

            //Dibujar box3D. Se deben dibujar primero todos los objetos 3D. Recien al final dibujar los Sprites
            box.render();

            //Iniciar dibujado de todos los Sprites de la escena (en este caso es solo uno)
            TgcDrawer2D.Instance.beginDrawSprite();

            //Dibujar sprite (si hubiese mas, deberian ir todos aqu�)
            //Actualizamos el estado de la animacion y renderizamos
            animatedSprite.updateAndRender(elapsedTime);

            //Finalizar el dibujado de Sprites
            TgcDrawer2D.Instance.endDrawSprite();
        }

        public override void close()
        {
            animatedSprite.dispose();
            box.dispose();
        }
    }
}