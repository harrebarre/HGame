using HGame.HShapes;

namespace HGame.HInterfaces
{
    internal interface ICorporeal : ISpatial
    {
        Shape Shape { get; set; }
    }
}