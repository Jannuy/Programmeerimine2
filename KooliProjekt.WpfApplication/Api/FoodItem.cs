namespace KooliProjekt.WpfApplication
{
    public class FoodItem : NotifyPropertyChangedBase
    {
        private int _id;
        private string _name;
        private int _servingSize;
        private int _servingUnit;
        private string _energy;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }

        public int ServingSize
        {
            get { return _servingSize; }
            set
            {
                _servingSize = value;
                NotifyPropertyChanged();
            }
        }

        public int ServingUnit
        {
            get { return _servingUnit; }
            set
            {
                _servingUnit = value;
                NotifyPropertyChanged();
            }
        }

        public string Energy_Kcal_Per_Serving_Sugars_G_Per_Carbs_G
        {
            get { return _energy; }
            set
            {
                _energy = value;
                NotifyPropertyChanged();
            }
        }
    }
}
