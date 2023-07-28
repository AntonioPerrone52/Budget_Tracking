using Budget.Data;
using Budget.Models;
using Microsoft.Extensions.Options;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Grid = Microsoft.Maui.Controls.Grid;
using Label = Microsoft.Maui.Controls.Label;

namespace Budget
{
    public partial class MainPage : ContentPage
    {
        private Database database;
        int mese_ = DateTime.Now.Month-1;

        
        public MainPage()
        {
            InitializeComponent();

            database = new Database();
            mese_din.SelectedIndex = mese_;

        }



        int i = 0;
        int j = 0;
        private async void elenco_spese_nella_griglia(int mese)
        {

            //await App.Current.MainPage.DisplayAlert("Inserimento Eseguito",$"Id: {mese}", "OK");
            //foreach (var item in await database.LeggiBugetItem_query("Yummi4"))
            foreach (var item in await database.read_by_month(mese+1))
           // foreach (var item in await database.LeggiBugetItem())
            {
                // elenco += $" ID: {item.id} - NAME: {item.name} - TYPE: {item.type} - DATA: {item.data}";
                var label = new Label
                {
                    Text = item.name,
                    FontSize = 18,
                    ClassId = item.id.ToString()
                };
                // METTERE EVENTO CON CLICK COSI' DA POTER CANCELLARE CHI NON SI VUOLE label.On CLICK???
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) =>
                {
                    // Gestisci l'evento del click qui
                    Label tappedLabel = (Label)s;
                    string labelText = tappedLabel.Text;
                  //  int row = Grid.GetRow(tappedLabel);
                  //  int column = Grid.GetColumn(tappedLabel);
                    var field_temp = await database.Leggi_by_id(Convert.ToInt32(tappedLabel.ClassId));
                    bool answer = await DisplayAlert($"Cancellare {labelText}?", $"Tipo: {field_temp.type}; \nSpesa {field_temp.value}\n Data: {field_temp.data}", "Yes", "No");
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
        }

        //inserisce l'oggetto nella griglia inputata nel form fa le stesse cose come nella funzione elenco_spese_nella_griglia, ma solo per un elemento
        private void inserimento_dinamico(string nome, int id)
            {
            var label = new Label
            {
                Text = nome,
                FontSize = 18,
                ClassId = id.ToString()
            };
            // METTERE EVENTO CON CLICK COSI' DA POTER CANCELLARE CHI NON SI VUOLE label.On CLICK???
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                // Gestisci l'evento del click qui
                Label tappedLabel = (Label)s;
                string labelText = tappedLabel.Text;
                //  int row = Grid.GetRow(tappedLabel);
                //  int column = Grid.GetColumn(tappedLabel);
                var field_temp = await database.Leggi_by_id(Convert.ToInt32(tappedLabel.ClassId));
                bool answer = await DisplayAlert($"Cancellare {labelText}?", $"Tipo: {field_temp.type}; \nSpesa {field_temp.value}\n Data: {field_temp.data}", "Yes", "No");
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


        
        private async void SubmitButton_Clicked(object sender, EventArgs e)
        {
            //Verifica se tutti i campi sono stati compilati

            bool isFormValid = !string.IsNullOrWhiteSpace(nome.Text) &&
                               !string.IsNullOrWhiteSpace(descrizione.Text) &&
                               !string.IsNullOrWhiteSpace(data.Date.ToShortDateString()) &&
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
                    data = data.Date.ToShortDateString(),
                    mese = data.Date.Month,
                    name = nome.Text,
                    description = descrizione.Text,
                    type = tipo.Text,
                    value = valore.Text
                };

                int flag = await database.AggiungiBugetItem(budget_fields);

                if (flag != 0)
                {
                    inserimento_dinamico(budget_fields.name, budget_fields.id);
                    await App.Current.MainPage.DisplayAlert("Inserimento Eseguito",
                        $"Id: {budget_fields.id} \n Nome: {budget_fields.name} \n Tipo {budget_fields.type} \n Data: {budget_fields.data} \n Valore: {budget_fields.value}", "OK");
                }
                else
                    await App.Current.MainPage.DisplayAlert("Orrorw", "Boh?", "OK");
                
            }
        }

        private void mese_din_SelectedIndexChanged(object sender, EventArgs e)
        {

            i = 0;
            j = 0;
            grid_.Children.Clear();
            elenco_spese_nella_griglia(mese_din.SelectedIndex);
        }
    }
}