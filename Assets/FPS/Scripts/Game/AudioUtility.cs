using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Unity.FPS.Game
{
    /// <summary>
    /// ����� �÷��� ���� ��� ����
    /// </summary>
    public class AudioUtility : MonoBehaviour
    {
        #region Variables

        #endregion


        //������ ��ġ�� ���ӿ�����Ʈ �����ϰ� AudioSource ������Ʈ�� �߰��ؼ� ������ Ŭ���� �÷����Ѵ�
        //Ŭ�� ���� �÷��̰� ������ �ڵ����� ų�Ѵ� - TimeselfDestruct ������Ʈ �̿� 
        public static void CreateSfx(AudioClip clip, Vector3 position, float spartialBlend, float rolloffDistanceMin = 1f)
        {
            GameObject impactSfxInstrance = new GameObject();
            impactSfxInstrance.transform.position = position;

            AudioSource source = impactSfxInstrance.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = spartialBlend;
            source.minDistance = rolloffDistanceMin;
            source.Play();

            //������Ʈ kill
            TimeSelfDestruct timeSelfDestruct = impactSfxInstrance.AddComponent<TimeSelfDestruct>();
            timeSelfDestruct.lifeTime = clip.length;
        }
    }
}