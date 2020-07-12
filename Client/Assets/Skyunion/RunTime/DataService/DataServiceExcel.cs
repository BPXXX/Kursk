using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ExcelDataReader;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Data;

namespace Skyunion
{
    // SQLite的到时候自己写个 DataService_SQLite的实现类
    class DataServiceExcel : DataService
    {
        public class CompareFieldInfoByLenght : IComparer<FieldInfo>
        {
            public int Compare(FieldInfo left, FieldInfo right)
            {
                if (left.alias.Length < right.alias.Length)
                {
                    return 1;
                }
                else if (left.alias.Length == right.alias.Length)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
        }

        protected override ITable<T> CreateTable<T>(Type type)
        {
            return new TableExcelBinary<T>(type);
        }
        public override DataMode GetDataMode()
        {
            return DataMode.Excel;
        }

        private float mStartTime;
        public override void Init()
        {
            mStartTime = Time.realtimeSinceStartup;
            m_ExcelDir = Path.Combine(Application.streamingAssetsPath, "Config", "Excel");
#if UNITY_EDITOR
            var excelPath = UnityEditor.EditorPrefs.GetString("ExcelPath", "");
            if(!excelPath.Equals(string.Empty))
            {
                m_ExcelDir = excelPath;
            }
#endif

            CoreUtils.assetService.WaitInitAsync(() =>
            {
                LoadCache();
                LoadFile();
                CollectInfo();
                ReplaceAlias();
                BuildCSVCache();
                BuildBinaryCache();
                ReplaceTextColor();
                SaveCache();
                //CoreUtils.logService.Info($"LoadExcel:{Time.realtimeSinceStartup - mStartTime}s", Color.green);
                OnInitialized();
            });
        }

        public class FieldInfo
        {
            public string name;
            public string type;
            public string value;
            public string alias;
            public string initValue;
            public string comment;
            public string meta;
        };
        class TableFieldInfo
        {
            public string name;
            public string type;
            public string basetype;
            public string split;
            public string comment;
            public bool list = false;
            public ObjectType ot;
        };

        class ObjectType
        {
            public string name;
            public bool IsEnum;
            public List<FieldInfo> field = new List<FieldInfo>();
            public List<FieldInfo> fieldSort = new List<FieldInfo>();
        };

        class TableInfo
        {
            public string fileName;
            public string name;
            public string package;
            public bool isVertical;
            public long modifyTime;
            public long fileSize;
            public string md5;
            public Dictionary<string, ObjectType> objectType = new Dictionary<string, ObjectType>();
            public List<TableFieldInfo> field = new List<TableFieldInfo>();
            public List<List<string>> records = new List<List<string>>();
            public bool isHasChange;
        };

        Dictionary<string, TableInfo> m_mapTables = new Dictionary<string, TableInfo>();
        Dictionary<string, ObjectType> m_mapObjectType = new Dictionary<string, ObjectType>();
        int mExcelChangeCount = 0;

        public static string m_ExcelDir;

