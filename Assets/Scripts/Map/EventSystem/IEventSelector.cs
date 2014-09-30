using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace Assets.Scripts.Map.EventSystem
{
    public interface IEventSelector
    {
        IEnumerable<GameObject> GetObjects();
    }
}
