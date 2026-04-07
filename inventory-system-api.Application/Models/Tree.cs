namespace inventory_system_api.Application.Models
{
    public class Tree
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ParentID { get; set; }
        public bool IsProduct { get; set; }
        public List<Tree> Children { get; set; } = [];
    }
}
