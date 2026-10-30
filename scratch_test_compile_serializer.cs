using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using ColossalFramework;

public static class TestSerializer
{
    private static byte[] s_pendingHighIdTMPEData;

    public static void SetPendingHighIdTMPEData(byte[] data)
    {
        s_pendingHighIdTMPEData = data;
    }

    public static Assembly GetTMPEAssembly()
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (asm.GetName().Name == "TrafficManager") return asm;
        }
        return null;
    }

    public static Assembly GetTMPECommonsAssembly()
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (asm.GetName().Name == "CSUtil.Commons") return asm;
        }
        return null;
    }

    public static Assembly GetTMPEAPIAssembly()
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (asm.GetName().Name == "TMPE.API") return asm;
        }
        return null;
    }

    public static object GetManagerInstance(Type managerType)
    {
        PropertyInfo prop = managerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        if (prop != null) return prop.GetValue(null, null);
        FieldInfo field = managerType.GetField("Instance", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        if (field != null) return field.GetValue(null);
        return null;
    }

    public static byte[] SerializeHighIdTMPEData()
    {
        try
        {
            Assembly tmpe = GetTMPEAssembly();
            if (tmpe == null) return null;

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write((uint)0x4D4E4354); // Magic: 'MNCT'
                bw.Write((byte)1);          // Version 1

                // 1. Junction Restrictions (Segments 36,864 .. 65,023)
                Type jrmType = tmpe.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
                object jrm = jrmType != null ? GetManagerInstance(jrmType) : null;
                List<byte[]> jrRecords = new List<byte[]>();

                if (jrm != null)
                {
                    MethodInfo mGetUturn = jrmType.GetMethod("GetUturnAllowed", new Type[] { typeof(ushort), typeof(bool) });
                    MethodInfo mGetBlocked = jrmType.GetMethod("GetEnteringBlockedJunctionAllowed", new Type[] { typeof(ushort), typeof(bool) });
                    MethodInfo mGetPed = jrmType.GetMethod("GetPedestrianCrossingAllowed", new Type[] { typeof(ushort), typeof(bool) });
                    MethodInfo mGetStraight = jrmType.GetMethod("GetLaneChangingAllowedWhenGoingStraight", new Type[] { typeof(ushort), typeof(bool) });
                    MethodInfo mGetNear = jrmType.GetMethod("GetNearTurnOnRedAllowed", new Type[] { typeof(ushort), typeof(bool) });
                    MethodInfo mGetFar = jrmType.GetMethod("GetFarTurnOnRedAllowed", new Type[] { typeof(ushort), typeof(bool) });

                    var nm = Singleton<NetManager>.instance;
                    NetSegment[] segBuffer = (nm != null && nm.m_segments != null) ? nm.m_segments.m_buffer : null;

                    if (segBuffer != null && mGetUturn != null)
                    {
                        object[] args = new object[2];
                        for (int s = 36864; s < 65024 && s < segBuffer.Length; s++)
                        {
                            if ((segBuffer[s].m_flags & NetSegment.Flags.Created) == 0) continue;

                            for (int end = 0; end < 2; end++)
                            {
                                bool startNode = (end == 0);
                                args[0] = (ushort)s;
                                args[1] = startNode;

                                byte uturn = Convert.ToByte(mGetUturn.Invoke(jrm, args));
                                byte blocked = mGetBlocked != null ? Convert.ToByte(mGetBlocked.Invoke(jrm, args)) : (byte)0;
                                byte ped = mGetPed != null ? Convert.ToByte(mGetPed.Invoke(jrm, args)) : (byte)0;
                                byte straight = mGetStraight != null ? Convert.ToByte(mGetStraight.Invoke(jrm, args)) : (byte)0;
                                byte near = mGetNear != null ? Convert.ToByte(mGetNear.Invoke(jrm, args)) : (byte)0;
                                byte far = mGetFar != null ? Convert.ToByte(mGetFar.Invoke(jrm, args)) : (byte)0;

                                if (uturn != 0 || blocked != 0 || ped != 0 || straight != 0 || near != 0 || far != 0)
                                {
                                    using (MemoryStream rMs = new MemoryStream())
                                    using (BinaryWriter rBw = new BinaryWriter(rMs))
                                    {
                                        rBw.Write((ushort)s);
                                        rBw.Write(startNode);
                                        rBw.Write(uturn);
                                        rBw.Write(blocked);
                                        rBw.Write(ped);
                                        rBw.Write(straight);
                                        rBw.Write(near);
                                        rBw.Write(far);
                                        jrRecords.Add(rMs.ToArray());
                                    }
                                }
                            }
                        }
                    }
                }

                bw.Write((int)jrRecords.Count);
                for (int i = 0; i < jrRecords.Count; i++)
                {
                    bw.Write(jrRecords[i]);
                }

                // 2. Priority Signs (Segments 36,864 .. 65,023)
                Type tpmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
                object tpm = tpmType != null ? GetManagerInstance(tpmType) : null;
                List<byte[]> psRecords = new List<byte[]>();

                if (tpm != null)
                {
                    MethodInfo mGetSign = tpmType.GetMethod("GetPrioritySign", new Type[] { typeof(ushort), typeof(bool) });
                    var nm = Singleton<NetManager>.instance;
                    NetSegment[] segBuffer = (nm != null && nm.m_segments != null) ? nm.m_segments.m_buffer : null;

                    if (segBuffer != null && mGetSign != null)
                    {
                        object[] args = new object[2];
                        for (int s = 36864; s < 65024 && s < segBuffer.Length; s++)
                        {
                            if ((segBuffer[s].m_flags & NetSegment.Flags.Created) == 0) continue;

                            for (int end = 0; end < 2; end++)
                            {
                                bool startNode = (end == 0);
                                args[0] = (ushort)s;
                                args[1] = startNode;

                                object signObj = mGetSign.Invoke(tpm, args);
                                byte sign = Convert.ToByte(signObj);
                                if (sign != 0)
                                {
                                    using (MemoryStream rMs = new MemoryStream())
                                    using (BinaryWriter rBw = new BinaryWriter(rMs))
                                    {
                                        rBw.Write((ushort)s);
                                        rBw.Write(startNode);
                                        rBw.Write(sign);
                                        psRecords.Add(rMs.ToArray());
                                    }
                                }
                            }
                        }
                    }
                }

                bw.Write((int)psRecords.Count);
                for (int i = 0; i < psRecords.Count; i++)
                {
                    bw.Write(psRecords[i]);
                }

                // 3. Timed Traffic Lights (Nodes 32,768 .. 65,023)
                Type tlsmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
                object tlsm = tlsmType != null ? GetManagerInstance(tlsmType) : null;
                byte[] ttlBytes = null;

                if (tlsm != null)
                {
                    Type ttlCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLights");
                    Type stepCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLightsStep");
                    Type cslCfgType = tmpe.GetType("TrafficManager.Configuration+CustomSegmentLights");
                    Type cslLightCfgType = tmpe.GetType("TrafficManager.Configuration+CustomSegmentLight");

                    FieldInfo fSims = tlsmType.GetField("<TrafficLightSimulations>k__BackingField", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                   ?? tlsmType.GetField("TrafficLightSimulations", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    Array sims = fSims != null ? fSims.GetValue(tlsm) as Array : null;

                    MethodInfo mHasTimed = tlsmType.GetMethod("HasTimedSimulation", new Type[] { typeof(ushort) });

                    if (sims != null && mHasTimed != null && ttlCfgType != null && stepCfgType != null && cslCfgType != null && cslLightCfgType != null)
                    {
                        Type listType = typeof(List<>).MakeGenericType(ttlCfgType);
                        IList ttlList = (IList)Activator.CreateInstance(listType);

                        Type tlsType = tmpe.GetType("TrafficManager.TrafficLight.Impl.TrafficLightSimulation");
                        FieldInfo fTimedLight = tlsType != null ? tlsType.GetField("timedLight", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) : null;

                        object[] hasArgs = new object[1];
                        for (int n = 32768; n < 65024 && n < sims.Length; n++)
                        {
                            hasArgs[0] = (ushort)n;
                            bool has = (bool)mHasTimed.Invoke(tlsm, hasArgs);
                            if (!has) continue;

                            object sim = sims.GetValue(n);
                            if (sim == null) continue;

                            object timedLight = fTimedLight != null ? fTimedLight.GetValue(sim) : null;
                            if (timedLight == null) continue;

                            object ttlObj = Activator.CreateInstance(ttlCfgType);
                            ttlCfgType.GetField("nodeId").SetValue(ttlObj, (ushort)n);

                            PropertyInfo pGroup = timedLight.GetType().GetProperty("NodeGroup");
                            IEnumerable groupEnum = pGroup != null ? pGroup.GetValue(timedLight, null) as IEnumerable : null;
                            List<ushort> groupList = new List<ushort>();
                            if (groupEnum != null)
                            {
                                foreach (var gId in groupEnum) groupList.Add(Convert.ToUInt16(gId));
                            }
                            if (groupList.Count == 0) groupList.Add((ushort)n);
                            ttlCfgType.GetField("nodeGroup").SetValue(ttlObj, groupList);

                            MethodInfo mIsStarted = timedLight.GetType().GetMethod("IsStarted", Type.EmptyTypes);
                            bool isStarted = mIsStarted != null && (bool)mIsStarted.Invoke(timedLight, null);
                            ttlCfgType.GetField("started").SetValue(ttlObj, isStarted);

                            PropertyInfo pCurStep = timedLight.GetType().GetProperty("CurrentStep");
                            int curStep = pCurStep != null ? (int)pCurStep.GetValue(timedLight, null) : 0;
                            ttlCfgType.GetField("currentStep").SetValue(ttlObj, curStep);

                            Type stepListType = typeof(List<>).MakeGenericType(stepCfgType);
                            IList stepList = (IList)Activator.CreateInstance(stepListType);
                            ttlCfgType.GetField("timedSteps").SetValue(ttlObj, stepList);

                            MethodInfo mNumSteps = timedLight.GetType().GetMethod("NumSteps", Type.EmptyTypes);
                            MethodInfo mGetStep = timedLight.GetType().GetMethod("GetStep", new Type[] { typeof(int) });
                            int numSteps = mNumSteps != null ? (int)mNumSteps.Invoke(timedLight, null) : 0;

                            for (int stepIdx = 0; stepIdx < numSteps; stepIdx++)
                            {
                                object step = mGetStep.Invoke(timedLight, new object[] { stepIdx });
                                if (step == null) continue;

                                object stepObj = Activator.CreateInstance(stepCfgType);
                                stepCfgType.GetField("minTime").SetValue(stepObj, Convert.ToInt32(step.GetType().GetProperty("MinTime").GetValue(step, null)));
                                stepCfgType.GetField("maxTime").SetValue(stepObj, Convert.ToInt32(step.GetType().GetProperty("MaxTime").GetValue(step, null)));
                                stepCfgType.GetField("changeMetric").SetValue(stepObj, Convert.ToInt32(step.GetType().GetProperty("ChangeMetric").GetValue(step, null)));
                                stepCfgType.GetField("waitFlowBalance").SetValue(stepObj, Convert.ToSingle(step.GetType().GetProperty("WaitFlowBalance").GetValue(step, null)));

                                Type cslDictType = typeof(Dictionary<,>).MakeGenericType(typeof(ushort), cslCfgType);
                                IDictionary cslDict = (IDictionary)Activator.CreateInstance(cslDictType);
                                stepCfgType.GetField("segmentLights").SetValue(stepObj, cslDict);

                                PropertyInfo pCsl = step.GetType().GetProperty("CustomSegmentLights");
                                IEnumerable cslEnum = pCsl != null ? pCsl.GetValue(step, null) as IEnumerable : null;
                                if (cslEnum != null)
                                {
                                    foreach (var cslItem in cslEnum)
                                    {
                                        PropertyInfo pVal = cslItem.GetType().GetProperty("Value");
                                        object csl = pVal != null ? pVal.GetValue(cslItem, null) : cslItem;
                                        if (csl == null) continue;

                                        ushort cslNodeId = Convert.ToUInt16(csl.GetType().GetProperty("NodeId").GetValue(csl, null));
                                        ushort cslSegId = Convert.ToUInt16(csl.GetType().GetProperty("SegmentId").GetValue(csl, null));

                                        object cslObj = Activator.CreateInstance(cslCfgType);
                                        cslCfgType.GetField("nodeId").SetValue(cslObj, cslNodeId);
                                        cslCfgType.GetField("segmentId").SetValue(cslObj, cslSegId);
                                        cslCfgType.GetField("manualPedestrianMode").SetValue(cslObj, Convert.ToBoolean(csl.GetType().GetProperty("ManualPedestrianMode").GetValue(csl, null)));

                                        object pedState = csl.GetType().GetProperty("PedestrianLightState").GetValue(csl, null);
                                        cslCfgType.GetField("pedestrianLightState").SetValue(cslObj, pedState);

                                        IDictionary lightDict = (IDictionary)Activator.CreateInstance(cslCfgType.GetField("customLights").FieldType);
                                        cslCfgType.GetField("customLights").SetValue(cslObj, lightDict);

                                        PropertyInfo pLights = csl.GetType().GetProperty("CustomLights");
                                        IEnumerable lightsEnum = pLights != null ? pLights.GetValue(csl, null) as IEnumerable : null;
                                        if (lightsEnum != null)
                                        {
                                            foreach (var lightEntry in lightsEnum)
                                            {
                                                object key = lightEntry.GetType().GetProperty("Key").GetValue(lightEntry, null);
                                                object light = lightEntry.GetType().GetProperty("Value").GetValue(lightEntry, null);
                                                if (light == null) continue;

                                                int mode = Convert.ToInt32(light.GetType().GetProperty("CurrentMode").GetValue(light, null));
                                                object leftLight = light.GetType().GetProperty("LightLeft").GetValue(light, null);
                                                object mainLight = light.GetType().GetProperty("LightMain").GetValue(light, null);
                                                object rightLight = light.GetType().GetProperty("LightRight").GetValue(light, null);

                                                ConstructorInfo cslCtor = cslLightCfgType.GetConstructors()[0];
                                                object lightObj = cslCtor.Invoke(new object[] { cslNodeId, cslSegId, mode, leftLight, mainLight, rightLight });

                                                lightDict.Add(key, lightObj);
                                            }
                                        }

                                        cslDict.Add(cslSegId, cslObj);
                                    }
                                }

                                stepList.Add(stepObj);
                            }

                            ttlList.Add(ttlObj);
                        }

                        if (ttlList.Count > 0)
                        {
                            using (MemoryStream ttlStream = new MemoryStream())
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                bf.Serialize(ttlStream, ttlList);
                                ttlBytes = ttlStream.ToArray();
                            }
                        }
                    }
                }

                if (ttlBytes != null && ttlBytes.Length > 0)
                {
                    bw.Write((int)ttlBytes.Length);
                    bw.Write(ttlBytes);
                }
                else
                {
                    bw.Write((int)0);
                }

                if (jrRecords.Count == 0 && psRecords.Count == 0 && (ttlBytes == null || ttlBytes.Length == 0))
                {
                    return null;
                }

                return ms.ToArray();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[MoreNetworkCapacityRenew] SerializeHighIdTMPEData error: " + ex);
            return null;
        }
    }

    public static void RestorePendingHighIdTMPEData()
    {
        if (s_pendingHighIdTMPEData == null || s_pendingHighIdTMPEData.Length < 5)
            return;

        try
        {
            Assembly tmpe = GetTMPEAssembly();
            if (tmpe == null) return;

            using (MemoryStream ms = new MemoryStream(s_pendingHighIdTMPEData))
            using (BinaryReader br = new BinaryReader(ms))
            {
                uint magic = br.ReadUInt32();
                if (magic != 0x4D4E4354) return;
                byte ver = br.ReadByte();

                // 1. Restore Junction Restrictions
                int jrCount = br.ReadInt32();
                Type jrmType = tmpe.GetType("TrafficManager.Manager.Impl.JunctionRestrictionsManager");
                object jrm = jrmType != null ? GetManagerInstance(jrmType) : null;
                Assembly commons = GetTMPECommonsAssembly();
                Type tbType = commons != null ? commons.GetType("CSUtil.Commons.TernaryBool") : null;

                MethodInfo mSetUturn = (jrmType != null && tbType != null) ? jrmType.GetMethod("SetUturnAllowed", new Type[] { typeof(ushort), typeof(bool), tbType }) : null;
                MethodInfo mSetBlocked = (jrmType != null && tbType != null) ? jrmType.GetMethod("SetEnteringBlockedJunctionAllowed", new Type[] { typeof(ushort), typeof(bool), tbType }) : null;
                MethodInfo mSetPed = (jrmType != null && tbType != null) ? jrmType.GetMethod("SetPedestrianCrossingAllowed", new Type[] { typeof(ushort), typeof(bool), tbType }) : null;
                MethodInfo mSetStraight = (jrmType != null && tbType != null) ? jrmType.GetMethod("SetLaneChangingAllowedWhenGoingStraight", new Type[] { typeof(ushort), typeof(bool), tbType }) : null;
                MethodInfo mSetNear = (jrmType != null && tbType != null) ? jrmType.GetMethod("SetNearTurnOnRedAllowed", new Type[] { typeof(ushort), typeof(bool), tbType }) : null;
                MethodInfo mSetFar = (jrmType != null && tbType != null) ? jrmType.GetMethod("SetFarTurnOnRedAllowed", new Type[] { typeof(ushort), typeof(bool), tbType }) : null;

                object[] setArgs = new object[3];
                for (int i = 0; i < jrCount; i++)
                {
                    ushort segId = br.ReadUInt16();
                    bool startNode = br.ReadBoolean();
                    byte uturn = br.ReadByte();
                    byte blocked = br.ReadByte();
                    byte ped = br.ReadByte();
                    byte straight = br.ReadByte();
                    byte near = br.ReadByte();
                    byte far = br.ReadByte();

                    if (jrm != null && tbType != null)
                    {
                        setArgs[0] = segId;
                        setArgs[1] = startNode;

                        if (uturn != 0 && mSetUturn != null) { setArgs[2] = Enum.ToObject(tbType, uturn); mSetUturn.Invoke(jrm, setArgs); }
                        if (blocked != 0 && mSetBlocked != null) { setArgs[2] = Enum.ToObject(tbType, blocked); mSetBlocked.Invoke(jrm, setArgs); }
                        if (ped != 0 && mSetPed != null) { setArgs[2] = Enum.ToObject(tbType, ped); mSetPed.Invoke(jrm, setArgs); }
                        if (straight != 0 && mSetStraight != null) { setArgs[2] = Enum.ToObject(tbType, straight); mSetStraight.Invoke(jrm, setArgs); }
                        if (near != 0 && mSetNear != null) { setArgs[2] = Enum.ToObject(tbType, near); mSetNear.Invoke(jrm, setArgs); }
                        if (far != 0 && mSetFar != null) { setArgs[2] = Enum.ToObject(tbType, far); mSetFar.Invoke(jrm, setArgs); }
                    }
                }

                // 2. Restore Priority Signs
                int psCount = br.ReadInt32();
                Type tpmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficPriorityManager");
                object tpm = tpmType != null ? GetManagerInstance(tpmType) : null;
                Assembly apiAsm = GetTMPEAPIAssembly();
                Type ptType = apiAsm != null ? apiAsm.GetType("TrafficManager.API.Traffic.Enums.PriorityType") : null;

                MethodInfo mSetSign = (tpmType != null && ptType != null) ? tpmType.GetMethod("SetPrioritySign", new Type[] { typeof(ushort), typeof(bool), ptType }) : null;
                object[] psArgs = new object[3];
                for (int i = 0; i < psCount; i++)
                {
                    ushort segId = br.ReadUInt16();
                    bool startNode = br.ReadBoolean();
                    byte sign = br.ReadByte();

                    if (tpm != null && ptType != null && mSetSign != null)
                    {
                        psArgs[0] = segId;
                        psArgs[1] = startNode;
                        psArgs[2] = Enum.ToObject(ptType, sign);
                        try { mSetSign.Invoke(tpm, psArgs); } catch { }
                    }
                }

                // 3. Restore Timed Traffic Lights
                int ttlLen = br.ReadInt32();
                int ttlCount = 0;
                if (ttlLen > 0)
                {
                    byte[] ttlBytes = br.ReadBytes(ttlLen);
                    Type tlsmType = tmpe.GetType("TrafficManager.Manager.Impl.TrafficLightSimulationManager");
                    object tlsm = tlsmType != null ? GetManagerInstance(tlsmType) : null;

                    if (tlsm != null)
                    {
                        Type ttlCfgType = tmpe.GetType("TrafficManager.Configuration+TimedTrafficLights");
                        Type listType = typeof(List<>).MakeGenericType(ttlCfgType);
                        MethodInfo mLoad = tlsmType.GetMethod("LoadData", new Type[] { listType });

                        if (mLoad != null)
                        {
                            using (MemoryStream ttlStream = new MemoryStream(ttlBytes))
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                IList ttlList = (IList)bf.Deserialize(ttlStream);
                                if (ttlList != null)
                                {
                                    ttlCount = ttlList.Count;
                                    mLoad.Invoke(tlsm, new object[] { ttlList });
                                }
                            }
                        }
                    }
                }

                Debug.Log(string.Format("[MoreNetworkCapacityRenew] Successfully restored High-ID TM:PE data: {0} Junction Restrictions, {1} Priority Signs, {2} Timed Traffic Lights.",
                    jrCount, psCount, ttlCount));
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("[MoreNetworkCapacityRenew] RestorePendingHighIdTMPEData error: " + ex);
        }
        finally
        {
            s_pendingHighIdTMPEData = null;
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("TestSerializer compiled cleanly!");
    }
}