        bool LoadFile()
        {
            CoreUtils.logService.Info($"Start Analysis Excel", Color.green);
            var cacheDir = m_ExcelDir;
            List<string> withoutExtensions = new List<string>() { ".xlsm", ".xlsx" };
            var files = (Directory.GetFiles(cacheDir, "*.*", SearchOption.TopDirectoryOnly)
                .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray());
            
            mExcelChangeCount = 0;
            foreach (var path in files)
            {
                FileInfo fi = new FileInfo(path);
                var fileName = fi.Name;
                if (fileName[0] == '~')
                    continue;

                // 临时屏蔽
                if (fileName == "s_MonsterPoint.xlsm" || fileName == "s_ResourceGatherPoint.xlsm")
                    continue;

                var modifyTime = fi.LastWriteTime.ToFileTime();
                var fileSize = fi.Length;
                TableInfo table;
                if (m_mapTables.TryGetValue(fileName, out table))
                {
                    if (modifyTime == table.modifyTime && fileSize == table.fileSize)
                        continue;
                }
                byte[] content = null;
                Debug.Log(path);
                try
                {
                    content = File.ReadAllBytes(path);
                }
                catch(Exception e)
                {
                    var tmp = path + ".tmp";
                    File.Copy(path, path + ".tmp", true);
                    content = File.ReadAllBytes(tmp);
                    File.Delete(tmp);
                }
                MD5 md5Hash = MD5.Create();
                byte[] data = md5Hash.ComputeHash(content);
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                var md5 = sBuilder.ToString();
                if (table != null)
                {
                    if (md5 == table.md5)
                    {
                        table.modifyTime = modifyTime;
                        continue;
                    }
                }

                mExcelChangeCount++;
                table = new TableInfo();
                table.isHasChange = true;
                table.fileName = fileName;
                table.fileSize = fileSize;
                table.modifyTime = modifyTime;
                table.md5 = md5;

                m_mapTables[fileName] = table;
                ParseXlsx(content, table);
            }
            CoreUtils.logService.Info($"End Analysis Excel:{mExcelChangeCount} Excel Changed!", Color.green);
            return true;
        }

        bool CollectInfo()
        {
            foreach(var itr in m_mapTables)
            {
                var table = itr.Value;

                foreach (var typeItr in table.objectType)
                {
                    ObjectType type;
                    if(m_mapObjectType.TryGetValue(typeItr.Key, out type))
                    {
                        Debug.LogError($"find same object type: {typeItr.Key}");
                        return false;
                    }
                    m_mapObjectType.Add(typeItr.Key, typeItr.Value);
                }
            }
            foreach(var ot in m_mapObjectType)
            {
                ot.Value.fieldSort = new List<FieldInfo>(ot.Value.field.ToArray());
                ot.Value.fieldSort.Sort(new CompareFieldInfoByLenght());
            }
            foreach (var itr in m_mapTables)
            {
                var table = itr.Value;
                foreach (var field in table.field)
                {
                    field.list = false;
                    if (field.type[0] == 'L')
                    {
                        int nIdx = field.type.IndexOf("List<");
                        if (nIdx != -1)
                        {
                            int nIdx2 = field.type.LastIndexOf(">");
                            field.basetype = field.type.Substring(nIdx + 5, nIdx2 - 5 - nIdx);
                            field.list = true;
                            m_mapObjectType.TryGetValue(field.basetype, out field.ot);
                        }
                    }
                    if (!field.list)
                    {
                        m_mapObjectType.TryGetValue(field.type, out field.ot);
                    }
                }
            }

            return true;
        }

