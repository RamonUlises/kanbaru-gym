using kanbarugym.Clases;
using kanbarugym.Lib;
using kanbarugym.Views;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace kanbarugym.Pages;

public partial class RegistrarCliente : ContentPage
{
    public RegistrarCliente()
    {
        InitializeComponent();
    }

    public async void OnCreateCliente(object sender, EventArgs e)
    {
        string nombres = txtNombreCliente.Text;
        string fechaNacimiento = txtFechaNacimiento.Text;
        string correoElectronico = txtNECliente.Text;
        string telefono = txtNTClient.Text;
        string? sexo = (cmbSexo.SelectedItem?.ToString() == "Femenino") ? "F" : (cmbSexo.SelectedItem != null ? "M" : null);

        Regex nameRegex = new(@"^(?:[A-Z�����][a-z�����]+(?:\s[A-Z�����][a-z�����]+){0,4})$");
        Regex emailRegex = new(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        Regex dateRegex = new(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}$");
        Regex phoneRegex = new(@"^[0-9]{8}$");

        if (string.IsNullOrWhiteSpace(nombres) ||
            string.IsNullOrWhiteSpace(fechaNacimiento) ||
            string.IsNullOrWhiteSpace(correoElectronico) ||
            string.IsNullOrWhiteSpace(telefono) ||
            string.IsNullOrWhiteSpace(sexo))
        {
            await DisplayAlert("Error", "Por favor, complete todos los campos.", "OK");
            return;
        }

        if (!nameRegex.IsMatch(nombres))
        {
            await DisplayAlert("Error", "Nombre inv�lido.", "OK");
            return;
        }

        if (!emailRegex.IsMatch(correoElectronico))
        {
            await DisplayAlert("Error", "Correo electr�nico inv�lido.", "OK");
            return;
        }

        if (!dateRegex.IsMatch(fechaNacimiento))
        {
            await DisplayAlert("Error", "Fecha de nacimiento inv�lida. Use el formato AAAA-MM-DD.", "OK");
            return;
        }

        if (!phoneRegex.IsMatch(telefono))
        {
            await DisplayAlert("Error", "N�mero de tel�fono inv�lido. Debe contener exactamente 8 d�gitos.", "OK");
            return;
        }

        var cliente = new ClientesLib();

        var nuevoCliente = new
        {
            id = "",
            nombres,
            fechaNacimiento,
            correoElectronico,
            telefono,
            sexo
        };

        ClienteResponse response = await ClientesLib.CrearCliente(nuevoCliente);

        if (response.message == "Cliente creado")
        {
            await DisplayAlert("�xito", "Cliente creado exitosamente.", "OK");

            if(response.id != null)
            {
                await Navigation.PushAsync(new RegstrarMembresia(response.id, nombres));
            }
        }
        else
        {
            await DisplayAlert("Error", $"{response.message}", "OK");
        }
    }

}