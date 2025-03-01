﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2025-03-01 22:02:25.426
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameFramework
{
    /// <summary>
    /// 资源配置表。
    /// </summary>
    public class DRAsset : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取资源路径。
        /// </summary>
        public string AssetPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取所在资源组。
        /// </summary>
        public string ResourceGroup
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            AssetPath = columnStrings[index++];
            ResourceGroup = columnStrings[index++];

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    AssetPath = binaryReader.ReadString();
                    ResourceGroup = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
