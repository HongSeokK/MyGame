using UnityEngine;

namespace ManeProject.Domain.Box
{

    public interface Box
    {

        Position Position { get; }

        BoxType Type { get; }
    }
}