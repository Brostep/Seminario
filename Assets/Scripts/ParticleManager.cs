using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static ParticleManager _instance;
    public static ParticleManager Instance { get { return _instance; } }

    //Agregar partículas desde el Inspector.
    public List<GameObject> particles = new List<GameObject>();

    //Diccionario de Pools.
    private Dictionary<int, Pool<GameObject>> _dicPool = new Dictionary<int, Pool<GameObject>>();

    //Diccionario de métodos.
    private Dictionary<int, Pool<GameObject>.CallbackFactory> _dicCallBacks = new Dictionary<int, Pool<GameObject>.CallbackFactory>();

    #region Names of Particles
    public const int GROUND_CRACKS = 0;
    public const int BLOOD_DEAD_SPAWNER_EFFECT = 1;


    #endregion

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        _dicCallBacks.Add(GROUND_CRACKS, GroundCracksParticlesFactory);
        _dicCallBacks.Add(BLOOD_DEAD_SPAWNER_EFFECT, BloodDeadSpawnerEffectFactory);
    }

    public GameObject GetParticle(int name)
    {
        if (!_dicPool.ContainsKey(name))
        {
            var _aux = new Pool<GameObject>(3, _dicCallBacks[name], InitializePool, DisposePool, true);

            _dicPool[name] = _aux;
        }

        return _dicPool[name].GetObjectFromPool();
    }

    public void InitializePool(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void DisposePool(GameObject obj)
    {
        obj.SetActive(false);
    }

    #region Particles CallBacks

    private GameObject GroundCracksParticlesFactory()
    {
        return Instantiate<GameObject>(particles[GROUND_CRACKS]);
    }

    private GameObject BloodDeadSpawnerEffectFactory()
    {
        return Instantiate<GameObject>(particles[BLOOD_DEAD_SPAWNER_EFFECT]);
    }

    #endregion
}
