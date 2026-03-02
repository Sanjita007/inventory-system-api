namespace inventory_system_api.Models
{
    public class Tree
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public bool IsProduct { get; set; }
        public List<Tree> Children { get; set; }
    }
}
