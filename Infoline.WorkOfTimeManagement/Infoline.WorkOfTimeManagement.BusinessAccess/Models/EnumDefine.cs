using Infoline.Framework.Database;
using Infoline.WorkOfTimeManagement.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Infoline.WorkOfTimeManagement.BusinessAccess
{

    public class EnumDefine : Attribute
    {
        public Type TableObject { get; set; }
        public string TableField { get; set; }

        public EnumDefine(Type tableObject, string tableField = "TEST")
        {
            this.TableObject = tableObject;
            this.TableField = tableObject.GetProperty(tableField) != null
                ? tableObject.GetProperty(tableField).Name
                : null;
        }

    }

    public class SearchEnums
    {

        public ResultStatus Run()
        {

            var insertList = new List<SYS_Enums>();
            var updateList = new List<SYS_Enums>();

            var db = new WorkOfTimeManagementDatabase();
            var enums = db.GetSYS_Enums().ToList();
            //  var projectEnums = typeof(WorkOfTimeManagementDatabase).GetNestedTypes().Where(a => a.IsEnum == true).ToList();
            var projectEnums = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsEnum == true).ToList();

            foreach (var item in projectEnums)
            {

                var _tableName = String.Empty;
                var _fieldName = String.Empty;

                if (item.GetCustomAttribute<EnumDefine>() != null)
                {
                    var Attribute = item.GetCustomAttribute<EnumDefine>();
                    _tableName = Attribute.TableObject.Name;
                    _fieldName = Attribute.TableField;
                }

                foreach (var prop in Enum.GetValues(item))
                {

                    var key = ((Int32)prop).ToString();
                    var value = prop.ToString();
                    var description = item.GetField(value).GetCustomAttribute<DescriptionAttribute>() != null
                        ? item.GetField(value).GetCustomAttribute<DescriptionAttribute>().Description
                        : null;

                    var thisEnum = enums.Where(a => a.tableName == _tableName && a.fieldName == _fieldName && a.enumKey == key).FirstOrDefault();
                    if (thisEnum != null)
                    {

                        if (thisEnum.enumKey != key ||
                            thisEnum.Name != item.Name ||
                            thisEnum.enumValue != value ||
                            thisEnum.tableName != _tableName ||
                            thisEnum.fieldName != _fieldName ||
                            thisEnum.enumDescription != description
                            )
                        {

                            updateList.Add(new SYS_Enums
                            {
                                id = thisEnum.id,
                                changedby = Guid.Empty,
                                changed = DateTime.Now,

                                tableName = _tableName,
                                fieldName = _fieldName,
                                enumKey = key,
                                Name = item.Name,
                                enumValue = value,
                                enumDescription = description
                            });

                        }

                        enums.Remove(enums.Where(a => a.tableName == _tableName && a.fieldName == _fieldName && a.enumKey == key).FirstOrDefault());

                    }
                    else
                    {
                        insertList.Add(new SYS_Enums
                        {
                            createdby = Guid.Empty,
                            created = DateTime.Now,

                            tableName = _tableName,
                            fieldName = _fieldName,
                            Name = item.Name,
                            enumKey = key,
                            enumValue = value,
                            enumDescription = description
                        });
                    }

                }

            }

            var dbresDelete = enums.Count() > 0 ? db.BulkDeleteSYS_Enums(enums) : new ResultStatus { result = true, message = "0" };
            var dbresUpdate = updateList.Count() > 0 ? db.BulkUpdateSYS_Enums(updateList) : new ResultStatus { result = true, message = "0" };
            var dbresInsert = insertList.Count() > 0 ? db.BulkInsertSYS_Enums(insertList) : new ResultStatus { result = true, message = "0" };

            return new ResultStatus
            {
                result = (dbresInsert.result || dbresUpdate.result || dbresDelete.result),
                message = (dbresInsert.message + " " + dbresUpdate.message + " " + dbresDelete.message)
            };

        }

    }

}