        bool ReplaceTextColor()
        {
            var colorFile = Path.Combine(m_ExcelDir, "TextColors.xml");
            if (!File.Exists(colorFile))
            {
                CoreUtils.logService.Info($"TextColors.xml Not Found!", Color.green);
                return false;
            }

            XmlDocument doc = new XmlDocument(); ;
            doc.Load(colorFile);
            var rootNode = doc.DocumentElement;
            Dictionary<string, string> mapColors = new Dictionary<string, string>();
            foreach (var excelNode in rootNode.ChildNodes)
            {
                var excelElement = excelNode as XmlElement;
                var name = excelElement.GetAttribute("Name");
                var code = excelElement.GetAttribute("Code");
                mapColors.Add(name, code);
            }
            TableInfo table;
            if(!m_mapTables.TryGetValue("s_Language.xlsm", out table))
            {
                return false;
            }
            if (!table.isHasChange)
                return true;

            var binDir = Path.Combine(m_ExcelDir, "Cache", "Bin");

            var path = Path.Combine(binDir, $"{table.name}.bin");
            var mem = new MemoryStream();
            var writer = new BinaryWriter(mem);

            List<string> listText = new List<string>();
            Dictionary<string, int> mapTextIdx = new Dictionary<string, int>();
            List<int> listIdx = new List<int>();
            for (int row = 0; row < table.records.Count; row++)
            {
                for (int col = 0; col < table.field.Count; col++)
                {
                    var value = table.records[row][col];
                    int idx;
                    if (!mapTextIdx.TryGetValue(value, out idx))
                    {
                        idx = listText.Count;
                        mapTextIdx.Add(value, listText.Count);

                        listText.Add(value);
                    }
                    listIdx.Add(idx);
                }
            }

            foreach(var colorPair in mapColors)
            {
                string oldColor = $"<color={colorPair.Key}>";
                string newColor = $"<color={colorPair.Value}>";
                for (int i = 0; i < listText.Count; i++)
                {
                    listText[i] = listText[i].Replace(oldColor, newColor);
                }
            }

            writer.Write(listText.Count);
            for (int i = 0; i < listText.Count; i++)
            {
                var bytes = Encoding.UTF8.GetBytes(listText[i]);
                writer.Write((UInt16)bytes.Length);
                writer.Write(bytes, 0, bytes.Length);
            }

            writer.Write(table.records.Count);
            writer.Write((byte)table.field.Count);
            int nCount = table.records.Count * table.field.Count;
            for (int i = 0; i < nCount; i++)
            {
                writer.Write(listIdx[i]);
            }

            var allBytes = mem.ToArray();
            File.WriteAllBytes(path, allBytes);
            var binPath = Path.Combine(Application.streamingAssetsPath, "Config/Bin/", $"{table.name}.bin");
            File.WriteAllBytes(binPath, allBytes);


            return true;
        }
        bool LoadCache()
        {
            var cacheDir = Path.Combine(m_ExcelDir, "Cache");
            var cacheFile = Path.Combine(cacheDir, "ExcelCache.xml");
            if (!File.Exists(cacheFile))
            {
                CoreUtils.logService.Info($"Excel CacheFile Not Found!", Color.green);
                return false;
            }

            CoreUtils.logService.Info($"LoadExcelCache !", Color.green);

            XmlDocument doc = new XmlDocument(); ;
            doc.Load(cacheFile);
            var rootNode = doc.DocumentElement;
            foreach(var excelNode in rootNode.ChildNodes)
            {
                var excelElement = excelNode as XmlElement;
                TableInfo table = new TableInfo();

                table.isHasChange = false;
                table.name = excelElement.GetAttribute("Name");
                table.package = excelElement.GetAttribute("Package");
                table.isVertical = excelElement.GetAttribute("Vertical") == "true";
                table.fileName = excelElement.GetAttribute("FileName");
                table.fileSize = Convert.ToInt64(excelElement.GetAttribute("FileSize"));
                table.modifyTime = Convert.ToInt64(excelElement.GetAttribute("ModifyTime"));
                table.md5 = excelElement.GetAttribute("Md5");

                foreach(var childNode in excelElement.ChildNodes)
                {
                    var childElement = childNode as XmlElement;
                    if (childElement.Name == "Field")
                    {
                        var fieldElement = childNode as XmlElement;

                        var field = new TableFieldInfo();
                        field.name = fieldElement.GetAttribute("Name");
                        field.type = fieldElement.GetAttribute("Type");
                        field.split = fieldElement.GetAttribute("Split");
                        field.comment = fieldElement.GetAttribute("Comment");
                        table.field.Add(field);
                    }
                    else if (childElement.Name == "ObjectType")
                    {
                        var objectTypeNode = childNode as XmlElement;
                        ObjectType objectType = new ObjectType();
                        objectType.name = objectTypeNode.GetAttribute("Name");
                        objectType.IsEnum = false;

                        foreach (var fieldNode in objectTypeNode.ChildNodes)
                        {
                            var fieldElement = fieldNode as XmlElement;

                            FieldInfo field = new FieldInfo();
                            field.name = fieldElement.GetAttribute("Name");
                            field.type = fieldElement.GetAttribute("Type");
                            field.value = fieldElement.GetAttribute("Value");

                            if (field.value.Length > 0)
                                objectType.IsEnum = true;

                            field.alias = fieldElement.GetAttribute("Alias");
                            field.initValue = fieldElement.GetAttribute("InitValue");
                            field.comment = fieldElement.GetAttribute("Comment");
                            field.meta = fieldElement.GetAttribute("Meta");

                            objectType.field.Add(field);
                        };

                        table.objectType.Add(objectType.name, objectType);
                    }
                }

                m_mapTables.Add(table.fileName,table);
            }
            return true;
        }
        bool SaveCache()
        {
            if (mExcelChangeCount == 0)
            {
                CoreUtils.logService.Info($"Excel Not Change !");
                return true;
            }
            CoreUtils.logService.Info($"Excel Change Count {mExcelChangeCount}", Color.green);
            CoreUtils.logService.Info("SaveCache", Color.green);

            var cacheDir = Path.Combine(m_ExcelDir, "Cache");
            var cacheFile = Path.Combine(cacheDir, "ExcelCache.xml");
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }

