using System.Collections.Generic;


namespace ActGame
{
    public class InventoryPage
    {
        public List<InventoryCell> grids;

        // 存储的类型, 存储的容量
        public InventoryPage(InventoryConfig cfg)
        {
            uint capacity = cfg.length * cfg.width;
            for (int i = 0; i < capacity; i++)
            {
                var grid = new InventoryCell(i);
                grids.Add(grid);
            }
        }
        public void Add()
        {

        }
        public void Remove()
        {

        }
    }
}