namespace ActGame
{
    public class InventoryCell
    {
        private int _index;
        private int _capacity;
        private int _size;
        public InventoryCell(int index, int capacity = 1, int size = 0)
        {
            _index = index;
            _capacity = capacity;
            _size = size;
        }
    }
}