            XmlDocument doc = new XmlDocument();
            var root = doc.CreateElement("ExcelCache");
            doc.AppendChild(root);

            foreach(var itrTable in m_mapTables)
            {
                var excelNode = doc.CreateElement("Excel");
                var table = itrTable.Value;

                excelNode.SetAttribute("Name", table.name);
                excelNode.SetAttribute("Package", table.package);
                excelNode.SetAttribute("Vertical", table.isVertical ? "true" : "false");
                excelNode.SetAttribute("FileName", table.fileName);
                excelNode.SetAttribute("FileSize", table.fileSize.ToString());
                excelNode.SetAttribute("ModifyTime", table.modifyTime.ToString());
                excelNode.SetAttribute("Md5", table.md5);

                for (int i = 0; i < table.field.Count; i++)
                {
                    var fieldNode = doc.CreateElement("Field");
                    var field = table.field[i];
                    fieldNode.SetAttribute("Name", field.name);
                    fieldNode.SetAttribute("Type", field.type);
                    fieldNode.SetAttribute("Split", field.split);
                    fieldNode.SetAttribute("Comment", field.comment);

                    excelNode.AppendChild(fieldNode);
                }

                foreach(var itrType in table.objectType)
                {
                    var typeNode = doc.CreateElement("ObjectType");
                    var type = itrType.Value;
                    typeNode.SetAttribute("Name", type.name);

                    foreach(var itrField in type.field)
                    {
                        var fieldNode = doc.CreateElement("Field");
                        var field = itrField;

                        fieldNode.SetAttribute("Name", field.name);
                        fieldNode.SetAttribute("Type", field.type);
                        fieldNode.SetAttribute("Value", field.value);
                        fieldNode.SetAttribute("Alias", field.alias);
                        fieldNode.SetAttribute("InitValue", field.initValue);
                        fieldNode.SetAttribute("Meta", field.meta);
                        fieldNode.SetAttribute("Comment", field.comment);

                        typeNode.AppendChild(fieldNode);
                    }

                    excelNode.AppendChild(typeNode);
                }

                root.AppendChild(excelNode);
            }

