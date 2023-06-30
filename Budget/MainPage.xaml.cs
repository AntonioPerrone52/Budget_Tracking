using Budget.Data;
using Budget.Models;

namespace Budget
{
    public partial class MainPage : ContentPage
    {
         private Database database;

        public MainPage()
        {
            InitializeComponent();
            database = new Database();
        }


        private async void OnCounterClicked(object sender, EventArgs e)
        {
            budget_fields budget_fields = new budget_fields
            {
                 data = DateTime.Now.ToString(),
                 name = "Test",
                 description = "Desc",
                 type = TipoTXT.Text,
                 value ="euri€"
             };
              int flag = await database.AggiungiBugetItem(budget_fields);

             if (flag != 0)
                 await App.Current.MainPage.DisplayAlert("Inserimento Eseguito",
                     $"Id Generato da SQLite: {budget_fields.id}", "OK");
             else
                 await App.Current.MainPage.DisplayAlert("Ocane Possiede un cane", "Di chi è il cane?", "OK");
            
            

            output();
        }

        private async void output()
        {
            string elenco = "";

            foreach (var item in await database.LeggiBugetItem())
                elenco += $"{item.id} - {item.name} -  {item.type}";

            await App.Current.MainPage.DisplayAlert("Ocane Possiede un cane",elenco, "OK");
        }
    }
}