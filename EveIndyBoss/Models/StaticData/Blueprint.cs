namespace EveIndyBoss.Models.StaticData
{
    public class Blueprint
    {
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public int MaxProductionLimit { get; set; }
        public int OutputTypeId { get; set; }
    }
}