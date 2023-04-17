namespace GripFoodBackEnd.Models
{
    public class RestaurantDetailModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string FoodId { get; set; } = "";
        public string FoodName { get; set; } = "";
        public decimal FoodPrice { get; set; }
    }
}
