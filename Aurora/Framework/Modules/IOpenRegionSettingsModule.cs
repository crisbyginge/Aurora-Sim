/*
 * Copyright (c) Contributors, http://aurora-sim.org/, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the Aurora-Sim Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.Collections.Generic;
using Aurora.Framework;
using OpenMetaverse;
using OpenMetaverse.StructuredData;

namespace Aurora.Framework
{
    public interface IOpenRegionSettingsModule
    {
        float MaxDragDistance { get; set; }
        float DefaultDrawDistance { get; set; }
        float MaximumPrimScale { get; set; }
        float MinimumPrimScale { get; set; }
        float MaximumPhysPrimScale { get; set; }
        float MaximumHollowSize { get; set; }
        float MinimumHoleSize { get; set; }
        int MaximumLinkCount { get; set; }
        int MaximumLinkCountPhys { get; set; }
        OSDArray LSLCommands { get; set; }
        float WhisperDistance { get; set; }
        float SayDistance { get; set; }
        float ShoutDistance { get; set; }
        bool RenderWater { get; set; }
        int MaximumInventoryItemsTransfer { get; set; }
        bool DisplayMinimap { get; set; }
        bool AllowPhysicalPrims { get; set; }
        int OffsetOfUTC { get; set; }
        bool EnableTeenMode { get; set; }
        UUID DefaultUnderpants { get; set; }
        UUID DefaultUndershirt { get; set; }
        bool ClampPrimSizes { get; set; }
        bool ForceDrawDistance { get; set; }
        int ShowTags { get; set; }
        int MaxGroups { get; set; }
        bool AllowParcelWindLight { get; set; }
        void RegisterGenericValue(string key, string value);
    }

    public interface IOpenRegionSettingsConnector : IAuroraDataPlugin
    {
        /// <summary>
        ///   Get the OpenRegionSettings info from the database for the given region
        /// </summary>
        /// <param name = "regionID"></param>
        /// <returns></returns>
        OpenRegionSettings GetSettings(UUID regionID);

        /// <summary>
        ///   Set the OpenRegionSettings info for the given region in the database
        /// </summary>
        /// <param name = "regionID"></param>
        /// <param name = "settings"></param>
        void SetSettings(UUID regionID, OpenRegionSettings settings);

        /// <summary>
        ///   Create a webpage that allows for the editing of the OpenRegionSettings for the given region
        /// </summary>
        /// <param name = "CurrentRegionID"></param>
        /// <returns>The URL to the webpage</returns>
        string AddOpenRegionSettingsHTMLPage(UUID regionID);
    }

    /// <summary>
    ///   This module sends Aurora-specific settings to the viewer to tell it about different settings for the region
    /// </summary>
    public class OpenRegionSettings : IDataTransferable
    {
        #region Declares

        private bool m_AllowParcelWindLight = true;
        private bool m_AllowPhysicalPrims = true;
        private bool m_ClampPrimSizes = true;
        private float m_DefaultDrawDistance = -1;
        public UUID m_DefaultUnderpants = UUID.Zero;
        public UUID m_DefaultUndershirt = UUID.Zero;
        private bool m_DisplayMinimap = true;
        private bool m_ForceDrawDistance = true;
        private OSDArray m_LSLCommands = new OSDArray();
        private float m_MaxDragDistance = -1;
        private int m_MaxGroups = -1;

        private float m_MaximumHollowSize = -1;
        private int m_MaximumInventoryItemsTransfer = -1;

        private int m_MaximumLinkCount = -1;
        private int m_MaximumLinkCountPhys = -1;
        private float m_MaximumPhysPrimScale = -1;
        private float m_MaximumPrimScale = -1;
        private float m_MinimumHoleSize = -1;
        private float m_MinimumPrimScale = -1;
        private int m_OffsetOfUTC = -8;
        private bool m_RenderWater = true;

        private float m_SayDistance = 30;
        private float m_ShoutDistance = 100;

        private int m_ShowTags = 2; //Show always
        private float m_TerrainDetailScale = 16f;
        private float m_WhisperDistance = 10;

        #endregion

        #region Public properties

        public float MaxDragDistance
        {
            get { return m_MaxDragDistance; }
            set { m_MaxDragDistance = value; }
        }

        public float DefaultDrawDistance
        {
            get { return m_DefaultDrawDistance; }
            set { m_DefaultDrawDistance = value; }
        }

        public float MaximumPrimScale
        {
            get
            {
                if (m_ClampPrimSizes)
                    return m_MaximumPrimScale;
                else
                    return float.MaxValue;
            }
            set { m_MaximumPrimScale = value; }
        }

        public float MinimumPrimScale
        {
            get
            {
                if (m_ClampPrimSizes)
                    return m_MinimumPrimScale;
                else
                    return 0;
            }
            set { m_MinimumPrimScale = value; }
        }

        public float MaximumPhysPrimScale
        {
            get
            {
                if (m_ClampPrimSizes)
                    return m_MaximumPhysPrimScale;
                else
                    return float.MaxValue;
            }
            set { m_MaximumPhysPrimScale = value; }
        }

        public float MaximumHollowSize
        {
            get { return m_MaximumHollowSize; }
            set { m_MaximumHollowSize = value; }
        }

        public float MinimumHoleSize
        {
            get { return m_MinimumHoleSize; }
            set { m_MinimumHoleSize = value; }
        }

        public int MaximumLinkCount
        {
            get { return m_MaximumLinkCount; }
            set { m_MaximumLinkCount = value; }
        }

        public int MaximumLinkCountPhys
        {
            get { return m_MaximumLinkCountPhys; }
            set { m_MaximumLinkCountPhys = value; }
        }

        public OSDArray LSLCommands
        {
            get { return m_LSLCommands; }
            set { m_LSLCommands = value; }
        }

        public float WhisperDistance
        {
            get { return m_WhisperDistance; }
            set { m_WhisperDistance = value; }
        }

        public float SayDistance
        {
            get { return m_SayDistance; }
            set { m_SayDistance = value; }
        }

        public float ShoutDistance
        {
            get { return m_ShoutDistance; }
            set { m_ShoutDistance = value; }
        }

        public bool RenderWater
        {
            get { return m_RenderWater; }
            set { m_RenderWater = value; }
        }

        public float TerrainDetailScale
        {
            get { return m_TerrainDetailScale; }
            set
            {
                if (value >= 1f)
                    m_TerrainDetailScale = value;
            }
        }

        public int MaximumInventoryItemsTransfer
        {
            get { return m_MaximumInventoryItemsTransfer; }
            set { m_MaximumInventoryItemsTransfer = value; }
        }

        public bool DisplayMinimap
        {
            get { return m_DisplayMinimap; }
            set { m_DisplayMinimap = value; }
        }

        public bool AllowPhysicalPrims
        {
            get { return m_AllowPhysicalPrims; }
            set { m_AllowPhysicalPrims = value; }
        }

        public int OffsetOfUTC
        {
            get { return m_OffsetOfUTC; }
            set { m_OffsetOfUTC = value; }
        }

        public bool OffsetOfUTCDST { get; set; }

        public bool EnableTeenMode { get; set; }

        public bool SetTeenMode { get; set; }

        public UUID DefaultUnderpants
        {
            get { return m_DefaultUnderpants; }
            set { m_DefaultUnderpants = value; }
        }

        public UUID DefaultUndershirt
        {
            get { return m_DefaultUndershirt; }
            set { m_DefaultUndershirt = value; }
        }

        public bool ClampPrimSizes
        {
            get { return m_ClampPrimSizes; }
            set { m_ClampPrimSizes = value; }
        }

        public bool ForceDrawDistance
        {
            get { return m_ForceDrawDistance; }
            set { m_ForceDrawDistance = value; }
        }

        public int ShowTags
        {
            get { return m_ShowTags; }
            set { m_ShowTags = value; }
        }

        public int MaxGroups
        {
            get { return m_MaxGroups; }
            set { m_MaxGroups = value; }
        }

        public bool AllowParcelWindLight
        {
            get { return m_AllowParcelWindLight; }
            set { m_AllowParcelWindLight = value; }
        }

        #endregion

        #region IDataTransferable

        public override void FromOSD(OSDMap rm)
        {
            if (rm == null)
                return;
            MaxDragDistance = (float)rm["MaxDragDistance"].AsReal();
            DefaultDrawDistance = (float)rm["DrawDistance"].AsReal();
            ForceDrawDistance = rm["ForceDrawDistance"].AsInteger() == 1;
            MaximumPrimScale = (float) rm["MaxPrimScale"].AsReal();
            MinimumPrimScale = (float) rm["MinPrimScale"].AsReal();
            MaximumPhysPrimScale = (float) rm["MaxPhysPrimScale"].AsReal();
            MaximumHollowSize = (float) rm["MaxHollowSize"].AsReal();
            MinimumHoleSize = (float) rm["MinHoleSize"].AsReal();
            ClampPrimSizes = rm["EnforceMaxBuild"].AsInteger() == 1;
            MaximumLinkCount = rm["MaxLinkCount"].AsInteger();
            MaximumLinkCountPhys = rm["MaxLinkCountPhys"].AsInteger();
            MaxDragDistance = (float) rm["MaxDragDistance"].AsReal();
            RenderWater = rm["RenderWater"].AsInteger() == 1;
            TerrainDetailScale = (float) rm["TerrainDetailScale"].AsReal();
            MaximumInventoryItemsTransfer = rm["MaxInventoryItemsTransfer"].AsInteger();
            DisplayMinimap = rm["AllowMinimap"].AsInteger() == 1;
            AllowPhysicalPrims = rm["AllowPhysicalPrims"].AsInteger() == 1;
            OffsetOfUTC = rm["OffsetOfUTC"].AsInteger();
            OffsetOfUTCDST = rm["OffsetOfUTCDST"].AsInteger() == 1;
            EnableTeenMode = rm["ToggleTeenMode"].AsInteger() == 1;
            SetTeenMode = rm["SetTeenMode"].AsInteger() == 1;
            ShowTags = rm["ShowTags"].AsInteger();
            MaxGroups = rm["MaxGroups"].AsInteger();
            AllowParcelWindLight = rm["AllowParcelWindLight"].AsInteger() == 1;
        }

        public override void FromKVP(Dictionary<string, object> KVP)
        {
            FromOSD(Util.DictionaryToOSD(KVP));
        }

        public override Dictionary<string, object> ToKVP()
        {
            return Util.OSDToDictionary(ToOSD());
        }

        public override OSDMap ToOSD()
        {
            OSDMap body = new OSDMap
                              {
                                  {"MaxDragDistance", OSD.FromReal(MaxDragDistance)},
                                  {"DrawDistance", OSD.FromReal(DefaultDrawDistance)},
                                  {"ForceDrawDistance", OSD.FromInteger(ForceDrawDistance ? 1 : 0)},
                                  {"MaxPrimScale", OSD.FromReal(MaximumPrimScale)},
                                  {"MinPrimScale", OSD.FromReal(MinimumPrimScale)},
                                  {"MaxPhysPrimScale", OSD.FromReal(MaximumPhysPrimScale)},
                                  {"MaxHollowSize", OSD.FromReal(MaximumHollowSize)},
                                  {"MinHoleSize", OSD.FromReal(MinimumHoleSize)},
                                  {"EnforceMaxBuild", OSD.FromInteger(ClampPrimSizes ? 1 : 0)},
                                  {"MaxLinkCount", OSD.FromInteger(MaximumLinkCount)},
                                  {"MaxLinkCountPhys", OSD.FromInteger(MaximumLinkCountPhys)},
                                  {"LSLFunctions", LSLCommands},
                                  {"RenderWater", OSD.FromInteger(RenderWater ? 1 : 0)},
                                  {"TerrainDetailScale", OSD.FromReal(TerrainDetailScale)},
                                  {"MaxInventoryItemsTransfer", OSD.FromInteger(MaximumInventoryItemsTransfer)},
                                  {"AllowMinimap", OSD.FromInteger(DisplayMinimap ? 1 : 0)},
                                  {"AllowPhysicalPrims", OSD.FromInteger(AllowPhysicalPrims ? 1 : 0)},
                                  {"OffsetOfUTC", OSD.FromInteger(OffsetOfUTC)},
                                  {"OffsetOfUTCDST", OSD.FromInteger(OffsetOfUTCDST ? 1 : 0)},
                                  {"ToggleTeenMode", OSD.FromInteger(EnableTeenMode ? 1 : 0)},
                                  {"SetTeenMode", OSD.FromInteger(SetTeenMode ? 1 : 0)},
                                  {"ShowTags", OSD.FromInteger(ShowTags)},
                                  {"MaxGroups", OSD.FromInteger(MaxGroups)},
                                  {"AllowParcelWindLight", OSD.FromInteger(AllowParcelWindLight ? 1 : 0)}
                              };

            return body;
        }

        #endregion
    }
}