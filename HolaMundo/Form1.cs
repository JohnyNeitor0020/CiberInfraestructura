using System.Text.RegularExpressions;

namespace HolaMundo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnValidar_Click(object sender, EventArgs e)
        {
            //Variables
            string pass = txtPassword.Text;
            string confirm = txtConfirmPassword.Text;

            // Al menos una mayúscula, una minúscula, un número y un símbolo
            string regex = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$";

            //Primer confirmacion
            if (!Regex.IsMatch(pass, regex))
            {
                MessageBox.Show("La contraseña no cumple con los requisitos mínimos.");
                return;//si no cumple bai
            }
            //segunda confirmacion
            if (pass != confirm)
            {
                MessageBox.Show("Las contraseñas no coinciden.");
                return; // si no cumple bai
            }
            //message de exito
            MessageBox.Show("La contraseña ha sido validada");
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}