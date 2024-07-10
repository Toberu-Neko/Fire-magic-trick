using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

/// <summary>
/// <para>功用: 呼叫已設置的共用粒子</para>
///<para>使用方式: 在外面做好粒子後,就可以呼叫 PublicParticleSystem.PlayParticle?.Invoke();</para>
/// </summary>
public class PublicParticleSystem : MonoBehaviour
{
    [SerializeField] List<ParticlePack> Particles;
    [SerializeField] GameObject TextBase;
    [SerializeField] GameObject TextFolder;
   readonly Dictionary<TextMeshProUGUI, bool> Texts = new();

    // 設置新共用粒子時先寫入寫入粒子種類
    public enum ParticleType
    {
        None,
        MonsterHurt,

    }

    //可呼叫功能

    /// <summary>播放粒子(位置,種類)</summary>
    public static Action<Vector2, ParticleType> PlayParticle { get; private set; }
    /// <summary>播放文字(位置,內容,字體)</summary>
    public static Action<Vector2, string, TMP_FontAsset> PlayText { get; private set; }
    /// <summary>播放客製粒子(位置,角度,種類,大小,顏色,種類)</summary>
    public static Action<Vector2, float, Vector2, Color, ParticleType> PlayCustomParticle { get; private set; }


    #region 火星文

    private void Awake()
    {
        PlayParticle += FindAndPlayParticle;
        PlayCustomParticle += FindAndPlayCustomSetParticle;
        PlayText += FindAndPlayText;
    }
    private void OnDestroy()
    {
        PlayParticle -= FindAndPlayParticle;
        PlayCustomParticle -= FindAndPlayCustomSetParticle;
        PlayText -= FindAndPlayText;
    }

    void FindAndPlayText(Vector2 Position, string Text, TMP_FontAsset asset)
    {
        TextMeshProUGUI text = GetText();
        Texts[text] = true;
        text.transform.position = Position;
        text.text = Text;
        text.font = asset;
        StartCoroutine(DamageNumberAnimation(text));
    }
    TextMeshProUGUI GetText()
    {
        if (Texts.Count > 0)
        {
            foreach (TextMeshProUGUI text in Texts.Keys)
            {
                if (!Texts[text])
                {
                    return text;
                }
            }
        }

        GameObject New = Instantiate(TextBase, Vector2.one * -100, Quaternion.identity, TextFolder.transform);

        TextMeshProUGUI NewText = New.GetComponent<TextMeshProUGUI>();
        Texts.Add(NewText, true);
        New.SetActive(true);
        return NewText;
    }

    //文字動畫,可改雪
    IEnumerator DamageNumberAnimation(TextMeshProUGUI text)
    {
        text.transform.localScale = Vector2.zero;

        text.GetComponent<Rigidbody2D>().AddForce(UnityEngine.Random.insideUnitCircle * 3f, ForceMode2D.Impulse);
        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            text.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one * 0.015f, i / 0.2f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        for (float i = 0; i < 0.05f; i += Time.deltaTime)
        {
            text.transform.localScale = Vector2.Lerp(Vector2.one * 0.015f, Vector2.one * 0.01f, i / 0.05f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(2);
        for (float i = 0; i < 0.2f; i += Time.deltaTime)
        {
            text.transform.localScale = Vector2.Lerp(Vector2.one * 0.01f, Vector2.zero, i / 0.2f);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        text.transform.localScale = Vector2.zero;
        Texts[text] = false;
    }



    ParticleSystem GetParticle(ParticleType type)
    {
        foreach (ParticlePack pack in Particles)
        {
            if (pack.Type == type && pack.Particle != null)
            {
                return pack.Particle;
            }
        }
        return null;
    }
    void FindAndPlayParticle(Vector2 position, ParticleType type)
    {

        ParticleSystem particle = GetParticle(type);
        particle.transform.position = position;
        particle.Play(true);
    }
    void FindAndPlayCustomSetParticle(Vector2 position, float Rotation, Vector2 Scale, Color color, ParticleType type)
    {
        GameObject FoundParticle = null;
        foreach (ParticlePack pack in Particles)
        {
            if (pack.Type == type && pack.Particle != null)
            {
                FoundParticle = pack.Particle.gameObject;
                break;
            }
        }
        if (FoundParticle != null)
        {
            GameObject NewCopy = Instantiate(FoundParticle, position, Quaternion.identity);

            ParticleSystem.MainModule particle = NewCopy.GetComponent<ParticleSystem>().main;

            particle.startColor = color;
            particle.startRotation = Rotation;
            NewCopy.transform.localScale = Scale;
            NewCopy.GetComponent<ParticleSystem>().Play(true);
            Destroy(NewCopy, particle.startLifetime.constantMax);
        }

    }

    [System.Serializable]
    class ParticlePack
    {
        public ParticleType Type = ParticleType.None;
        public ParticleSystem Particle = null;
    }
    #endregion

}
