using Budget.Data;
using Budget.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific;
using Grid = Microsoft.Maui.Controls.Grid;
using Label = Microsoft.Maui.Controls.Label;

namespace Budget
{
    public partial class MainPage : ContentPage
    {
         private Database database;

        public MainPage()
        {
            InitializeComponent();
            database = new Database();
            
            elenco_spese_nella_griglia(DateTime.Now.Month);
        }



        private async void elenco_spese_nella_griglia(int mese)
        {
            string elenco = "";
            var i = 0;
            var j = 0;
            //foreach (var item in await database.LeggiBugetItem_query("Yummi4"))
            foreach (var item in await database.LeggiBugetItem())
            {
               // elenco += $" ID: {item.id} - NAME: {item.name} - TYPE: {item.type} - DATA: {item.data}";
                var label = new Label { 
                    Text = item.name, 
                    FontSize = 18,
                    ClassId =  item.id.ToString()
                };
                // METTERE EVENTO CON CLICK COSI' DA POTER CANCELLARE CHI NON SI VUOLE label.On CLICK???
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) => {
                    // Gestisci l'evento del click qui
                    Label tappedLabel = (Label)s;
                    string labelText = tappedLabel.Text;
                    int row = Grid.GetRow(tappedLabel);
                    int column = Grid.GetColumn(tappedLabel);
                    bool answer = await DisplayAlert($"Cancellare {labelText}?", $"{row} - {column}", "Yes", "No");
                    if (answer)
                    {
                        await database.rimuovi_tupla(Convert.ToInt32(tappedLabel.ClassId));
                        grid_.Children.Remove(tappedLabel);


                    }

                    //METTERE CHE CANCELLLA IN BASE ALL'ID
                };
                label.GestureRecognizers.Add(tapGestureRecognizer);


                if (i == 5)
                {
                    j++;
                    i = 0;
                }
                grid_.Add(label, i++, j);
            }

            /*
             
            <Label Text="00/23: 00,00€" Grid.Row="0" Grid.Column="0" />
            <Label Text="00/23: 00,00€" Grid.Row="0" Grid.Column="1" />
            <Label Text="00/23: 00,00€" Grid.Row="0" Grid.Column="2" />
            <Label Text="00/23: 00,00€" Grid.Row="0" Grid.Column="3" />
            <Label Text="00/23: 00,00€" Grid.Row="0" Grid.Column="4" />
             */
            

        }
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            //Verifica se tutti i campi sono stati compilati

            bool isFormValid = !string.IsNullOrWhiteSpace(nome.Text) &&
                               !string.IsNullOrWhiteSpace(descrizione.Text) &&
                               !string.IsNullOrWhiteSpace(data.Date.ToString()) &&
                               !string.IsNullOrWhiteSpace(valore.Text) &&
                               !string.IsNullOrWhiteSpace(tipo.Text);

            // Abilita o disabilita il bottone in base alla validità del form
            //SubmitButton.IsEnabled = isFormValid;

            if (isFormValid)
            {
                // Esegui l'azione desiderata quando il form è valido
                //await App.Current.MainPage.DisplayAlert("Ocane Possiede un cane", "Cioao", "OK");
                budget_fields budget_fields = new budget_fields
                {
                    data = data.Date.ToString(),
                    name = nome.Text,
                    description = descrizione.Text,
                    type = tipo.Text,
                    value = valore.Text
                };

                int flag = await database.AggiungiBugetItem(budget_fields);

                if (flag != 0)

                    await App.Current.MainPage.DisplayAlert("Inserimento Eseguito",
                        $"Id: {budget_fields.id} \n Nome: {budget_fields.name} \n Tipo {budget_fields.type} \n Data: {budget_fields.data} \n Valore: {budget_fields.value}", "OK");
                else
                    await App.Current.MainPage.DisplayAlert("Orrorw", "Boh?", "OK");
                
            }
        }

    }
}