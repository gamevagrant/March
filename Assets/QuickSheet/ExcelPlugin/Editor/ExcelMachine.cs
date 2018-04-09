///////////////////////////////////////////////////////////////////////////////
///
/// ExcelMachine.cs
///
/// (c)2014 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace UnityQuickSheet
{
    /// <summary>
    /// A class for various setting to read excel file and generated related script files.
    /// </summary>
    internal class ExcelMachine : BaseMachine
    {
        [Serializable]
        protected class ListColumnHeader
        {
            public List<ColumnHeader> Headers;
        }
        /// <summary>
        /// where the .xls or .xlsx file is. The path should start with "Assets/".
        /// </summary>
        public string excelFilePath;

        // both are needed for popup editor control.
        public string[] SheetNames = { "" };
        public int CurrentSheetIndex
        {
            get { return currentSelectedSheet; }
            set { currentSelectedSheet = value;}
        }

        public override List<ColumnHeader> ColumnHeaderList
        {
            get
            {
                CheckValidation();
                return columnHeaderListMap[currentSelectedSheet].Headers;
            }
            set
            {
                columnHeaderListMap[currentSelectedSheet].Headers = value;
                columnHeaderList = value;
            }
        }

        [SerializeField]
        protected int currentSelectedSheet = 0;

        [SerializeField]
        protected List<ListColumnHeader> columnHeaderListMap;

        /// <summary>
        /// Note: Called when the asset file is created.
        /// </summary>

        private void Awake() {
            // excel and google plugin have its own template files,
            // so we need to set the different path when the asset file is created.
            TemplatePath = ExcelSettings.Instance.TemplatePath;
        }
        protected override void OnEnable()
        {
            CheckValidation();
        }

        private void CheckValidation()
        {
            if (columnHeaderListMap == null || columnHeaderListMap.Count == 0)
            {
                columnHeaderListMap = new List<ListColumnHeader>(SupportedColumnCount);
                for (var i = 0; i < SupportedColumnCount; ++i)
                    columnHeaderListMap.Add(new ListColumnHeader
                    {
                        Headers = new List<ColumnHeader>()
                    });
            }

            if (columnHeaderList == null)
                columnHeaderList = columnHeaderListMap[currentSelectedSheet].Headers;
        }

        /// <summary>
        /// A menu item which create a 'ExcelMachine' asset file.
        /// </summary>
        [MenuItem("Assets/Create/QuickSheet/Tools/Excel")]
        public static void CreateScriptMachineAsset()
        {
            ExcelMachine inst = ScriptableObject.CreateInstance<ExcelMachine>();
            string path = CustomAssetUtility.GetUniqueAssetPathNameOrFallback(ImportSettingFilename);
            AssetDatabase.CreateAsset(inst, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = inst;
        }
    }
}