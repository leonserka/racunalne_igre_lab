using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip[] footstepClips;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip fireClip;
    public float footstepVolume = 0.7f;
    public float actionVolume = 0.9f;

    [Header("Particles")]
    public ParticleSystem landingDust;
    public ParticleSystem muzzleFlash;
    public Material dustMaterial;
    public Material muzzleFlashMaterial;
    public Transform muzzlePoint;

    [Header("References")]
    public GameObject weapon;

    private AudioSource audioSource;
    private int footstepIndex;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.2f;

        LoadDefaultAudioClips();
        CreateFallbackMaterials();
        EnsureLandingDust();
        EnsureMuzzleFlash();
    }

    void LoadDefaultAudioClips()
    {
        if (footstepClips == null || footstepClips.Length == 0)
        {
            footstepClips = new[]
            {
                Resources.Load<AudioClip>("Audio/Footstep_01"),
                Resources.Load<AudioClip>("Audio/Footstep_02")
            };
        }

        if (jumpClip == null)
            jumpClip = Resources.Load<AudioClip>("Audio/Jump");

        if (landClip == null)
            landClip = Resources.Load<AudioClip>("Audio/Land");

        if (fireClip == null)
            fireClip = Resources.Load<AudioClip>("Audio/Fire");
    }

    void EnsureLandingDust()
    {
        if (landingDust == null)
            landingDust = transform.Find("LandingDust")?.GetComponent<ParticleSystem>();

        if (landingDust == null)
        {
            GameObject dustObject = new GameObject("LandingDust");
            dustObject.transform.SetParent(transform, false);
            dustObject.transform.localPosition = new Vector3(0f, -0.9f, 0f);

            landingDust = dustObject.AddComponent<ParticleSystem>();
        }

        ConfigureDust(landingDust);
    }

    void EnsureMuzzleFlash()
    {
        if (weapon == null)
            weapon = GameObject.Find("Weapon");

        Transform parent = muzzlePoint != null ? muzzlePoint : (weapon != null ? weapon.transform : transform);

        if (muzzleFlash == null)
            muzzleFlash = parent.Find("MuzzleFlash")?.GetComponent<ParticleSystem>();

        if (muzzleFlash == null)
        {
            GameObject flashObject = new GameObject("MuzzleFlash");
            flashObject.transform.SetParent(parent, false);
            flashObject.transform.localPosition = new Vector3(0f, 0f, 0.45f);
            flashObject.transform.localRotation = Quaternion.identity;

            muzzleFlash = flashObject.AddComponent<ParticleSystem>();
        }

        muzzleFlash.transform.localScale = new Vector3(0.08f, 0.08f, 0.25f);
        ConfigureMuzzleFlash(muzzleFlash);
    }

    void CreateFallbackMaterials()
    {
        Shader particleShader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        if (particleShader == null)
            particleShader = Shader.Find("Universal Render Pipeline/Unlit");
        if (particleShader == null)
            particleShader = Shader.Find("Sprites/Default");

        if (dustMaterial == null && particleShader != null)
        {
            dustMaterial = new Material(particleShader);
            dustMaterial.name = "RuntimeDustParticle";
            dustMaterial.color = new Color(0.55f, 0.5f, 0.42f, 0.35f);
        }

        if (muzzleFlashMaterial == null && particleShader != null)
        {
            muzzleFlashMaterial = new Material(particleShader);
            muzzleFlashMaterial.name = "RuntimeMuzzleFlashParticle";
            muzzleFlashMaterial.color = new Color(1f, 0.58f, 0.12f, 0.9f);
        }
    }

    void ConfigureDust(ParticleSystem particles)
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = particles.main;
        main.duration = 0.25f;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = 0.35f;
        main.startSpeed = 1.4f;
        main.startSize = 0.35f;
        main.startColor = new Color(0.7f, 0.63f, 0.5f, 0.5f);
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = true;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 16) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.55f;

        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.sharedMaterial = dustMaterial;

    }

    void ConfigureMuzzleFlash(ParticleSystem particles)
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ParticleSystem.MainModule main = particles.main;
        main.duration = 0.08f;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = 0.07f;
        main.startSpeed = 0.8f;
        main.startSize = 0.25f;
        main.startColor = new Color(1f, 0.74f, 0.25f, 1f);

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.enabled = true;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 10) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 18f;
        shape.radius = 0.05f;

        ParticleSystemRenderer renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.sharedMaterial = muzzleFlashMaterial;

    }

    public void PlayFootstep()
    {
        if (footstepClips == null || footstepClips.Length == 0 || audioSource == null)
            return;

        AudioClip clip = footstepClips[footstepIndex % footstepClips.Length];
        footstepIndex++;

        if (clip != null)
            audioSource.PlayOneShot(clip, footstepVolume);
    }

    public void PlayJump()
    {
        PlayActionClip(jumpClip);
    }

    public void PlayLand()
    {
        PlayActionClip(landClip);

        if (landingDust != null)
            landingDust.Play();
    }

    public void FireWeapon()
    {
        PlayActionClip(fireClip);

        if (muzzleFlash != null)
            muzzleFlash.Play();
    }

    void PlayActionClip(AudioClip clip)
    {
        if (clip != null && audioSource != null)
            audioSource.PlayOneShot(clip, actionVolume);
    }
}
