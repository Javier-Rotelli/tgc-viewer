using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TgcViewer;
using TgcViewer.Utils.TgcSceneLoader;
using TGC.Core.Example;

namespace Examples
{
    /// <summary>
    ///     EjemploBatchPrimitivesDx
    /// </summary>
    public class EjemploBatchPrimitivesDx : TgcExample
    {
        private const float boxSize = 3f;
        private const int boxPerSquare = 50;
        private TgcTexture box1Texture;
        private TgcTexture box2Texture;
        private TgcTexture box3Texture;

        private RenderMethod currentRenderMethod;
        private Mesh[] meshes;
        private readonly int totalBoxes = boxPerSquare*boxPerSquare;

        public override string getCategory()
        {
            return "Otros";
        }

        public override string getName()
        {
            return "BatchPrimitivesDx";
        }

        public override string getDescription()
        {
            return "BatchPrimitivesDx";
        }

        public override void init()
        {
            var d3dDevice = GuiController.Instance.D3dDevice;

            box1Texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice,
                GuiController.Instance.ExamplesMediaDir + "Texturas\\pasto.jpg");
            box2Texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice,
                GuiController.Instance.ExamplesMediaDir + "Texturas\\tierra.jpg");
            box3Texture = TgcTexture.createTexture(GuiController.Instance.D3dDevice,
                GuiController.Instance.ExamplesMediaDir + "Texturas\\madera.jpg");

            GuiController.Instance.Modifiers.addEnum("Render Method", typeof (RenderMethod), RenderMethod.Unsorted);
            createMeshes(d3dDevice);

            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.FpsCamera.setCamera(new Vector3(32.1944f, 42.1327f, -68.7882f),
                new Vector3(265.5333f, -258.1551f, 856.0794f));
        }

        private void createMeshes(Device d3dDevice)
        {
            meshes = new Mesh[totalBoxes];
            for (var i = 0; i < meshes.Length; i++)
            {
                meshes[i] = Mesh.Box(d3dDevice, boxSize, boxSize, boxSize);
            }
        }

        private void doRender(RenderMethod renderMethod)
        {
            if (currentRenderMethod != renderMethod)
            {
                currentRenderMethod = renderMethod;
            }

            switch (currentRenderMethod)
            {
                case RenderMethod.Unsorted:
                    doUnsortedRender();
                    break;

                case RenderMethod.Texture_Sort:
                    doTextureSortRender();
                    break;
            }
        }

        /// <summary>
        ///     Renderizar haciendo que se tenga que alternar la textura a propůsito
        /// </summary>
        private void doUnsortedRender()
        {
            var d3dDevice = GuiController.Instance.D3dDevice;

            for (var i = 0; i < boxPerSquare; i++)
            {
                for (var j = 0; j < boxPerSquare; j++)
                {
                    d3dDevice.GetTexture(0);

                    //Forzar a proposito el cambio de textura
                    d3dDevice.SetTexture(0, null);
                    d3dDevice.SetTexture(0, box3Texture.D3dTexture);
                    d3dDevice.SetTexture(0, box2Texture.D3dTexture);
                    d3dDevice.SetTexture(0, box1Texture.D3dTexture);

                    d3dDevice.SetTexture(0, box1Texture.D3dTexture);

                    d3dDevice.Transform.World = Matrix.Translation(boxSize*2*i, 0, boxSize*2*j);
                    meshes[i].DrawSubset(0);
                }
            }
        }

        /// <summary>
        ///     Renderizar primero las de la textura 1 y despues la de textura 2, para minimizar los Texture State Change
        /// </summary>
        private void doTextureSortRender()
        {
            var d3dDevice = GuiController.Instance.D3dDevice;

            //Un solo texture change
            d3dDevice.SetTexture(0, box1Texture.D3dTexture);

            for (var i = 0; i < boxPerSquare; i++)
            {
                for (var j = 0; j < boxPerSquare; j++)
                {
                    d3dDevice.Transform.World = Matrix.Translation(boxSize*2*i, 0, boxSize*2*j);
                    meshes[i].DrawSubset(0);
                }
            }
        }

        public override void render(float elapsedTime)
        {
            var d3dDevice = GuiController.Instance.D3dDevice;

            var renderMethod = (RenderMethod) GuiController.Instance.Modifiers["Render Method"];
            doRender(renderMethod);
        }

        public override void close()
        {
            currentRenderMethod = RenderMethod.Unsorted;
            disposeCajas();
        }

        /// <summary>
        ///     Liberar recursos de cajas
        /// </summary>
        private void disposeCajas()
        {
            for (var i = 0; i < meshes.Length; i++)
            {
                meshes[i].Dispose();
            }
        }

        private enum RenderMethod
        {
            Unsorted,
            Texture_Sort
        }
    }
}