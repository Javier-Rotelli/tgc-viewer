using Microsoft.DirectX;
using TGC.Core.Example;
using TGC.Util;

namespace TGC.Examples.RoomsEditor
{
    /// <summary>
    ///     Ejemplo Caja
    ///     Unidades Involucradas:
    ///     # Unidad 3 - Conceptos B�sicos de 3D - Mesh
    ///     # Unidad 7 - T�cnicas de Optimizaci�n - Indoor
    ///     Herramienta para crear escenarios Indoor compuestos por cuartos rectangulares
    ///     que se comunican entre s�.
    ///     Permite crear cuartos rectangulares en un plano 2D con vista superior y, a partir
    ///     de este plano, genera el escenario 3D.
    ///     Calcula autom�ticamente los lados de los rect�ngulos que tocan entre s� y genera
    ///     las aberturas necesarias que permien comunicar ambos cuartos (simulando puertas o ventanas)
    ///     Las instrucciones se muestran al hacer clic en el bot�n "Help" de este Modifier.
    ///     Autor: Mat�as Leone, Leandro Barbagallo
    /// </summary>
    public class TgcRoomsEditor : TgcExample
    {
        private RoomsEditorModifier modifier;

        public override string getCategory()
        {
            return "Utils";
        }

        public override string getName()
        {
            return "RoomsEditor";
        }

        public override string getDescription()
        {
            return
                "Herramienta para crear escenarios Indoor compuestos por cuartos rectangulares que se comunican entre s�.";
        }

        public override void init()
        {
            modifier = new RoomsEditorModifier("RoomsEditor", this);
            GuiController.Instance.Modifiers.add(modifier);

            GuiController.Instance.FpsCamera.Enable = true;
            GuiController.Instance.FpsCamera.MovementSpeed = 200f;
            GuiController.Instance.FpsCamera.JumpSpeed = 200f;
            GuiController.Instance.FpsCamera.setCamera(new Vector3(133.0014f, 264.8258f, -119.0311f),
                new Vector3(498.1584f, -299.4199f, 621.433f));
        }

        public override void render(float elapsedTime)
        {
            foreach (var room in modifier.Rooms)
            {
                foreach (var wall in room.Walls)
                {
                    wall.render();
                }
            }
        }

        public override void close()
        {
            modifier.dispose();
        }

        /// <summary>
        ///     M�todo que se llama cuando se quiere exportar la informacion de la escena a un XML,
        ///     a trav�s del bot�n "Custom Export"
        ///     MODIFICAR ESTA SECCION PARA ADAPTARSE A LAS NECESIDADES DEL ALUMNO
        /// </summary>
        internal void customExport(string savePath)
        {
        }
    }
}