namespace AutoRepairShop
{
    public partial class TypeOfRepair
    {
        public override string ToString()
        {
            return vid_TypeOfRepair;
        }
    }
    public partial class Cars
    {
        public override string ToString()
        {
            return "Модель: " + model_car + "\nМарка: " + mark_car;
        }
    }
    public partial class Repair
    {
        public override string ToString()
        {
            return "Модель: " + Cars.model_car + "\nВид ремонта: " + TypeOfRepair.vid_TypeOfRepair + "\nСтоимость услуги: " + cost_repair;
        }
    }
}
