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
    public const int BLOOD_GULA_HIT_EFFECT = 1;
    public const int BULLET_NULL_PARTICLE = 2;


    #endregion

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        _dicCallBacks.Add(GROUND_CRACKS, GroundCracksParticlesFactory);
        _dicCallBacks.Add(BLOOD_GULA_HIT_EFFECT, BloodGulaHitEffect);
        _dicCallBacks.Add(BULLET_NULL_PARTICLE, BulletNullParticle);
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

    public void ReturnParticle(int name, GameObject obj)
    {
        _dicPool[name].DisablePoolObject(obj);
    }

    #region Particles CallBacks

    private GameObject GroundCracksParticlesFactory()
    {
        return Instantiate<GameObject>(particles[GROUND_CRACKS]);
    }

    private GameObject BloodGulaHitEffect()
    {
        return Instantiate<GameObject>(particles[BLOOD_GULA_HIT_EFFECT]);
    }

    private GameObject BulletNullParticle()
    {
        return Instantiate<GameObject>(particles[BULLET_NULL_PARTICLE]);
    }

    #endregion
}
