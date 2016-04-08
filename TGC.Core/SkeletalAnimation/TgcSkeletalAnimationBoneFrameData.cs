namespace TGC.Core.SkeletalAnimation
{
    /// <summary>
    ///     KeyFrame de animaci�n de un hueso, para una animaci�n particular
    /// </summary>
    public class TgcSkeletalAnimationBoneFrameData
    {
        //Numero de frame en el que ocurre la animacion
        public int frame;

        public float[] position;
        public float[] rotation;
    }
}