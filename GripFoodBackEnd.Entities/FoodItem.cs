namespace GripFoodBackEnd.Entities
{
    public class FoodItem
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string RestaurantId { get; set; } = "";
        public Restaurant Restaurant { get; set; } = null!;
        public List<CartDetail> CartsDetails { get; set; } = new List<CartDetail>();
    }
}