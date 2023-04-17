namespace GripFoodBackEnd.Models
{
    public class FoodItemCreateModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string RestaurantId { get; set; } = "";
    }
}