            doc.Save(cacheFile);
            return true;
        }
        bool BuildCSVCache()
        {
            var binDir = Path.Combine(m_ExcelDir, "Cache", "CSV");
            if (!Directory.Exists(binDir))
            {
                Directory.CreateDirectory(binDir);
            }

            foreach (var pair in m_mapTables)
            {
                var table = pair.Value;
                if (!table.isHasChange)
                    continue;

                var path = Path.Combine(binDir, $"{table.name}.csv");
                StringBuilder sb = new StringBuilder();


                for (int i = 0; i < table.field.Count; i++)
                {
                    sb.Append('"');
                    sb.Append(table.field[i].name);
                    sb.Append('"');
                    if (i + 1 != table.field.Count)
                    {
                        sb.Append(',');
                    }
                }
                sb.Append("\r\n");

                for (int i = 0; i < table.field.Count; i++)
                {
                    sb.Append('"');
                    sb.Append(table.field[i].type);
                    sb.Append('"');
                    if (i + 1 != table.field.Count)
                    {
                        sb.Append(',');
                    }
                }
                sb.Append("\r\n");


                for (int row = 0; row < table.records.Count; row++)
                {
                    for (int col = 0; col < table.field.Count; col++)
                    {
                        sb.Append('"');
                        sb.Append(table.records[row][col]);
                        sb.Append('"');
                        if (col + 1 != table.field.Count)
                        {
                            sb.Append(',');
                        }
                    }
                    if (row + 1 != table.records.Count)
                    {
                        sb.Append("\r\n");

                    }
                }
                File.WriteAllText(path, sb.ToString());
            }
            return true;
        }
        bool BuildBinaryCache()
        {
            var binDir = Path.Combine(m_ExcelDir, "Cache", "Bin");
            if (!Directory.Exists(binDir))
            {
                Directory.CreateDirectory(binDir);
            }
            foreach (var pair in m_mapTables)
            {
                var table = pair.Value;
                if (!table.isHasChange)
                    continue;

                var path = Path.Combine(binDir, $"{table.name}.bin");
                var mem = new MemoryStream();
                var writer = new BinaryWriter(mem);

                List<string> listText = new List<string>();
                Dictionary<string, int> mapTextIdx = new Dictionary<string, int>();
                List<int> listIdx = new List<int>();
                for (int row = 0; row < table.records.Count; row++)
                {
                    for(int col = 0; col < table.field.Count; col++)
                    {
                        var value = table.records[row][col];
                        int idx;
                        if(!mapTextIdx.TryGetValue(value, out idx))
                        {
                            idx = listText.Count;
                            mapTextIdx.Add(value, listText.Count);
                            listText.Add(value);
                        }
                        listIdx.Add(idx);
                    }
                }
                writer.Write(listText.Count);
                for (int i = 0; i < listText.Count; i++)
                {
                    var bytes = Encoding.UTF8.GetBytes(listText[i]);
                    writer.Write((UInt16)bytes.Length);
                    writer.Write(bytes, 0, bytes.Length);
                }

                writer.Write(table.records.Count);
                writer.Write((byte)table.field.Count);
                int nCount = table.records.Count * table.field.Count;
                for (int i = 0; i < nCount; i++)
                {
                    writer.Write(listIdx[i]);
                }

                var allBytes = mem.ToArray();
                File.WriteAllBytes(path, allBytes);
                var binPath = Path.Combine(Application.streamingAssetsPath, "Config/Bin/", $"{table.name}.bin");
                File.WriteAllBytes(binPath, allBytes);
            }

            return true;
        }
        bool ReplaceAlias()
        {
            foreach(var itr in m_mapTables)
            {
                var table = itr.Value;
                if (!table.isHasChange)
                    continue;

                for (int row = 0; row < table.records.Count; row++)
                {
                    var cols = table.records[row];
                    for (int col = 0; col < cols.Count; col++)
                    {
                        var value = cols[col];
                        if (value.Length == 0)
                        {
                            continue;
                        }
                        var field = table.field[col];
                        if (field.ot == null)
                            continue;

                        var type = field.ot;
                        if (type.IsEnum)
                        {
                            if (field.list)
                            {
                                for (int i = 0; i < type.fieldSort.Count; i++)
                                {
                                    var fieldType = type.fieldSort[i];
                                    if (fieldType.alias.Length > 0)
                                    {
                                        value = value.Replace(fieldType.alias, fieldType.value);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < type.fieldSort.Count; i++)
                                {
                                    var fieldType = type.fieldSort[i];
                                    if (fieldType.alias == value)
                                    {
                                        value = fieldType.value;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < type.fieldSort.Count; i++)
                            {
                                var fieldType = type.fieldSort[i];
                                if (fieldType.alias.Length > 0)
                                {
                                    value = value.Replace(fieldType.alias, fieldType.value);
                                }
                            }
                            if (field.list)
                            {
                                value = value.Replace("|", "}|{\"");
                            }
                            value = value.Replace(" ", ",\"");
                            value = value.Replace(":", "\":");
                            if (value[value.Length - 1] == '"')
                            {
                                value.Remove(value.Length - 1);
                            }
                            value = "{\"" + value + "}";
                        }
                        cols[col] = value;
                    }
                }
            }
            return true;
        }

        Dictionary<string, string> GetProps(string text)
        {
            text = text.Replace("  ", " ");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var texts = text.Split(' ');
            for (int i = 0; i < texts.Length - 1; i += 2)
            {
                if (texts[i + 1][0] == '"')
                {
                    dic.Add(texts[i].Substring(0, texts[i].Length - 1), texts[i+1].Substring(1, texts[i + 1].Length - 2));
                }
                else
                {
                    dic.Add(texts[i].Substring(0, texts[i].Length - 1), texts[i + 1]);
                }
            }
            return dic;
        }

        string GetCSharpType(string type)
        {
            bool bIsArray = false;
            if (type[0] == '[')
            {
                type = type.Substring(2);
                bIsArray = true;
            }
            if (type == "int32")
            {
                type = "int";
            }
            else if (type == "int64")
            {
                type = "long";
            }
            if (bIsArray)
            {
                return "List<" + type + ">";
            }
            return type;
        }

        bool ParseXlsx(byte[] content, TableInfo table)
        {
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(new MemoryStream(content));
            var ds = excelReader.AsDataSet();

            // 解析类型
            var typeSheet = ds.Tables["@Types"];
            var sheetPropsStr = typeSheet.Rows[0][0].ToString();
            var values = GetProps(sheetPropsStr);

            var tableName = values["TableName"];
            table.name = tableName;
            table.package = values["Package"];
            table.isVertical = false;
            if (values.ContainsKey("Vertical"))
            {
                table.isVertical = values["Vertical"] == "true";
            }

            for (int i = 3; i < typeSheet.Rows.Count; i++)
            {
                FieldInfo fieldInfo = new FieldInfo();
                var objectName = typeSheet.Rows[i][0].ToString();
                if (objectName.Length == 0)
                    break;
                ObjectType type;
                if (!table.objectType.TryGetValue(objectName, out type))
                {
                    type = table.objectType[objectName] = new ObjectType();
                    type.name = objectName;
                    type.IsEnum = false;
                }

                fieldInfo.name = typeSheet.Rows[i][1].ToString();
                fieldInfo.type = GetCSharpType(typeSheet.Rows[i][2].ToString());
                fieldInfo.value = typeSheet.Rows[i][3].ToString();
                fieldInfo.alias = typeSheet.Rows[i][4].ToString();
                fieldInfo.initValue = typeSheet.Rows[i][5].ToString();
                fieldInfo.meta = typeSheet.Rows[i][6].ToString();
                fieldInfo.comment = typeSheet.Rows[i][7].ToString();
                fieldInfo.comment = fieldInfo.comment.Replace("\n", "");
                if (fieldInfo.value.Length > 0)
                {
                    type.IsEnum = true;
                }
                type.field.Add(fieldInfo);
            }

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                var sheet = ds.Tables[i];
                var name = sheet.TableName;
                if (name[0] == '#' || name[0] == '@')
                    continue;

                List<int> vecIdx = new List<int>();
                if (table.isVertical)
                {
                    for (int row = 1; row < sheet.Rows.Count; row++)
                    {
                        var fieldInfo = new TableFieldInfo();
                        fieldInfo.name = sheet.Rows[row][0].ToString();
                        if (fieldInfo.name.Length == 0)
                            break;
                        if (fieldInfo.name[0] == '#' || fieldInfo.name[0] == '*')
                            continue;

                        vecIdx.Add(row);
                        fieldInfo.type = GetCSharpType(sheet.Rows[row][1].ToString());
                        fieldInfo.split = sheet.Rows[row][2].ToString();
                        fieldInfo.comment = sheet.Rows[row][3].ToString() + sheet.Rows[row][5].ToString(); ;
                        fieldInfo.comment = fieldInfo.comment.Replace("\n", "");
                        if (fieldInfo.type.Length > 0 && fieldInfo.type[0] == '[')
                        {
                            var props = GetProps(fieldInfo.split);
                            fieldInfo.split = props["ListSpliter"];
                        }
                        table.field.Add(fieldInfo);
                    }
                    List<string> cols = new List<string>();
                    for (int row = 0; row < vecIdx.Count; row++)
                    {
                        var value = sheet.Rows[vecIdx[row]][4].ToString();
                        cols.Add(value);
                    }

                    if (cols[0].Length > 0)
                        table.records.Add(cols);
                }
                else
                {
                    for (int col = 0; col < sheet.Columns.Count; col++)
                    {
                        TableFieldInfo fieldInfo = new TableFieldInfo();
                        fieldInfo.name = sheet.Rows[0][col].ToString();
                        if (fieldInfo.name.Length == 0)
                            break;
                        if (fieldInfo.name[0] == '#' || fieldInfo.name[0] == '*')
                            continue;
                        vecIdx.Add(col);
                        fieldInfo.type = GetCSharpType(sheet.Rows[1][col].ToString());
                        fieldInfo.split = sheet.Rows[2][col].ToString();
                        fieldInfo.comment = sheet.Rows[3][col].ToString();
                        fieldInfo.comment = fieldInfo.comment.Replace("\n", "");
                        if (fieldInfo.type.Length > 0 && fieldInfo.type[0] == '[')
                        {
                            if (fieldInfo.split.Length == 0)
                            {
                                fieldInfo.split = "|";
                            }
                            else
                            {
                                var props = GetProps(fieldInfo.split);
                                fieldInfo.split = props["ListSpliter"];
                            }
                        }
                        table.field.Add(fieldInfo);
                    }

                    table.records = new List<List<string>>(sheet.Rows.Count - 4);
                    for (int row = 4; row < sheet.Rows.Count; row++)
                    {
                        List<string> cols = new List<string>(vecIdx.Count);
                        for (int col = 0; col < vecIdx.Count; col++)
                        {
                            var value = sheet.Rows[row][vecIdx[col]].ToString();
                            cols.Add(value);
                        }
                        if (cols[0].Length > 0)
                            table.records.Add(cols);
                    }
                }
                break;
            }
            return true;
        }
    }

    public class TableExcelBinary<T> : TableBase<T>
    {
        public TableExcelBinary(Type type) : base(type)
        {
        }
        public TableExcelBinary() : base(typeof(T))
        {
        }
        protected override void LoadTable()
        {
            float startTime = Time.realtimeSinceStartup;
            var binDir = Path.Combine(DataServiceExcel.m_ExcelDir, "Cache", "Bin");
            var path = Path.Combine(binDir, $"{tableName}.bin");
            var content = File.ReadAllBytes(path);
            var reader = new BinaryReader(new MemoryStream(content));

            int textNum = reader.ReadInt32();
            List<string> listText = new List<string>(textNum);

            for (int i = 0; i < textNum; i++)
            {
                int nSize = reader.ReadUInt16();
                var data = reader.ReadBytes(nSize);
                var value = Encoding.UTF8.GetString(data);
                listText.Add(value);
            }

            int nRow = reader.ReadInt32();
            Byte nCol = reader.ReadByte();
            rows = new List<List<string>>(nRow);
            listRecords = new List<T>(rows.Count);
            for (int row = 0; row < nRow; row++)
            {
                var cols = new List<string>(nCol);
                for (int col = 0; col < nCol; col++)
                {
                    int nIdx = reader.ReadInt32();
                    cols.Add(listText[nIdx]);
                }
                try
                {
                    int id = Convert.ToInt32(cols[0]);
                    mapIdx.Add(id, rows.Count);
                }
                catch (Exception e)
                {
                    mapIdx.Add(mapIdx.Count, rows.Count);
                }
                rows.Add(cols);
                listRecords.Add(default);
            }
            Color color = Color.green;
            var message = string.Format("<b><color=#{0:X2}{1:X2}{2:X2}>{3}</color></b>\n", (byte)(color.r * 255), (byte)(color.g * 255), (byte)(color.b * 255), $"LoadTable {tableName}:{Time.realtimeSinceStartup - startTime}s");
            Debug.Log(message);
        }
    }
}
