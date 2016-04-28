namespace EveIndyBoss.Services.StaticData
{
    internal static class StaticDataSql
    {
        internal const string SelectAllBlueprints = @"
SELECT
	T3.typeId AS TypeId,
	T1.categoryId AS CategoryId,
	T2.groupId AS GroupId,
	T2.groupName AS GroupName,
	T3.typeName AS Name,
	T4.maxProductionLimit AS MaxProductionLimit,
	T5.productTypeId AS OutputTypeId
FROM
	invCategories T1,
	invGroups T2,
	invTypes T3,
	industryBlueprints T4,
	industryActivityProducts T5
WHERE T1.categoryId=9
	AND T2.groupId=@groupId
	AND T2.categoryId=T1.categoryId
	AND T3.groupId=T2.groupId
	AND T3.published=1
	AND T4.typeId=T3.typeId
	AND T5.typeId=T3.typeId
    AND T5.ActivityId=1
ORDER BY T3.typeName";

        internal const string SelectAllBlueprintGroups = @"
SELECT
	T1.categoryId AS CategoryId,
	T2.groupId AS GroupId,
	T2.groupName AS GroupName
FROM
	invCategories T1,
	invGroups T2
WHERE T1.categoryId=9
	AND T2.categoryId=T1.categoryId
ORDER BY T2.groupName";

        internal const string SelectMaterialsForType = @"
SELECT
    T1.typeId AS TypeId,
	typeName AS MaterialName,
	materialTypeId AS MaterialTypeId,
	quantity AS Quantity,
	volume AS Volume
FROM
	industryActivityMaterials T1
JOIN invTypes T2 ON T1.materialTypeId=T2.typeId
WHERE T1.typeId=@typeId
	AND activityId=@activityId";

        internal const string SelectItemFromTypes = @"
SELECT
	typeId AS TypeId,
	groupId AS GroupId,
	typeName AS TypeName,
	volume AS Volume
FROM
	invTypes
WHERE typeId=@typeId";
    }
}