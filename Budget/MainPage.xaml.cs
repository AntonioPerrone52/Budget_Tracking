using Budget.Data;
using Budget.Models;
using System.Data;

namespace Budget
{
    public partial class MainPage : ContentPage
    {
         private Database database;

        public MainPage()
        {
            InitializeComponent();
            database = new Database();
            output();
        }


  

        private async void output()
        {
            string elenco = "";

            //foreach (var item in await database.LeggiBugetItem_query("Yummi4"))
            foreach (var item in await database.LeggiBugetItem())
                elenco += $" ID: {item.id} - NAME: {item.name} - TYPE: {item.type}";



           /* foreach (var item in await database.Leggi_by_id(Convert.ToInt32("2")))
                elenco += $" ID: {item.id} - NAME: {item.name} - TYPE: {item.type} \n";*/

            await App.Current.MainPage.DisplayAlert("Ocane Possiede un cane",elenco, "OK");

        }
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            //Verifica se tutti i campi sono stati compilati
            bool isFormValid = !string.IsNullOrWhiteSpace(nome.Text) &&
                               !string.IsNullOrWhiteSpace(descrizione.Text) &&
                               !string.IsNullOrWhiteSpace(data.Text) &&
                               !string.IsNullOrWhiteSpace(valore.Text) &&
                               !string.IsNullOrWhiteSpace(tipo.Text);

            // Abilita o disabilita il bottone in base alla validità del form
            //SubmitButton.IsEnabled = isFormValid;

            if (isFormValid)
            {
                // Esegui l'azione desiderata quando il form è valido
                //await App.Current.MainPage.DisplayAlert("Ocane Possiede un cane", "Cioao sono mario", "OK");
                budget_fields budget_fields = new budget_fields
                {
                    data = data.Text,
                    name = nome.Text,
                    description = descrizione.Text,
                    type = tipo.Text,
                    value = valore.Text
                };
                int flag = await database.AggiungiBugetItem(budget_fields);

                if (flag != 0)
                    await App.Current.MainPage.DisplayAlert("Inserimento Eseguito",
                        $"Id Generato da SQLite: {budget_fields.id}", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("Ocane Possiede un cane", "Di chi è il cane?", "OK");
                
            }
        }











    }
}