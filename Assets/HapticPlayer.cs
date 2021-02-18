using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
public class HapticPlayer : MonoBehaviour
{
    [SerializeField] HapticTypes completeHaptic = HapticTypes.RigidImpact;
    [SerializeField] HapticTypes timeOverHaptic = HapticTypes.Success;
 
    public void PlayCompleteHaptic()
    {
        MMVibrationManager.Haptic(completeHaptic);
    }

    public void PlayTimeOverHaptic()
    {
       MMVibrationManager.Haptic(timeOverHaptic); 
    }
}
